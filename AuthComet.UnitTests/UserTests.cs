using AuthComet.Domain.Dtos;
using AuthComet.Domain.Validations;
using AuthComet.Domain.Validations.Messages;
using FluentAssertions;

namespace AuthComet.UnitTests
{
    public class UserTests
    {
        private readonly UserDomain _userDomain;

        public UserTests()
        {
            _userDomain = new UserDomain();
        }

        [Fact]
        public void CreateUser_WithValidData_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var userDto = new UserDto("validUser", "ValidPassword123!", "valid@example.com");

            // Act
            var result = _userDomain.CreateUser(userDto);

            // Assert
            result.Ok.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Username.Should().Be("validUser");
            result.Data.Password.Should().NotBe("ValidPassword123!");
            result.Data.Email.Should().Be("valid@example.com");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateUser_WithInvalidUsername_ShouldFail(string? invalidUsername)
        {
            // Arrange
            var userDto = new UserDto(invalidUsername!, "ValidPassword123!", "valid@example.com");

            // Act
            var result = _userDomain.CreateUser(userDto);

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be(UserMessages.UsernameRequired);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateUser_WithInvalidPassword_ShouldFail(string? invalidPassword)
        {
            // Arrange
            var userDto = new UserDto("validUser", invalidPassword!, "valid@example.com");

            // Act
            var result = _userDomain.CreateUser(userDto);

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be(UserMessages.PasswordRequired);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalidEmail")]
        [InlineData("invalid@Email")]
        public void CreateUser_WithInvalidEmail_ShouldFail(string? invalidEmail)
        {
            // Arrange
            var userDto = new UserDto("validUser", "ValidPassword123!", invalidEmail!);

            // Act
            var result = _userDomain.CreateUser(userDto);

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be(UserMessages.InvalidEmail);
        }

        [Fact]
        public void CreateUser_WithAllValidFields_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var userDto = new UserDto("validUser", "ValidPassword123!", "valid@example.com");

            // Act
            var result = _userDomain.CreateUser(userDto);

            // Assert
            result.Ok.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Username.Should().Be("validUser");
            result.Data.Password.Should().NotBe("ValidPassword123!");
            result.Data.Email.Should().Be("valid@example.com");
            result.Data.Id.Should().Be(0);
            result.Data.CreationDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void HashPassword_ShouldGenerateHashedPassword()
        {
            // Arrange
            var plainPassword = "TestPassword123";

            // Act
            var hashedPassword = _userDomain.HashPassword(plainPassword);

            // Assert
            hashedPassword.Should().NotBeNullOrWhiteSpace();
            hashedPassword.Should().NotBe(plainPassword);
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var plainPassword = "TestPassword123";
            var hashedPassword = _userDomain.HashPassword(plainPassword);

            // Act
            var result = _userDomain.VerifyPassword(plainPassword, hashedPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var plainPassword = "TestPassword123";
            var wrongPassword = "WrongPassword";
            var hashedPassword = _userDomain.HashPassword(plainPassword);

            // Act
            var result = _userDomain.VerifyPassword(wrongPassword, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }
    }
}