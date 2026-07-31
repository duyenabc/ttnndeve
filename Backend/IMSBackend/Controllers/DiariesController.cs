using System;
using System.Linq;
using System.Threading.Tasks;
using IMSBackend.Data;
using IMSBackend.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut("{id}/feedback")]
        public async Task<IActionResult> AddFeedback(string id, [FromBody] FeedbackRequest req)
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null) return NotFound();

            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                DiaryId = id,
                TeacherName = req.TeacherName,
                Content = req.Content,
                Timestamp = DateTime.UtcNow
            };

            diary.IsReadByTeacher = true;
            diary.IsReadByStudent = false;
            diary.NgayCapNhat = DateTime.UtcNow;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Đã gửi phản hồi" });
        }
        
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(string id)
        {
            var diary = await _context.Diaries.FindAsync(id);
            if (diary == null) return NotFound();
            
            diary.IsReadByTeacher = true; // or Student depending on role
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã đánh dấu đọc" });
        }
    }

    public class FeedbackRequest
    {
        public string TeacherName { get; set; }
        public string Content { get; set; }
    }
}
