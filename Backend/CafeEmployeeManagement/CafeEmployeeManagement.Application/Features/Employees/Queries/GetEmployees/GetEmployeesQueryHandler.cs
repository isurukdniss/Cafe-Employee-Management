using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;
using System.Linq.Expressions;

namespace CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, ApiResponse<IEnumerable<EmployeeDto>>>
    {
        private readonly IRepository<Employee, string> _repository;
        private readonly IMapper _mapper;

        public GetEmployeesQueryHandler(IRepository<Employee, string> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Employee, bool>> filter = null;

            if (request.Cafe != null)
            {
                filter = x => x.Cafe.Name.ToLower().Contains(request.Cafe.ToLower());
            }

            var employees = await _repository.GetAllAsync(filter, r => r.Cafe);

            if (employees == null)
            {
                return ApiResponse<IEnumerable<EmployeeDto>>.SetFailure(["Employee not found"]);
            }

            var employeeList = _mapper.Map<IEnumerable<EmployeeDto>>(employees).OrderByDescending(e => e.DaysWorked);

            return ApiResponse<IEnumerable<EmployeeDto>>.SetSuccess(employeeList);

        }
    }
}
