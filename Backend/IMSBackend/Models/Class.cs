using System;
using System.ComponentModel.DataAnnotations;

namespace IMSBackend.Models
{
    public class Class
    {
        [Key]
        public string Id { get; set; } // Equivalent to Firebase document ID (MaLop)
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string GiangVienId { get; set; }
        public int SoSinhVien { get; set; }
        public string? DiaryConfig { get; set; }
    }
}
