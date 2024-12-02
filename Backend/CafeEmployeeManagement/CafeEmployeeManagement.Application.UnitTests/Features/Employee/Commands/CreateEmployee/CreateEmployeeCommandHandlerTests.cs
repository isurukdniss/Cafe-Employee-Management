using AutoMapper;
using CafeEmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using CafeEmployeeManagement.Domain.Interfaces;
using Moq;

namespace CafeEmployeeManagement.Application.UnitTests.Features.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateEmployeeCommandHandler _handler;

        public CreateEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateEmployeeCommandHandler(_employeeRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldAddEmployeeAndReturnSuccessResponse()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            {
                Name = "John Doe",
                Email = "john@abc.com",
                PhoneNumber = 98888888,
                Gender = Domain.Enums.Gender.Male,
                CafeId = Guid.Parse("3e58d4dd-3d2a-422f-a4c0-e13758fdc7ef"),
            };

            var employee = new Domain.Entities.Employee
            {
                Id = "UIBrfrEfg",
                Email = "john@abc.com",
                PhoneNumber = 98888888,
                Gender = Domain.Enums.Gender.Male,
                CafeId = Guid.Parse("3e58d4dd-3d2a-422f-a4c0-e13758fdc7ef"),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Employee>(command)).Returns(employee);
            _employeeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(employee.Id, result.Data);

            _mapperMock.Verify(m => m.Map<Domain.Entities.Employee>(command), Times.Once);
            //_employeeRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Domain.Entities.Employee>(e => 
            //    e.Name == "John Doe" && e.Email == "john@abc.com")), Times.Once);
        }

    }
}
