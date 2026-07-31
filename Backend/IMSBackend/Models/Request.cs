using System;
using System.ComponentModel.DataAnnotations;

namespace IMSBackend.Models
{
    public class Request
    {
        [Key]
        public string MaYeuCau { get; set; }
        public string MaNguoiYeuCau { get; set; }
        public string TenDoiTuong { get; set; }
        public string LoaiYeuCau { get; set; }
        public string TrangThai { get; set; } = "ChoXuLy";
        public string LyDoTuChoi { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
        public string Data { get; set; }
    }
}
