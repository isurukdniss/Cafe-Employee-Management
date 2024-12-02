using CafeEmployeeManagement.Application.Common.Models;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesQuery : IRequest<ApiResponse<IEnumerable<EmployeeDto>>>
    {
        public Guid? CafeId { get; set; }
        public string? Cafe { get; set; }
    }
}
