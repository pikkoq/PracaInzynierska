using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using System.Security.Claims;
using System.Drawing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace ShoeBoardAPI.Services.ShoeService
{
    public class ShoeService : IShoeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ShoeService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<bool>> DeleteUserShoe(int shoeId, string userId)
        {
            var response = new ServiceResponse<bool>();
            var shoe = await _context.Shoes.FindAsync(shoeId);

            if (shoe == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "User shoe not found.";
                return response;
            }

            if (shoe.UserId != userId)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "You can delete only yours shoes!";
                return response;
            }

            _context.Shoes.Remove(shoe);
            var result = await _context.SaveChangesAsync();
            response.Success = result > 0;
            response.Data = response.Success ? true : false;
            response.Message = response.Success ? "Shoe delted." : "Failed to delete shoe.";
            return response;
        }

        public async Task<ServiceResponse<bool>> EditUserShoe(int shoeId, EditUserShoeDto updatedShoe, string userId)
        {
            var response = new ServiceResponse<bool>();

            var shoe = await _context.Shoes.FindAsync(shoeId);

            if (shoe == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Shoe not found.";
                return response;
            }

            if (shoe.UserId != userId)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "You can edit only yours shoes!";
                return response;
            }

            _mapper.Map(updatedShoe, shoe);
            var result = await _context.SaveChangesAsync();

            response.Success = result > 0;
            response.Data = response.Success ? true : false;
            response.Message = response.Success ? "Shoe updated." : "Failed to update shoe.";
            return response;
        }

        public async Task<ServiceResponse<List<GetAllUserShoesDto>>> GetAllUserShoes(string userId)
        {
            var response = new ServiceResponse<List<GetAllUserShoesDto>>();

            var userShoes = await _context.Shoes
                .Where(s => s.UserId == userId)
                .Include(s => s.ShoeCatalog)
                .Select(s => new GetAllUserShoesDto
                {
                    Id = s.Id,
                    Title = s.ShoeCatalog.Title,
                    Gender =  s.ShoeCatalog.Gender ,
                    Image_Url = s.ShoeCatalog.Image_Url,
                    Size = s.Size,
                    DateAdded = s.DateAdded
                }
                ).ToListAsync();

            response.Data = userShoes;
            response.Success = true;
            response.Message = "Successfully retrived user shoes";
            return response;
        }

        public async Task<ServiceResponse<GetCatalogShoeDetailsDto>> GetCatalogShoeDetails(int catalogShoeId)
        {
            var response = new ServiceResponse<GetCatalogShoeDetailsDto>();

            var shoe = await _context.ShoeCatalogs
                .Where(s => s.Id == catalogShoeId)
                .Select(s => new GetCatalogShoeDetailsDto
                {
                    Model_No = s.Model_No,
                    Title = s.Title,
                    Nickname = s.Nickname,
                    Brand = s.Brand,
                    Series = s.Series,
                    ShopLink = s.Url_Link_Handler,
                    Gender = s.Gender,
                    ImageUrl = s.Image_Url,
                    ReleaseDate = s.Release_Date,
                    MainColor = s.Main_Color,
                    Colorway = s.Colorway,
                    Price = s.Price
                })
                .FirstOrDefaultAsync();

            if(shoe == null)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Failed to retrive details data";
                return response;
            }

            response.Data = shoe;
            response.Success = true;
            response.Message = "Successfully retrived user shoes";
            return response;
        }

        public async Task<ServiceResponse<GetShoeDetailsDto>> GetShoeDetails(int shoeId)
        {
            var response = new ServiceResponse<GetShoeDetailsDto>();
            var shoe = await _context.Shoes
                .Include(s => s.ShoeCatalog)
                .Where(s => s.Id == shoeId)
                .Select(s => new GetShoeDetailsDto
                {
                    Id = s.Id,
                    Model_No = s.ShoeCatalog.Model_No,
                    Title =s.ShoeCatalog.Title,
                    Nickname = s.ShoeCatalog.Nickname,
                    Brand =  s.ShoeCatalog.Brand,
                    Series =  s.ShoeCatalog.Series,
                    Url_Link_Handler = s.ShoeCatalog.Url_Link_Handler,
                    Gender =  s.ShoeCatalog.Gender,
                    Image_Url = s.ShoeCatalog.Image_Url,
                    Release_Date = s.ShoeCatalog.Release_Date,
                    Main_Color = s.ShoeCatalog.Main_Color,
                    Colorway = s.ShoeCatalog.Colorway,
                    Price = s.ShoeCatalog.Price,
                    Size = s.Size,
                    ComfortRating = s.ComfortRating,
                    StyleRating = s.ComfortRating,
                    Season = s.Season,
                    Review = s.Review,
                    DateAdded = s.DateAdded,
                }).FirstOrDefaultAsync();

            if (shoe == null)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Shoe not found";
                return response;
            }

            response.Data = shoe;
            response.Success = true;
            response.Message = "Shoe details retrived successfully.";
            return response;
        }

        public async Task<ServiceResponse<List<TopPopularShoesDto>>> GetTopPoulrShoes()
        {
            var response = new ServiceResponse<List<TopPopularShoesDto>>();

            var topShoes = await _context.Shoes
                .GroupBy(s => s.ShoeCatalogId)
                .Select(g => new TopPopularShoesDto
                {
                    Id = g.Key,
                    Title = g.First().ShoeCatalog.Title,
                    Count = g.Count(),
                })
                .OrderByDescending(s => s.Count)
                .Take(8)
                .ToListAsync();

            if (topShoes == null)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Failed to get popular shoes";
                return response;
            }

            response.Data = topShoes;
            response.Success = true;
            response.Message = "Successfully retrived popular shoes";
            return response;

        }

        public async Task<ServiceResponse<bool>> NewShoeRegistry(NewShoeRegistryDto newShoe, string userId)
        {
            var response = new ServiceResponse<bool>();
            var shoeCatalog = _mapper.Map<UserShoeCatalog>(newShoe);

            if(string.IsNullOrEmpty(shoeCatalog.Title) || string.IsNullOrEmpty(shoeCatalog.Main_Color) ||
                string.IsNullOrEmpty(shoeCatalog.Brand) || string.IsNullOrEmpty(shoeCatalog.Nickname) || 
                string.IsNullOrEmpty(shoeCatalog.Gender) || string.IsNullOrEmpty(shoeCatalog.Series) ||
                shoeCatalog.Image_Path == null)
            {
                response.Success = false;
                response.Message = "Title, Brand, Nickname, Gender, Main_color, Series and Image can't be empty.";
                return response;
            }

            shoeCatalog.UserId = userId;

            if (newShoe.ImageFile != null)
            {
                if (!newShoe.ImageFile.ContentType.StartsWith("image/"))
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "The file must be an image";
                    return response;
                }

                try
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(newShoe.ImageFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var image = await SixLabors.ImageSharp.Image.LoadAsync(newShoe.ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new SixLabors.ImageSharp.Size(480, 480),
                            Mode = ResizeMode.Max

                        }));

                        await using(var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.SaveAsync(fileStream, new JpegEncoder
                            {
                                Quality = 90
                            });
                        }
                    }

                    shoeCatalog.Image_Path = $"/uploads/{fileName}";
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = $"Image processing failed: {ex.Message}";
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Image is required.";
                return response;
            }

            await _context.UserShoeCatalogs.AddAsync(shoeCatalog);
            var result = await _context.SaveChangesAsync();

            response.Success = result > 0;
            if (response.Success)
            {
                response.Message = "New shoe added to catalog.";
                response.Data = true;
                return response;
            }
            response.Message = "Failed to add new shoe";
            response.Data = false;
            return response;
        }

        public async Task<SearchServiseResponse<List<GetShoeSearchDto>>> SearchShoes(string searchTerm, int pageNumber = 1)
        {
            var response = new SearchServiseResponse<List<GetShoeSearchDto>>();
            int pageSize = 40;
            int offset = (pageNumber - 1) * pageSize;

            string BuildSearchQuery(string baseQuery, string searchTerm)
            {
                var searchWords = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var searchConditions = string.Join(" AND ", searchWords.Select(word => $"CONTAINS((Title, Brand, Model_No, Nickname, Series, Main_Color), '\"{word}\"')"));

                return $"{baseQuery} WHERE {searchConditions}";
            }

            var shoeCatalogQuery = @"
                SELECT *
                FROM ShoeCatalogs
                ";

            var shoeCatalogQueryCount = @"
                SELECT Title
                FROM ShoeCatalogs
                ";

            var dynamicShoeCatalogQuery = BuildSearchQuery(shoeCatalogQuery, searchTerm) + $" ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY;";

            var shoeCatalog = await _context.ShoeCatalogs
                .FromSqlRaw(dynamicShoeCatalogQuery)
                .ToListAsync();

            var totalCountShoeCatalog = await _context.ShoeCatalogs
                .FromSqlRaw(BuildSearchQuery(shoeCatalogQueryCount, searchTerm))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalCountShoeCatalog / pageSize);

            var catalogShoeDto = _mapper.Map<List<GetShoeSearchDto>>(shoeCatalog);

            response.Data = catalogShoeDto;
            response.TotalCount = totalCountShoeCatalog;
            response.TotalPages = totalPages;
            response.Message = "Search results retrieved successfully.";
            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<bool>> SignShoeToUser(SignShoeToUserDto newShoe, int shoeCatalogId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var shoe = new Shoe
            {
                UserId = userId,
                ShoeCatalogId = shoeCatalogId,
                Size = newShoe.Size,
                ComfortRating = newShoe.ComfortRating,
                StyleRating = newShoe.StyleRating,
                Season = newShoe.Season,
                Review = newShoe.Review,
                DateAdded = DateTime.UtcNow,
            };


            await _context.AddAsync(shoe);
            var result = await _context.SaveChangesAsync();

            response.Success = result > 0;
            if (response.Success)
            {
                response.Message = "Shoe successfully added to user's collection.";
                response.Data = true;
                return response;
            }
            response.Message = "Error adding shoe to user's collection.";
            response.Data = false;
            return response;

        }
    }

}
