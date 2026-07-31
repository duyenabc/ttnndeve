using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IMSBackend.Data;
using IMSBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IMSBackend.Controllers
{
    [ApiController]
    [Route("api/giangvien")]
    public class GiangVienController : ControllerBase
    {
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        private readonly AppDbContext _context;

        public GiangVienController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>UC-14.2 — load diary config for a class (students also call this).</summary>
        [HttpGet("classes/{classId}/diary-config")]
        public async Task<IActionResult> GetDiaryConfig(string classId)
        {
            var cls = await FindClassAsync(classId);
            if (cls == null || string.IsNullOrWhiteSpace(cls.DiaryConfig))
                return Ok(DiaryConfigDefaults.Create());

            try
            {
                var cfg = JsonSerializer.Deserialize<DiaryConfigDto>(cls.DiaryConfig, JsonOpts);
                return Ok(NormalizeConfig(cfg) ?? DiaryConfigDefaults.Create());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IMS] diary-config parse failed for {classId}: {ex.Message}");
                return Ok(DiaryConfigDefaults.Create());
            }
        }

        /// <summary>UC-14.2 — save diary config; applies to all students in the class.</summary>
        [HttpPut("classes/{classId}/diary-config")]
        public async Task<IActionResult> PutDiaryConfig(string classId, [FromBody] DiaryConfigDto body)
        {
            if (body == null)
                return BadRequest(new { message = "Thiếu nội dung cấu hình nhật ký" });

            var (ok, error, normalized) = ValidateAndNormalize(body);
            if (!ok)
                return BadRequest(new { message = error });

            var cls = await FindClassAsync(classId);
            if (cls == null)
            {
                cls = new Class
                {
                    Id = classId,
                    MaLop = classId,
                    TenLop = $"Lớp {classId}",
                    GiangVienId = User?.FindFirst("id")?.Value
                        ?? Request.Headers["X-User-Id"].FirstOrDefault()
                        ?? "GV001",
                    SoSinhVien = 0
                };
                _context.Classes.Add(cls);
            }

            cls.DiaryConfig = JsonSerializer.Serialize(normalized, JsonOpts);
            await _context.SaveChangesAsync();

            // #region agent log
            try
            {
                Console.WriteLine(
                    $"[IMS][UC-14.2] Saved diary-config class={cls.Id} enabled={normalized.IsEnabled} min={normalized.MinPerWeek} fieldsOn={normalized.Fields.Count(f => f.IsEnabled)}");
            }
            catch { /* ignore */ }
            // #endregion

            return Ok(new
            {
                message = "Lưu cấu hình lớp thực tập thành công",
                config = normalized
            });
        }

        private async Task<Class> FindClassAsync(string classId)
        {
            if (string.IsNullOrWhiteSpace(classId)) return null;
            return await _context.Classes.FirstOrDefaultAsync(c =>
                c.Id == classId || c.MaLop == classId);
        }

        private static (bool ok, string error, DiaryConfigDto normalized) ValidateAndNormalize(DiaryConfigDto body)
        {
            var cfg = NormalizeConfig(body) ?? DiaryConfigDefaults.Create();

            if (!cfg.IsEnabled)
            {
                // When disabled, keep payload but do not require fields/min rules
                return (true, null, cfg);
            }

            // UC-14.2 / 4a — reject empty or 0 before clamping
            if (cfg.MinPerWeek < 1 || cfg.MinPerWeek > 7)
                return (false, "Tối thiểu 1 nhật ký/tuần", cfg);

            if (cfg.Fields == null || !cfg.Fields.Any(f => f.IsEnabled))
                return (false, "Form nhật ký phải có ít nhất 1 trường thông tin được bật", cfg);

            if (string.IsNullOrWhiteSpace(cfg.DeadlineTime) || !Regex.IsMatch(cfg.DeadlineTime, @"^\d{2}:\d{2}$"))
                cfg.DeadlineTime = "23:59";

            if (cfg.DeadlineDay < 0 || cfg.DeadlineDay > 6)
                cfg.DeadlineDay = 0;

            return (true, null, cfg);
        }

        private static DiaryConfigDto NormalizeConfig(DiaryConfigDto cfg)
        {
            if (cfg == null) return null;

            var defaults = DiaryConfigDefaults.Create();
            var byId = (cfg.Fields ?? new List<DiaryFieldConfigDto>())
                .Where(f => f != null && !string.IsNullOrWhiteSpace(f.Id))
                .GroupBy(f => f.Id, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            var merged = new List<DiaryFieldConfigDto>();
            foreach (var def in defaults.Fields)
            {
                if (byId.TryGetValue(def.Id, out var incoming))
                {
                    var enabled = incoming.IsEnabled;
                    merged.Add(new DiaryFieldConfigDto
                    {
                        Id = def.Id,
                        Label = string.IsNullOrWhiteSpace(incoming.Label) ? def.Label : incoming.Label,
                        IsEnabled = enabled,
                        IsRequired = enabled && incoming.IsRequired
                    });
                }
                else
                {
                    merged.Add(def);
                }
            }

            cfg.Fields = merged;
            if (string.IsNullOrWhiteSpace(cfg.DeadlineTime)) cfg.DeadlineTime = "23:59";
            return cfg;
        }
    }
}
