using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(AppDbContext context, IConfiguration configuration, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<bool>> ChangeUserPassword(ChangePasswordDto userPassword, int id)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            if( !BCrypt.Net.BCrypt.Verify(userPassword.CurrentPassword, user.PasswordHash))
            {
                response.Success = false;
                response.Message = "Incorrect password";
                return response;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userPassword.NewPassword);

            try
            {
                await _context.SaveChangesAsync();
                response.Data = true;
                response.Success = true;
                response.Message = "Password changed";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while chaning password: {ex.Message}";
                response.Data = false;
            }
            return response;


        }

        public async Task<ServiceResponse<bool>> EditUserData(EditUserDto userEdit, int id)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            user.Username = userEdit.Username ?? user.Username;
            user.Email = userEdit.Email ?? user.Email;
            user.Bio = userEdit.Bio ?? user.Bio;
            user.ProilePicturePath = userEdit.ProilePicturePath ?? user.ProilePicturePath;

            try
            {
                await _context.SaveChangesAsync();
                response.Data = true;
                response.Message = "Updated data";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while updating user data: {ex.Message}";
                response.Data = false;
            }
            return response;

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

        public async Task<ServiceResponse<GetUserDto>> GetUser(int id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            response.Data = _mapper.Map<GetUserDto>(user);
            response.Message = "User has been found";
            return response;
        }

        public async Task<ServiceResponse<LoginResponseDto>> LoginUser(LoginUserDto loginUser)
        {
            var response = new ServiceResponse<LoginResponseDto>();
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
            {
                response.Success = false;

                response.Data = new LoginResponseDto { Token = null, Success = false};
                response.Message = "Failed to login";
                return response;
            }

            var token = GenerateJwtToken(user);
            response.Data = new LoginResponseDto { Token = token, Username = user.Username, Success = true };
            response.Message = "Succesfully logged in";
            return response;

        }

        public async Task<ServiceResponse<RegisterResponseDto>> ReigsterUser(RegisterUserDto registerUser)
        {
            var response = new ServiceResponse<RegisterResponseDto>();

            if (await _context.Users.AnyAsync(u => u.Email == registerUser.Email))
            {
                response.Success = false;
                response.Data = new RegisterResponseDto { Message = "Email already in use.", Success = false };
                response.Message = response.Data.Message;
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
            response.Message = response.Data.Message;
            return response;
        }
    }
}
