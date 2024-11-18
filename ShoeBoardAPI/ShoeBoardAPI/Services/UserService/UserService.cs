using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.PostDtos;
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

            if (!string.IsNullOrWhiteSpace(userEdit.Username) && userEdit.Username != user.UserName)
            {
                var userWithSameUsername = await _userManager.FindByNameAsync(userEdit.Username);
                if(userWithSameUsername != null && userWithSameUsername.Id != user.Id)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "Username is already taken.";
                    return response;
                }
            }

            if (!string.IsNullOrWhiteSpace(userEdit.Email) && userEdit.Email != user.Email)
            {
                var userWithSameEmail = await _userManager.FindByEmailAsync(userEdit.Email);
                if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "Email is already in use.";
                    return response;
                }
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

        public async Task<string> GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id),
                new Claim("username", user.UserName),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ServiceResponse<GetUserProfileDto>> GetProfile(string userName, string currentUserId)
        {
            var response = new ServiceResponse<GetUserProfileDto>();

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            var userProfile = await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => new GetUserProfileDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    UserProfileAvatar = u.ProfilePicturePath,
                    Bio = u.Bio,
                    Posts = u.Posts
                        .OrderByDescending(p => p.DatePosted)
                        .Select(p => new PostDto
                        {
                            Id = p.Id,
                            Username = p.User.UserName,
                            Content = p.Content,
                            DatePosted = p.DatePosted,
                            Size = p.Shoe.Size,
                            ComfortRating = p.Shoe.ComfortRating,
                            StyleRating = p.Shoe.StyleRating,
                            Season = p.Shoe.Season,
                            Review = p.Shoe.Review,
                            Image_Url = p.Shoe.ShoeCatalog.Image_Url,
                            Title = p.Shoe.ShoeCatalog.Title,
                            IsLiked = p.Likes.Any(l => l.UserId == u.Id),
                            CommentsCount = p.Comments.Count,
                            LikeCount = p.Likes.Count,
                        }).ToList(),
                    IsFriend = _context.Friends.Any(f =>
                        (f.UserId == currentUserId && f.FriendId == u.Id) ||
                        (f.UserId == u.Id && f.FriendId == currentUserId)),
                    IsRequestSent = _context.FriendRequests.Any(fs =>
                        fs.RequesterId == currentUserId && fs.ReceiverId == u.Id && !fs.IsAccepted),
                    IsRequestRecived = _context.FriendRequests.Any(fr =>
                        fr.ReceiverId == currentUserId && fr.RequesterId == u.Id && !fr.IsAccepted),
                    RequestId = _context.FriendRequests
                        .Where(r => (r.RequesterId == currentUserId && r.ReceiverId == u.Id && !r.IsAccepted) ||
                        (r.ReceiverId == currentUserId && r.RequesterId == u.Id && !r.IsAccepted))
                        .Select(r => r.Id)
                        .FirstOrDefault()
                }).FirstOrDefaultAsync();

            response.Data = userProfile;
            response.Success = true;
            response.Message = "Retrived user profile data.";
            return response;

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
            User user;

            if (loginUser.Login.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginUser.Login);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginUser.Login);
            }

            if (user == null )
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                return response;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
            if (result.Succeeded)
            {
                response.Data = await GenerateJwtToken(user);
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

            if (await _userManager.FindByNameAsync(registerUser.Username) != null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Username is already taken.";
                return response;
            }

            if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Email is already in use";
                return response;
            }

            var user = new User
            {
                UserName = registerUser.Username,
                Email = registerUser.Email,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
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
