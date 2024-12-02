using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, ApiResponse<EmployeeDto>>
    {
        private readonly IRepository<Employee,string> _repository;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IRepository<Employee,string> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {

            var employee = await _repository.GetByIdAsync(request.EmployeeId, e => e.Cafe);

            if (employee == null)
            {
                return ApiResponse<EmployeeDto>.SetFailure(["Employee not found"]);
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return ApiResponse<EmployeeDto>.SetSuccess(employeeDto);
        }
    }
}
