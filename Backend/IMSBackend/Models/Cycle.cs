using System;
using System.ComponentModel.DataAnnotations;

namespace IMSBackend.Models
{
    public class Cycle
    {
        [Key]
        public string MaKy { get; set; }
        public string TenKy { get; set; }
        public string LoaiThucTap { get; set; }
        public string HocKy { get; set; }
        public string NamHoc { get; set; }
        public string TrangThai { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
    }
}
