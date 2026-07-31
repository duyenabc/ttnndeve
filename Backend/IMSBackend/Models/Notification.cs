using System;
using System.ComponentModel.DataAnnotations;

namespace IMSBackend.Models
{
    public class Notification
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string Type { get; set; } = "info";
        public string Role { get; set; } = "ALL";
        public string UserId { get; set; } = "ALL";
        public string Icon { get; set; } = "notifications";
        public string BgClass { get; set; } = "bg-blue-100 text-blue-700";
        public string Link { get; set; } = "";
        public bool IsRead { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
    }
}
