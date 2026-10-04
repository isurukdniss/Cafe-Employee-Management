using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Features.Auth;
using CafeEmployeeManagement.Application.Features.Auth.Commands.Register;
using CafeEmployeeManagement.Application.Interfaces;
using CafeEmployeeManagement.Domain.Constants;
using Moq;

namespace CafeEmployeeManagement.Application.UnitTests.Features.Auth.Commands
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _handler = new RegisterCommandHandler(_identityServiceMock.Object, _jwtTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_NewUser_ShouldCreateUserWithUserRoleAndReturnToken()
        {
            // Arrange
            var command = new RegisterCommand { Email = "jane@abc.com", Password = "Passw0rd!" };
            var user = new AuthUser { Id = "1", Email = command.Email, Roles = [Roles.User] };
            var token = new AuthResponseDto { Token = "token", Email = command.Email, Roles = user.Roles };

            _identityServiceMock.Setup(s => s.CreateUserAsync(command.Email, command.Password, Roles.User))
                .ReturnsAsync(IdentityResultModel.Success());
            _identityServiceMock.Setup(s => s.ValidateCredentialsAsync(command.Email, command.Password))
                .ReturnsAsync(user);
            _jwtTokenGeneratorMock.Setup(g => g.GenerateToken(user)).Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Same(token, result.Data);
            _identityServiceMock.Verify(s => s.CreateUserAsync(command.Email, command.Password, Roles.User), Times.Once);
        }

        [Fact]
        public async Task Handle_CreateUserFails_ShouldReturnFailureWithErrors()
        {
            // Arrange
            var command = new RegisterCommand { Email = "jane@abc.com", Password = "Passw0rd!" };

            _identityServiceMock.Setup(s => s.CreateUserAsync(command.Email, command.Password, Roles.User))
                .ReturnsAsync(IdentityResultModel.Failure(["Email 'jane@abc.com' is already taken."]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Email 'jane@abc.com' is already taken.", result.Errors);
            _jwtTokenGeneratorMock.Verify(g => g.GenerateToken(It.IsAny<AuthUser>()), Times.Never);
        }
    }
}
