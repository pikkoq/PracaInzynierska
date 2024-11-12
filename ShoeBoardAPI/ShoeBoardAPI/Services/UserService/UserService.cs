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
        private readonly SignInManager<User> _signInManager;

        public UserService(AppDbContext context, IConfiguration configuration, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResponse<bool>> ChangeUserPassword(ChangePasswordDto userPassword, string id)
        {
            var response = new ServiceResponse<bool>();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            var result = await _userManager.ChangePasswordAsync(user, userPassword.CurrentPassword, userPassword.NewPassword);
            if( result.Succeeded)
            {
                response.Success = true;
                response.Message = "Password changed successfully";
                return response;
            }

            response.Success = false;
            response.Message = "Failed to change password";
            return response;


        }

        public async Task<ServiceResponse<bool>> EditUserData(EditUserDto userEdit, string id)
        {
            var response = new ServiceResponse<bool>();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            user.UserName = userEdit.Username ?? user.UserName;
            user.Email = userEdit.Email ?? user.Email;
            user.Bio = userEdit.Bio ?? user.Bio;
            user.ProfilePicturePath = userEdit.ProfilePicturePath == null
                ? user.ProfilePicturePath 
                : (string.IsNullOrWhiteSpace(userEdit.ProfilePicturePath)
                ? "https://icons.veryicon.com/png/o/miscellaneous/common-icons-31/default-avatar-2.png" 
                : userEdit.ProfilePicturePath);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                response.Success = true;
                response.Data = true;
                response.Message = "Updated data";
                return response;
            }

            response.Success = false;
            response.Message = "Error while updating user data";
            return response;

        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id)
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

        public async Task<ServiceResponse<GetUserDto>> GetUser(string id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var user = await _userManager.FindByIdAsync(id);

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

        public async Task<ServiceResponse<string>> LoginUser(LoginUserDto loginUser)
        {
            var response = new ServiceResponse<string>();
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user == null )
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                return response;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
            if (result.Succeeded)
            {
                response.Data = GenerateJwtToken(user);
                response.Success = true;
                response.Message = "User logged in successfully";
                return response;
            }
            response.Success = false;
            response.Message = "Invalid username or password";
            return response;

        }

        public async Task<ServiceResponse<bool>> ReigsterUser(RegisterUserDto registerUser)
        {
            var response = new ServiceResponse<bool>();

            var user = new User
            {
                UserName = registerUser.Username,
                Email = registerUser.Email,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                response.Success = true;
                response.Message = "User registered successfully";
                return response;
            }

            response.Success = false;
            response.Message = "User registration failed";
            return response;
        }
    }
}
