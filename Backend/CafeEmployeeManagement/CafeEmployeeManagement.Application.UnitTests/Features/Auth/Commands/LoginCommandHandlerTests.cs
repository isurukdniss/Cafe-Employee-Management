using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Features.Auth;
using CafeEmployeeManagement.Application.Features.Auth.Commands.Login;
using CafeEmployeeManagement.Application.Interfaces;
using CafeEmployeeManagement.Domain.Constants;
using Moq;

namespace CafeEmployeeManagement.Application.UnitTests.Features.Auth.Commands
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _handler = new LoginCommandHandler(_identityServiceMock.Object, _jwtTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var command = new LoginCommand { Email = "admin@abc.com", Password = "Passw0rd!" };
            var user = new AuthUser { Id = "1", Email = command.Email, Roles = [Roles.Admin] };
            var token = new AuthResponseDto { Token = "token", Email = command.Email, Roles = user.Roles };

            _identityServiceMock.Setup(s => s.ValidateCredentialsAsync(command.Email, command.Password))
                .ReturnsAsync(user);
            _jwtTokenGeneratorMock.Setup(g => g.GenerateToken(user)).Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Same(token, result.Data);
        }

        [Fact]
        public async Task Handle_InvalidCredentials_ShouldReturnFailureWithoutToken()
        {
            // Arrange
            var command = new LoginCommand { Email = "admin@abc.com", Password = "wrong" };

            _identityServiceMock.Setup(s => s.ValidateCredentialsAsync(command.Email, command.Password))
                .ReturnsAsync((AuthUser?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            _jwtTokenGeneratorMock.Verify(g => g.GenerateToken(It.IsAny<AuthUser>()), Times.Never);
        }
    }
}
