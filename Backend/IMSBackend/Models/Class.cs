using System;
using System.ComponentModel.DataAnnotations;

namespace IMSBackend.Models
{
    public class Class
    {
        [Key]
        public string Id { get; set; }
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string GiangVienId { get; set; }
        public int SoSinhVien { get; set; }
        public string? DiaryConfig { get; set; }
    }
}
