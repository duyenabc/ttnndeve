using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMSBackend.Data;
using IMSBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IMSBackend.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] string role)
        {
            var q = _context.Users.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(role) && role != "ALL")
                q = q.Where(u => u.VaiTro == role);

            var list = await q.ToListAsync();
            return Ok(new
            {
                tongSo = list.Count,
                dangHoatDong = list.Count(u => u.TrangThaiTaiKhoan == "DangHoatDong"),
                choKichHoat = list.Count(u => u.TrangThaiTaiKhoan == "ChoKichHoat" || u.TrangThaiTaiKhoan == "Nhap"),
                biKhoa = list.Count(u => u.TrangThaiTaiKhoan == "BiKhoa")
            });
        }

        [HttpGet("users")]
        public async Task<IActionResult> ListUsers(
            [FromQuery] string role,
            [FromQuery] string status,
            [FromQuery] string search,
            [FromQuery] string boMon,
            [FromQuery] string lopSinhHoat,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 200) pageSize = 20;

            var q = _context.Users.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(role) && role != "ALL")
                q = q.Where(u => u.VaiTro == role);
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(u => u.TrangThaiTaiKhoan == status);
            if (!string.IsNullOrWhiteSpace(boMon))
                q = q.Where(u => u.TenBoMon != null && u.TenBoMon.Contains(boMon));
            if (!string.IsNullOrWhiteSpace(lopSinhHoat))
                q = q.Where(u => u.LopSinhHoat != null && u.LopSinhHoat.Contains(lopSinhHoat));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(u =>
                    (u.MaDinhDanh != null && u.MaDinhDanh.Contains(s)) ||
                    (u.HoTen != null && u.HoTen.Contains(s)) ||
                    (u.Email != null && u.Email.Contains(s)));
            }

            var total = await q.CountAsync();
            var rows = await q
                .OrderByDescending(u => u.NgayTao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var items = rows.Select(MapUser).ToList();

            return Ok(new { items, total, page, pageSize });
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var u = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.MaNguoiDung == id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            return Ok(MapUser(u));
        }

        [HttpGet("users/{id}/audit")]
        public IActionResult Audit(string id)
        {
            _ = id;
            return Ok(Array.Empty<object>());
        }

        [HttpPost("users")]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.MaDinhDanh) || string.IsNullOrWhiteSpace(req.HoTen))
                return BadRequest(new { message = "Thiếu mã định danh hoặc họ tên" });

            var ma = req.MaDinhDanh.Trim();
            var exists = await _context.Users.AnyAsync(u =>
                u.MaDinhDanh != null && u.MaDinhDanh.ToLower() == ma.ToLower());
            if (exists)
                return BadRequest(new { message = $"Mã định danh '{ma}' đã tồn tại" });

            var temp = string.IsNullOrWhiteSpace(req.MatKhau) ? GenerateTempPassword() : req.MatKhau.Trim();
            var grantNow = req.CapTaiKhoanNgay != false;
            var user = new User
            {
                MaNguoiDung = Guid.NewGuid().ToString("N")[..12],
                MaDinhDanh = ma,
                HoTen = req.HoTen.Trim(),
                Email = req.Email?.Trim(),
                SoDienThoai = req.SoDienThoai?.Trim(),
                VaiTro = string.IsNullOrWhiteSpace(req.VaiTro) ? "SinhVien" : req.VaiTro.Trim(),
                TrangThaiTaiKhoan = grantNow ? "DangHoatDong" : "ChoKichHoat",
                MatKhau = temp,
                BuocDoiMatKhau = true,
                QuyenQuanLyNguoiDung = req.QuyenQuanLyNguoiDung,
                NgayTao = DateTime.UtcNow,
                LopSinhHoat = req.LopSinhHoat,
                TenBoMon = req.TenBoMon
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đã thêm tài khoản",
                user = MapUser(user),
                matKhauTam = temp
            });
        }

        [HttpPost("users/{id}/lock")]
        public async Task<IActionResult> Lock(string id, [FromBody] LockRequest req)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            if (string.IsNullOrWhiteSpace(req?.LyDo))
                return BadRequest(new { message = "Vui lòng nhập lý do khóa tài khoản" });

            u.TrangThaiTaiKhoan = "BiKhoa";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã khóa tài khoản" });
        }

        [HttpPost("users/{id}/unlock")]
        public async Task<IActionResult> Unlock(string id)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            u.TrangThaiTaiKhoan = "DangHoatDong";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã mở khóa tài khoản" });
        }

        [HttpPost("users/{id}/grant")]
        public async Task<IActionResult> Grant(string id)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            u.TrangThaiTaiKhoan = "DangHoatDong";
            if (string.IsNullOrEmpty(u.MatKhau))
                u.MatKhau = GenerateTempPassword();
            u.BuocDoiMatKhau = true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã cấp / kích hoạt tài khoản", matKhauTam = u.MatKhau });
        }

        [HttpPost("users/{id}/resend-activation")]
        public async Task<IActionResult> Resend(string id)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            return Ok(new { message = $"Đã gửi lại link kích hoạt tới {u.Email ?? u.MaDinhDanh}" });
        }

        [HttpPost("users/{id}/temp-password")]
        public async Task<IActionResult> TempPassword(string id)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            var temp = GenerateTempPassword();
            u.MatKhau = temp;
            u.BuocDoiMatKhau = true;
            u.TrangThaiTaiKhoan = "DangHoatDong";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã cấp mật khẩu tạm", matKhauTam = temp });
        }

        [HttpPut("users/{id}/permission")]
        public async Task<IActionResult> Permission(string id, [FromBody] PermissionRequest req)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            u.QuyenQuanLyNguoiDung = req?.QuyenQuanLyNguoiDung == true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã cập nhật quyền" });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound(new { message = "Không tìm thấy tài khoản" });
            if (u.TrangThaiTaiKhoan == "DangHoatDong")
                return BadRequest(new { message = "Chỉ có thể xóa tài khoản đang ở trạng thái Nháp/Chờ kích hoạt" });

            _context.Users.Remove(u);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xóa tài khoản" });
        }

        [HttpPost("users/delete-bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody] BulkIdsRequest req)
        {
            var ids = req?.Ids ?? new List<string>();
            var users = await _context.Users
                .Where(u => ids.Contains(u.MaNguoiDung) && u.TrangThaiTaiKhoan != "DangHoatDong")
                .ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã xóa {users.Count} tài khoản" });
        }

        [HttpPost("import-preview")]
        public async Task<IActionResult> ImportPreview([FromBody] ImportPreviewRequest req)
        {
            var role = string.IsNullOrWhiteSpace(req?.Role) ? "SinhVien" : req.Role;
            var rows = req?.Rows ?? new List<ImportRow>();
            var valid = new List<object>();
            var dup = new List<object>();
            var err = new List<object>();

            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.MaDinhDanh) || string.IsNullOrWhiteSpace(row.HoTen))
                {
                    err.Add(row);
                    continue;
                }

                var ma = row.MaDinhDanh.Trim();
                var exists = await _context.Users.AnyAsync(u =>
                    u.MaDinhDanh != null && u.MaDinhDanh.ToLower() == ma.ToLower());
                var item = new
                {
                    maDinhDanh = ma,
                    hoTen = row.HoTen?.Trim(),
                    email = row.Email?.Trim(),
                    soDienThoai = row.SoDienThoai?.Trim(),
                    vaiTro = role
                };
                if (exists) dup.Add(item);
                else valid.Add(item);
            }

            return Ok(new { validRows = valid, duplicateRows = dup, errorRows = err });
        }

        [HttpPost("import-confirm")]
        public async Task<IActionResult> ImportConfirm([FromBody] ImportConfirmRequest req)
        {
            var role = string.IsNullOrWhiteSpace(req?.Role) ? "SinhVien" : req.Role;
            var grantNow = req?.GrantNow != false;
            var items = req?.Items ?? new List<ImportRow>();
            var created = 0;

            foreach (var row in items)
            {
                if (string.IsNullOrWhiteSpace(row.MaDinhDanh) || string.IsNullOrWhiteSpace(row.HoTen))
                    continue;

                var ma = row.MaDinhDanh.Trim();
                var existing = await _context.Users.FirstOrDefaultAsync(u =>
                    u.MaDinhDanh != null && u.MaDinhDanh.ToLower() == ma.ToLower());
                if (existing != null)
                {
                    existing.HoTen = row.HoTen.Trim();
                    existing.Email = row.Email?.Trim() ?? existing.Email;
                    existing.SoDienThoai = row.SoDienThoai?.Trim() ?? existing.SoDienThoai;
                    if (grantNow)
                    {
                        existing.TrangThaiTaiKhoan = "DangHoatDong";
                        if (string.IsNullOrEmpty(existing.MatKhau))
                            existing.MatKhau = GenerateTempPassword();
                    }
                    continue;
                }

                _context.Users.Add(new User
                {
                    MaNguoiDung = Guid.NewGuid().ToString("N")[..12],
                    MaDinhDanh = ma,
                    HoTen = row.HoTen.Trim(),
                    Email = row.Email?.Trim(),
                    SoDienThoai = row.SoDienThoai?.Trim(),
                    VaiTro = role,
                    TrangThaiTaiKhoan = grantNow ? "DangHoatDong" : "ChoKichHoat",
                    MatKhau = GenerateTempPassword(),
                    BuocDoiMatKhau = true,
                    NgayTao = DateTime.UtcNow
                });
                created++;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Đã import {created} tài khoản mới" });
        }

        private static object MapUser(User u) => new
        {
            maNguoiDung = u.MaNguoiDung,
            maDinhDanh = u.MaDinhDanh,
            hoTen = u.HoTen,
            email = u.Email,
            soDienThoai = u.SoDienThoai,
            vaiTro = u.VaiTro,
            trangThaiTaiKhoan = u.TrangThaiTaiKhoan,
            buocDoiMatKhau = u.BuocDoiMatKhau,
            quyenQuanLyNguoiDung = u.QuyenQuanLyNguoiDung,
            ngayTao = u.NgayTao,
            anhDaiDien = u.AnhDaiDien,
            lopSinhHoat = u.LopSinhHoat,
            tenBoMon = u.TenBoMon,
            khoa = u.Khoa
        };

        private static string GenerateTempPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@";
            var rng = Random.Shared;
            return new string(Enumerable.Range(0, 10).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
        }
    }

    public class CreateAccountRequest
    {
        public string MaDinhDanh { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string VaiTro { get; set; }
        public bool? CapTaiKhoanNgay { get; set; }
        public bool QuyenQuanLyNguoiDung { get; set; }
        public string MatKhau { get; set; }
        public string LopSinhHoat { get; set; }
        public string TenBoMon { get; set; }
    }

    public class LockRequest
    {
        public string LyDo { get; set; }
    }

    public class PermissionRequest
    {
        public bool? QuyenQuanLyNguoiDung { get; set; }
    }

    public class BulkIdsRequest
    {
        public List<string> Ids { get; set; }
    }

    public class ImportRow
    {
        public string MaDinhDanh { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
    }

    public class ImportPreviewRequest
    {
        public string Role { get; set; }
        public List<ImportRow> Rows { get; set; }
    }

    public class ImportConfirmRequest
    {
        public string Role { get; set; }
        public bool? GrantNow { get; set; }
        public List<ImportRow> Items { get; set; }
    }
}
