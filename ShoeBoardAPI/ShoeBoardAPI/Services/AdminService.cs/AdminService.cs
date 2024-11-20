using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.AdminDtos;
using ShoeBoardAPI.Models.DTO.PostDtos;

namespace ShoeBoardAPI.Services.AdminService.cs
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> AccecptNewAddedShoes(int shoeId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var shoe = await _context.UserShoeCatalogs.FindAsync(shoeId);
                if( shoe == null )
                {
                    response.Success = false;
                    response.Message = "Shoe not found";
                    response.Data = false;
                    return response;
                }

                var acceptedShoe = _mapper.Map<ShoeCatalog>(shoe);
                await _context.ShoeCatalogs.AddAsync(acceptedShoe);
                _context.UserShoeCatalogs.Remove(shoe);

                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = true;
                response.Message = "Accepted shoe";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error accepting shoe: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeclineNewAddedShoes(int shoeId)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var shoe = await _context.UserShoeCatalogs.FindAsync(shoeId);
                if (shoe == null)
                {
                    response.Success = false;
                    response.Data= false;
                    response.Message = "Shoe not found.";
                    return response;
                }

                _context.UserShoeCatalogs.Remove(shoe);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Declined new shoe";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error declining shoe: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteUserAccount(string userId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "User not found.";
                    return response;
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Deleted user";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error deleting user: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteUserPost(int postId)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var post = await _context.Posts.FindAsync(postId);
                if (post == null)
                {
                    response.Success = false;
                    response.Message = "Post not found.";
                    return response;
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Delted user post.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error deleting post: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> EditNewAddedShoes(int shoeId, EditNewAddedShoesDto editShoeDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var shoe = await _context.UserShoeCatalogs.FindAsync(shoeId);
                if (shoe == null)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "Shoe not found.";
                    return response;
                }

                _mapper.Map(editShoeDto, shoe);

                _context.UserShoeCatalogs.Update(shoe);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Edited shoe data.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error editing shoe: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> EditUserAccount(string userId, EditUserAccountDto editUserDto)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "User not found.";
                    return response;
                }

                _mapper.Map(editUserDto, user);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Edited user data.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error editing user account: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> EditUserPost(int postId, string content)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var post = await _context.Posts.FindAsync(postId);
                if (post == null)
                {
                    response.Success = false;
                    response.Message = "Post not found.";
                    return response;
                }

                _mapper.Map(content, post);

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Edited user post content.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = $"Error editing post: {ex.Message}";
            }
            return response;
        }

        public async Task<SearchServiseResponse<List<GetAllUsersDto>>> GetAllUsers(int pageNumber = 1)
        {
            var response = new SearchServiseResponse<List<GetAllUsersDto>>();
            int pageSize = 10;

            try
            {
                var totalCount = await _context.Users.CountAsync();
                var users = await _context.Users
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = _mapper.Map<List<GetAllUsersDto>>(users);

                response.Data = userDtos;
                response.Success = true;
                response.Message = "Retrieved shoes.";
                response.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                response.TotalCount = totalCount;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving users: {ex.Message}";
            }
            return response;
        }

        public async Task<SearchServiseResponse<List<GetAllUsersPostsDto>>> GetAllUsersPosts(int pageNumber = 1)
        {
            var response = new SearchServiseResponse<List<GetAllUsersPostsDto>>();
            int pageSize = 10;
            try
            {
                var totalCount = await _context.Posts.CountAsync();
                var posts = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Shoe)
                    .ThenInclude(s => s.ShoeCatalog)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments)
                    .OrderByDescending(p => p.DatePosted)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var postDtos = _mapper.Map<List<GetAllUsersPostsDto>>(posts);

                response.Data = postDtos;
                response.Message = "Retrived users.";
                response.Success = true;
                response.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                response.TotalCount = totalCount;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving posts: {ex.Message}";
            }
            return response;
        }

        public async Task<SearchServiseResponse<List<GetShoesToAcceptDto>>> GetShoesToAccept(int pageNumber = 1)
        {
            var response = new SearchServiseResponse<List<GetShoesToAcceptDto>>();
            int pageSize = 20;

            try
            {
                var totalCount = await _context.UserShoeCatalogs.CountAsync();
                var shoes = await _context.UserShoeCatalogs
                    .Include(u => u.User)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                response.Data = _mapper.Map<List<GetShoesToAcceptDto>>(shoes);
                response.TotalCount = totalCount;
                response.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                response.Message = "Successfully retrieved shoes to accept.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving shoes to accept: {ex.Message}";
            }
            return response;
        }
    }
}
