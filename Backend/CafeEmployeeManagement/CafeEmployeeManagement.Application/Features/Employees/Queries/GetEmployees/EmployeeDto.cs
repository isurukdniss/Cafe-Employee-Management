using CafeEmployeeManagement.Domain.Enums;

namespace CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string CafeName { get; set; }
        public int DaysWorked { get; set; }
        public Guid CafeId { get; set; }

    }
}
