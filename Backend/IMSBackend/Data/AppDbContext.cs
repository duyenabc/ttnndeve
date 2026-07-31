using Microsoft.EntityFrameworkCore;
using IMSBackend.Models;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace IMSBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Diary> Diaries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Diary>()
                .HasMany(d => d.Feedbacks)
                .WithOne(f => f.Diary)
                .HasForeignKey(f => f.DiaryId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User 
                {
                    MaNguoiDung = "admin_001",
                    MaDinhDanh = "admin",
                    HoTen = "Quản trị viên",
                    Email = "admin@example.com",
                    VaiTro = "Admin",
                    TrangThaiTaiKhoan = "DangHoatDong",
                    MatKhau = "Admin@123",
                    BuocDoiMatKhau = false,
                    QuyenQuanLyNguoiDung = true,
                    NgayTao = DateTime.UtcNow
                },
                new User 
                {
                    MaNguoiDung = "gv_001",
                    MaDinhDanh = "GV001",
                    HoTen = "ThS. Lê Hoàng Nam",
                    Email = "namlh@example.com",
                    VaiTro = "GiangVien",
                    TrangThaiTaiKhoan = "DangHoatDong",
                    MatKhau = "Gv@12345",
                    BuocDoiMatKhau = false,
                    NgayTao = DateTime.UtcNow
                },
                new User 
                {
                    MaNguoiDung = "sv_001",
                    MaDinhDanh = "SV001",
                    HoTen = "Nguyễn Văn A",
                    Email = "sv001@example.com",
                    VaiTro = "SinhVien",
                    TrangThaiTaiKhoan = "DangHoatDong",
                    MatKhau = "Sv@12345",
                    BuocDoiMatKhau = false,
                    NgayTao = DateTime.UtcNow,
                    LopSinhHoat = "K64-CNTT"
                }
            );
            
            // Seed Classes
            modelBuilder.Entity<Class>().HasData(
                new Class
                {
                    Id = "L001",
                    MaLop = "L001",
                    TenLop = "Lớp Thực tập K64",
                    GiangVienId = "GV001",
                    SoSinhVien = 35
                }
            );
        }
    }
}
