using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using peripatoiCrud.API.Controllers;
using peripatoiCrud.API.Models.DTOs;
using peripatoiCrud.API.Repositories;

namespace peripatoiCrud.API.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<ITokenRepositroy> _mockTokenRepository;
        private AuthController _authController;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            _mockTokenRepository = new Mock<ITokenRepositroy>();
            _authController = new AuthController(_mockUserManager.Object, _mockTokenRepository.Object);
        }

        [Test]
        public async Task EggrafhMeSwstaDataEpistrefei200()
        {
            // Arrange
            var eggrafhRequestDto = new EggrafhRequestDto
            {
                Username = "testuser@example.com",
                Password = "Password123!",
                Roloi = new[] { "read" }
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.AddToRolesAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authController.Register(eggrafhRequestDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EggrafhMeLathosDataEpistrefei400()
        {
            // Arrange
            var eggrafhRequestDto = new EggrafhRequestDto
            {
                Username = "testuser@example.com",
                Password = "Password123!"
            };

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _authController.Register(eggrafhRequestDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task SyndeshMeSwstaDataEpistrefei200MeToken()
        {
            // Arrange
            var syndeshRequestDto = new SyndeshRequestDto
            {
                Username = "testuser@example.com",
                Password = "Password123!"
            };

            var identityUser = new IdentityUser
            {
                UserName = syndeshRequestDto.Username,
                Email = syndeshRequestDto.Username
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(syndeshRequestDto.Username))
                .ReturnsAsync(identityUser);

            _mockUserManager.Setup(x => x.CheckPasswordAsync(identityUser, syndeshRequestDto.Password))
                .ReturnsAsync(true);

            _mockUserManager.Setup(x => x.GetRolesAsync(identityUser))
                .ReturnsAsync(new[] { "read" });

            _mockTokenRepository.Setup(x => x.DhmiourgiaJWTToken(identityUser, It.IsAny<List<string>>()))
                .Returns("mockToken");

            // Act
            var result = await _authController.Login(syndeshRequestDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as SyndeshResponseDto;
            Assert.IsNotNull(response);
            Assert.AreEqual("mockToken", response.JwtToken);
        }

        [Test]
        public async Task SyndeshMeSwstaDataEpistrefei400()
        {
            // Arrange
            var syndeshRequestDto = new SyndeshRequestDto
            {
                Username = "testuser@example.com",
                Password = "WrongPassword"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(syndeshRequestDto.Username))
                .ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _authController.Login(syndeshRequestDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
