using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Enums;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<ApiResponse<string>>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        public Gender Gender { get; set; }

        public Guid CafeId { get; set; }
    }
}
