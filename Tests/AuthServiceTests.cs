using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagement.Controllers;
using TaskManagement.Models;
using TaskManagement.DTOs;
using TaskManagement.Data;
using System.Linq.Expressions;
using BCrypt.Net;

namespace TaskManagement.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            _mockConfiguration = new Mock<IConfiguration>();

            _authController = new AuthController(_mockContext.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsSuccessfullyRegistered()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Name = "Alvaro Tester",
                Email = "test@test.com",
                Password = "test1234"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _authController.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully.", okResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "wrong.email@example.com",
                Password = "WrongPassword"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _authController.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenValidCredentials()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@test.com",
                Password = "test1234"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(m => m.Users).Returns(mockDbSet.Object);
            _mockContext.Setup(m => m.Users.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), CancellationToken.None))
                        .ReturnsAsync(new User
                        {
                            Id = 1,
                            Email = "test@test.com",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("test1234")
                        });

            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("SuperSecureJWTKey123456789012345");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("TaskManagement");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("TaskManagementUsers");

            // Act
            var result = await _authController.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(token);
        }
    }
}