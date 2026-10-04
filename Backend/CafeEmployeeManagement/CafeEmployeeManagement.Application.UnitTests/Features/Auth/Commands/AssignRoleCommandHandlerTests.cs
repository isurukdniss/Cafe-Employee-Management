using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Features.Auth.Commands.AssignRole;
using CafeEmployeeManagement.Application.Interfaces;
using CafeEmployeeManagement.Domain.Constants;
using Moq;

namespace CafeEmployeeManagement.Application.UnitTests.Features.Auth.Commands
{
    public class AssignRoleCommandHandlerTests
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly AssignRoleCommandHandler _handler;

        public AssignRoleCommandHandlerTests()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _handler = new AssignRoleCommandHandler(_identityServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingUser_ShouldAssignRole()
        {
            // Arrange
            var command = new AssignRoleCommand { Email = "jane@abc.com", Role = Roles.Admin };

            _identityServiceMock.Setup(s => s.AddToRoleAsync(command.Email, command.Role))
                .ReturnsAsync(IdentityResultModel.Success());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(Roles.Admin, result.Data);
        }

        [Fact]
        public async Task Handle_UnknownUser_ShouldReturnFailure()
        {
            // Arrange
            var command = new AssignRoleCommand { Email = "nobody@abc.com", Role = Roles.Admin };

            _identityServiceMock.Setup(s => s.AddToRoleAsync(command.Email, command.Role))
                .ReturnsAsync(IdentityResultModel.Failure(["User 'nobody@abc.com' was not found."]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.Errors);
        }
    }
}
