using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoeBoardAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse<LoginResponseDto>> LoginUser(LoginUserDto loginUser)
        {
            var response = new ServiceResponse<LoginResponseDto>();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
            {
                response.Success = false;

                response.Data = new LoginResponseDto { Token = null, Success = false};
                return response;
            }

            var token = GenerateJwtToken(user);
            response.Data = new LoginResponseDto { Token = token, Username = user.Username, Success = true };
            return response;

        }

        public async Task<ServiceResponse<RegisterResponseDto>> ReigsterUser(RegisterUserDto registerUser)
        {
            var response = new ServiceResponse<RegisterResponseDto>();

            if (await _context.Users.AnyAsync(u => u.Email == registerUser.Email))
            {
                response.Success = false;
                response.Data = new RegisterResponseDto { Message = "Email already in use.", Success = false };
                return response;
            }

            var user = new User
            {
                Username = registerUser.Username,
                Email = registerUser.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            response.Data = new RegisterResponseDto { Message = "User registered successfully.", DateTimeCreated = DateTime.Now, Success = true };
            return response;
        }
    }
}
