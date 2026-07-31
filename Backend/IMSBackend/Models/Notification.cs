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
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
    }
}
