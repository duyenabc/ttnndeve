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
    [Route("api/[controller]")]
    public class DiariesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DiariesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiaries([FromQuery] string userId, [FromQuery] string classId, [FromQuery] string week)
        {
            var query = _context.Diaries.Include(d => d.Feedbacks).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(d => d.UserId == userId);
            
            if (!string.IsNullOrEmpty(classId))
                query = query.Where(d => d.ClassId == classId);
                
            if (!string.IsNullOrEmpty(week))
                query = query.Where(d => d.Week == week);

            var list = await query.ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiary([FromBody] Diary diary)
        {
            diary.Id = Guid.NewGuid().ToString();
            diary.NgayTao = DateTime.UtcNow;
            diary.NgayCapNhat = DateTime.UtcNow;
            
            _context.Diaries.Add(diary);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Đã lưu nhật ký", diary });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiary(string id, [FromBody] Diary diaryData)
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null) return NotFound();

            diary.CompletionLevel = diaryData.CompletionLevel;
            diary.Feeling = diaryData.Feeling;
            diary.TaskDescription = diaryData.TaskDescription;
            diary.NewKnowledge = diaryData.NewKnowledge;
            diary.Issues = diaryData.Issues;
            diary.Solutions = diaryData.Solutions;
            diary.NextPlan = diaryData.NextPlan;
            diary.SupportNeeded = diaryData.SupportNeeded;
            diary.Evidence = diaryData.Evidence;
            diary.Status = diaryData.Status;
            diary.NgayCapNhat = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã cập nhật nhật ký" });
        }

        /// <summary>UC-18.3</summary>
        [HttpPut("{id}/feedback")]
        public async Task<IActionResult> AddFeedback(string id, [FromBody] FeedbackRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Content))
                return BadRequest(new { message = "Nội dung nhận xét không được để trống" });

            var diary = await _context.Diaries.Include(d => d.Feedbacks).FirstOrDefaultAsync(d => d.Id == id);
            if (diary == null) return NotFound(new { message = "Không tìm thấy nhật ký" });

            if (!string.Equals(diary.Status, "Submitted", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = "Chỉ nhận xét nhật ký đã nộp" });

            var teacherName = string.IsNullOrWhiteSpace(req.TeacherName) ? "GVHD" : req.TeacherName.Trim();
            var content = req.Content.Trim();
            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                DiaryId = id,
                TeacherName = teacherName,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            diary.IsReadByTeacher = true;
            diary.IsReadByStudent = false;
            diary.NgayCapNhat = DateTime.UtcNow;

            _context.Feedbacks.Add(feedback);

            var diaryDate = (diary.NgayTao == default ? DateTime.UtcNow : diary.NgayTao).ToLocalTime();
            var dateLabel = diaryDate.ToString("dd/MM/yyyy");
            var weekLabel = string.IsNullOrWhiteSpace(diary.Week) ? "?" : diary.Week;
            var notifyText = $"{teacherName} đã nhận xét nhật ký {dateLabel} - Tuần {weekLabel}.";

            _context.Notifications.Add(new Notification
            {
                TieuDe = "Phản hồi nhật ký thực tập",
                NoiDung = notifyText,
                Type = "diary_feedback",
                Role = "SinhVien",
                UserId = diary.UserId ?? "ALL",
                Icon = "rate_review",
                BgClass = "bg-amber-100 text-amber-800",
                Link = "/student/progress",
                IsRead = false,
                NgayTao = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Đã gửi phản hồi",
                feedback,
                notification = notifyText
            });
        }

        /// <summary>UC-18.3 feedback history</summary>
        [HttpGet("feedback-history")]
        public async Task<IActionResult> GetFeedbackHistory([FromQuery] string userId, [FromQuery] string classId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "Thiếu userId" });

            var query = _context.Diaries
                .AsNoTracking()
                .Include(d => d.Feedbacks)
                .Where(d => d.UserId == userId && d.Status == "Submitted");

            if (!string.IsNullOrWhiteSpace(classId))
                query = query.Where(d => d.ClassId == classId);

            var diaries = await query.ToListAsync();
            var history = diaries
                .SelectMany(d => (d.Feedbacks ?? new List<Feedback>()).Select(f => new
                {
                    id = f.Id,
                    diaryId = d.Id,
                    week = d.Week,
                    diaryDate = d.NgayTao,
                    teacherName = f.TeacherName,
                    content = f.Content,
                    timestamp = f.Timestamp,
                    classId = d.ClassId
                }))
                .OrderByDescending(x => x.timestamp)
                .ToList();

            return Ok(history);
        }
        
        /// <summary>UC-18.2 mark read</summary>
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(string id, [FromQuery] string by = "teacher")
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null) return NotFound();

            if (string.Equals(by, "student", StringComparison.OrdinalIgnoreCase))
                diary.IsReadByStudent = true;
            else
                diary.IsReadByTeacher = true;

            diary.NgayCapNhat = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Đã đánh dấu đọc",
                isReadByTeacher = diary.IsReadByTeacher,
                isReadByStudent = diary.IsReadByStudent
            });
        }
    }

    public class FeedbackRequest
    {
        public string TeacherName { get; set; }
        public string Content { get; set; }
    }
}
