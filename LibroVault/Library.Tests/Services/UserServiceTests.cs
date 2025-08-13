using Library.API.Interfaces;
using Library.API.Models;
using Library.API.Services;
using Moq;
using Xunit;
using FluentAssertions;

namespace Library.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _service = new UserService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, FullName = "Kavya", Email = "kavya@example.com" },
                new User { UserId = 2, FullName = "Karthik", Email = "Karthik@example.com" }
            };
            _repoMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First().FullName.Should().Be("Kavya");
        }

        [Fact]
        public async Task AddUserAsync_ShouldCallRepoAndSave()
        {
            // Arrange
            var user = new User { UserId = 1, FullName = "Kavya", Email = "kavya@example.com" };

            // Act
            await _service.AddUserAsync(user);

            // Assert
            _repoMock.Verify(r => r.AddUserAsync(user), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var email = "kavya@example.com";
            var user = new User { UserId = 1, Email = email };
            _repoMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByEmailAsync(email);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(email);
        }

        [Fact]
        public async Task ValidateUserAsync_ShouldReturnUserIfCredentialsMatch()
        {
            // Arrange
            var email = "kavya@example.com";
            var password = "12345";
            var user = new User { UserId = 1, Email = email, Password = password };
            _repoMock.Setup(r => r.ValidateUserAsync(email, password)).ReturnsAsync(user);

            // Act
            var result = await _service.ValidateUserAsync(email, password);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(email);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldCallRepoDeleteAndSave()
        {
            // Arrange
            var user = new User { UserId = 1 };

            // Act
            await _service.DeleteUserAsync(user);

            // Assert
            _repoMock.Verify(r => r.DeleteUserAsync(user), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
