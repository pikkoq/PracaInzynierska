using AutoMapper;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.AdminDtos;
using ShoeBoardAPI.Models.DTO.FrindsDtos;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Models.DTO.UserDtos;

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
            CreateMap<UserShoeCatalog, GetShoeSearchDto>();
            CreateMap<ShoeCatalog, GetShoeSearchDto>();
            CreateMap<EditUserShoeDto, Shoe >()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null &&
                !srcMember.Equals(default) &&
                (srcMember is not string || !string.IsNullOrWhiteSpace(srcMember.ToString()))));

            //Admin panel maps
            CreateMap<UserShoeCatalog, GetShoesToAcceptDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<UserShoeCatalog, ShoeCatalog>()
                .ForMember(dest => dest.Url_Link_Handler, opt => opt.MapFrom(src => src.ShopUrl))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Image_Url, opt => opt.MapFrom(src => src.Image_Path))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<UserShoeCatalog, EditNewAddedShoesDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, GetAllUsersDto>().ReverseMap();
            CreateMap<EditUserAccountDto,User >()
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => src.ProfilePicture));
            CreateMap<Post, GetAllUsersPostsDto>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicturePath))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src => src.DatePosted))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Shoe.Size))
                .ForMember(dest => dest.ComfortRating, opt => opt.MapFrom(src => src.Shoe.ComfortRating))
                .ForMember(dest => dest.StyleRating, opt => opt.MapFrom(src => src.Shoe.StyleRating))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Shoe.Season))
                .ForMember(dest => dest.Review, opt => opt.MapFrom(src => src.Shoe.Review))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Shoe.ShoeCatalog.Title))
                .ForMember(dest => dest.ShoePhoto, opt => opt.MapFrom(src => src.Shoe.ShoeCatalog.Image_Url))
                .ForMember(dest => dest.likeCount, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.commentsCount, opt => opt.MapFrom(src => src.Comments.Count));
        }
    }
}
