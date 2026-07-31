using System;
using System.ComponentModel.DataAnnotations;

namespace IMSBackend.Models
{
    public class User
    {
        [Key]
        public string MaNguoiDung { get; set; }
        public string MaDinhDanh { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string VaiTro { get; set; } = "SinhVien";
        public string TrangThaiTaiKhoan { get; set; } = "Nhap";
        public string MatKhau { get; set; }
        public bool BuocDoiMatKhau { get; set; }
        public bool QuyenQuanLyNguoiDung { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.UtcNow;
        
        public string AnhDaiDien { get; set; }
        public string LopSinhHoat { get; set; }
        public string TenBoMon { get; set; }
        public string Khoa { get; set; }
    }
}
