using AutoMapper;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;

namespace ShoeBoardAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
        }
    }
}
