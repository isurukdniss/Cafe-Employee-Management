using CafeEmployeeManagement.Application.Common.Models;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeeByIdQuery : IRequest<ApiResponse<EmployeeDto>>
    {
        public string EmployeeId { get; set; }
    }

}
