using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ApiResponse<bool>>
    {
        private readonly IEmployeeRepository employeeRepository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
            {
                return ApiResponse<bool>.SetFailure(["Employee not found"]);
            }

            await employeeRepository.DeleteAsync(employee.Id);

            return ApiResponse<bool>.SetSuccess(true);
        }
    }
}
