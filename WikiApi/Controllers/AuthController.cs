using CNSAWiki.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SharedData.DTOs;
using WikiApi.Data;
using WikiApi.Services;

namespace WikiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AuthService _auth;

        public AuthController(AppDbContext db, AuthService auth)
        {
            _db = db;
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("이미 존재하는 사용자입니다.");

            _auth.CreatePasswordHash(dto.Password, out var hash, out var salt);

            var user = new UserInfo
            {
                Username = dto.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("회원가입 성공");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return BadRequest("존재하지 않는 사용자");

            if (!_auth.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("비밀번호가 틀렸습니다.");

            var token = _auth.CreateToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new { username = User.Identity.Name });
        }
    }
}
