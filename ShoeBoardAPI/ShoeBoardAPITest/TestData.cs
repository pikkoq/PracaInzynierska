using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.AdminDtos;
using ShoeBoardAPI.Models.DTO.PostDtos;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Models.DTO.UserDtos;

namespace ShoeBoardAPITest
{
    public static class TestData
    {
        public static User GetTestUser1() => new User
        {
            Id = "user-1",
            UserName = "TestUser1",
            Email = "test1@example.com",
            Bio = "Test bio 1",
            ProfilePicturePath = "https://example.com/avatar1.png",
            DateCreated = DateTime.UtcNow
        };

        public static CreatePostDto GetTestCreatePostDto() => new CreatePostDto
        {
            ShoeId = 1,
            Content = "New post content"
        };

        public static CreateCommentDto GetTestCreateCommentDto() => new CreateCommentDto
        {
            PostId = 1,
            Content = "New comment"
        };

        public static EditUserShoeDto GetTestEditUserShoeDto() => new EditUserShoeDto
        {
            Size = "43",
            ComfortRating = 4,
            StyleRating = 5,
            Season = "Winter",
            Review = "Updated review"
        };

        public static SignShoeToUserDto GetTestSignShoeToUserDto() => new SignShoeToUserDto
        {
            Size = "42",
            ComfortRating = 5,
            StyleRating = 4,
            Season = "Summer",
            Review = "Great fit!"
        };

        public static EditNewAddedShoesDto GetTestEditNewAddedShoesDto() => new EditNewAddedShoesDto
        {
            Title = "Updated Title",
            Brand = "Updated Brand",
            Model_No = "UPD-001",
            Nickname = "Updated Nickname",
            Series = "Updated Series",
            Gender = "Unisex",
            MainColor = "Blue",
            Colorway = "Blue/White",
            Price = 199.99m
        };

        public static EditUserAccountDto GetTestEditUserAccountDto() => new EditUserAccountDto
        {
            Bio = "Updated bio",
            ProfilePicture = "https://example.com/new-avatar.png"
        };

        public static LoginUserDto GetTestLoginUserDto() => new LoginUserDto
        {
            Login = "TestUser1",
            Password = "TestPassword123!"
        };

        public static RegisterUserDto GetTestRegisterUserDto() => new RegisterUserDto
        {
            Username = "NewUser",
            Email = "newuser@example.com",
            Password = "NewPassword123!"
        };

        public static ChangePasswordDto GetTestChangePasswordDto() => new ChangePasswordDto
        {
            CurrentPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        public static EditUserDto GetTestEditUserDto() => new EditUserDto
        {
            Username = "EditedUser",
            Email = "edited@example.com",
            Bio = "Edited bio",
            ProfilePicturePath = "https://example.com/new-avatar.png"
        };
    }
}
