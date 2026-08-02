using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IMSBackend.Data;
using IMSBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IMSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                if (req == null || string.IsNullOrEmpty(req.MaDinhDanh))
                    return BadRequest(new { message = "Vui lòng nhập mã định danh hoặc email" });

                if (string.IsNullOrEmpty(req.MatKhau))
                    return BadRequest(new { message = "Vui lòng nhập mật khẩu" });

                var input = req.MaDinhDanh.Trim().ToLowerInvariant();

                // ILike avoids EF/Npgsql ToLower translation edge cases
                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    (u.MaDinhDanh != null && EF.Functions.ILike(u.MaDinhDanh, input)) ||
                    (u.Email != null && EF.Functions.ILike(u.Email, input)));

                if (user == null)
                    return Unauthorized(new { message = "Mã định danh hoặc mật khẩu không đúng" });

                if (user.TrangThaiTaiKhoan == "BiKhoa")
                    return Unauthorized(new { message = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên" });

                if (!string.IsNullOrEmpty(user.MatKhau) && user.MatKhau != req.MatKhau)
                    return Unauthorized(new { message = "Mã định danh hoặc mật khẩu không đúng" });

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _config["Jwt:Key"]
                    ?? Environment.GetEnvironmentVariable("Jwt__Key")
                    ?? "SuperSecretKey_For_Development_IMS_12345";
                var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
                if (keyBytes.Length < 32)
                    keyBytes = Encoding.UTF8.GetBytes(jwtKey.PadRight(32, '0'));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("id", user.MaNguoiDung),
                        new Claim(ClaimTypes.Role, user.VaiTro ?? "SinhVien")
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);

                var redirectTo = "/dashboard";
                if (user.BuocDoiMatKhau) redirectTo = "/change-password";
                else if (user.VaiTro == "Admin") redirectTo = "/admin/accounts";

                var userPayload = new
                {
                    maNguoiDung = user.MaNguoiDung,
                    maDinhDanh = user.MaDinhDanh,
                    hoTen = user.HoTen,
                    email = user.Email,
                    vaiTro = user.VaiTro,
                    trangThaiTaiKhoan = user.TrangThaiTaiKhoan,
                    buocDoiMatKhau = user.BuocDoiMatKhau,
                    quyenQuanLyNguoiDung = user.QuyenQuanLyNguoiDung
                };

                return Ok(new
                {
                    accessToken,
                    refreshToken = "mock_refresh_token",
                    redirectTo,
                    user = userPayload
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IMS] Login error: {ex}");
                return StatusCode(500, new
                {
                    message = "Không đăng nhập được do lỗi máy chủ (database).",
                    detail = ex.GetType().Name
                });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe([FromQuery] string id)
        {
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var userId = !string.IsNullOrEmpty(claimId) ? claimId : id;
            
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user != null) return Ok(user);
            return NotFound();
        }
        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Đã đăng xuất" });
        }
    }

    public class LoginRequest
    {
        public string MaDinhDanh { get; set; }
        public string MatKhau { get; set; }
    }
}
