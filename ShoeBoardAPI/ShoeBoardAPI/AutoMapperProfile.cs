using AutoMapper;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Models.DTO.UserDtos;
using ShoeBoardAPI.Models.Enums;

namespace ShoeBoardAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => src.ProfilePicturePath))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated));

            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<EditUserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => src.ProfilePicturePath));

            CreateMap<ShoeCatalog, GetShoeDetailsDto>();
            CreateMap<UserShoeCatalog, GetAllAddedUserShoesDto>();
            CreateMap<NewShoeRegistryDto, UserShoeCatalog>();
            CreateMap<Shoe, GetShoeDetailsDto>();
            CreateMap<UserShoeCatalog, GetShoeDetailsDto>();
            CreateMap<Shoe, GetAllUserShoesDto>();

        }
    }
}
