using Microsoft.AspNetCore.Mvc;
using Moq;
using revisifyBackened;
using revisifyBackened.Controllers;
using revisifyBackened.Interface;
using revisifyBackened.Models.Dto;
namespace TestProject1
{
    public class UnitTest1
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _authController;

        public UnitTest1()
        {
            _authServiceMock = new Mock<IAuthService>();
            _authController = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new LoginRequestDto
            {
                UserName = "test@example.com",
                Password = "ValidPassword123"
            };

            var expectedResponse = new ApiResponse<UserProfile>
            {
                Data = new UserProfile { Id = "123", UserName = "test@example.com" },
                IsSuccess = true,
                Message = "Logged In Successfully",
                StatusCode = 200
            };

            _authServiceMock
                .Setup(service => service.Login(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _authController.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<UserProfile>>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(200, response.StatusCode);
        }
    }
}