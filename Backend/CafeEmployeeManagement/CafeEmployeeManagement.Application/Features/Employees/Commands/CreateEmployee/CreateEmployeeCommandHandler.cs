using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, ApiResponse<string>>
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = mapper.Map<Employee>(request);
            employee.CreatedDate = DateTime.UtcNow;
            employee.UpdatedDate = DateTime.UtcNow;

            await employeeRepository.AddAsync(employee);

            return ApiResponse<string>.SetSuccess(employee.Id);
        }
    }
}
