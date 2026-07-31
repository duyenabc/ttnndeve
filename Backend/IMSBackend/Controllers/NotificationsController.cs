using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMSBackend.Data;
using IMSBackend.Models;

namespace IMSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public NotificationsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetMine([FromQuery] string role = "ALL", [FromQuery] string userId = "ALL")
        {
            var targetUser = string.IsNullOrWhiteSpace(userId) ? "ALL" : userId;
            var items = await _db.Notifications
                .Where(n =>
                    (n.UserId == "ALL" || n.UserId == targetUser) &&
                    (n.Role == "ALL" || n.Role == role || string.IsNullOrEmpty(n.Role)))
                .OrderByDescending(n => n.NgayTao)
                .Take(100)
                .ToListAsync();

            return Ok(items.Select(MapToClient));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
        {
            var entity = new Notification
            {
                TieuDe = dto.Title,
                NoiDung = dto.Content,
                Role = dto.Role ?? "ALL",
                UserId = string.IsNullOrWhiteSpace(dto.UserId) ? "ALL" : dto.UserId,
                Icon = dto.Icon ?? "notifications",
                BgClass = dto.BgClass ?? "bg-blue-100 text-blue-700",
                Link = dto.Link ?? "",
                Type = dto.Type ?? "info",
                IsRead = false,
                NgayTao = DateTime.UtcNow
            };

            _db.Notifications.Add(entity);
            await _db.SaveChangesAsync();
            return Ok(MapToClient(entity));
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(string id, [FromBody] MarkReadDto dto)
        {
            var entity = await _db.Notifications.FindAsync(id);
            if (entity == null) return NotFound();
            entity.IsRead = dto.Read;
            await _db.SaveChangesAsync();
            return Ok(MapToClient(entity));
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllRead([FromQuery] string role = "ALL", [FromQuery] string userId = "ALL")
        {
            var targetUser = string.IsNullOrWhiteSpace(userId) ? "ALL" : userId;
            var items = await _db.Notifications
                .Where(n =>
                    !n.IsRead &&
                    (n.UserId == "ALL" || n.UserId == targetUser) &&
                    (n.Role == "ALL" || n.Role == role || string.IsNullOrEmpty(n.Role)))
                .ToListAsync();

            foreach (var item in items) item.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok(new { updated = items.Count });
        }

        [HttpDelete]
        public async Task<IActionResult> Clear([FromQuery] string role = "ALL", [FromQuery] string userId = "ALL")
        {
            var targetUser = string.IsNullOrWhiteSpace(userId) ? "ALL" : userId;
            var items = await _db.Notifications
                .Where(n =>
                    (n.UserId == "ALL" || n.UserId == targetUser) &&
                    (n.Role == "ALL" || n.Role == role || string.IsNullOrEmpty(n.Role)))
                .ToListAsync();

            _db.Notifications.RemoveRange(items);
            await _db.SaveChangesAsync();
            return Ok(new { deleted = items.Count });
        }

        private static object MapToClient(Notification n) => new
        {
            id = n.Id,
            title = n.TieuDe,
            content = n.NoiDung,
            type = n.Type,
            role = n.Role,
            userId = n.UserId,
            icon = n.Icon,
            bgClass = n.BgClass,
            link = n.Link,
            read = n.IsRead,
            createdAt = n.NgayTao.ToString("o")
        };
    }

    public class CreateNotificationDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public string Icon { get; set; }
        public string BgClass { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
    }

    public class MarkReadDto
    {
        public bool Read { get; set; } = true;
    }
}
