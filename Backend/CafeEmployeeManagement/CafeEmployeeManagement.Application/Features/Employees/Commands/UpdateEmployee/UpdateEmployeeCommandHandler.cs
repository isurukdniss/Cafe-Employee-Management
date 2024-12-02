using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Application.Interfaces;
using CafeEmployeeManagement.Domain.Entities;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ApiResponse<bool>>
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly ICafeRepository cafeRepository;
        private readonly IMapper mapper;

        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository
            , ICafeRepository cafeRepository
            , IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.cafeRepository = cafeRepository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id);
            if (employee == null)
            {
                return ApiResponse<bool>.SetFailure(["Employee not found"]);
            }

            if (request.CafeId.HasValue)
            {
                var cafe = await cafeRepository.GetByIdAsync(request.CafeId.Value);
                if (cafe == null)
                {
                    return ApiResponse<bool>.SetFailure(["The given Cafe Id not found"]);
                }
            }

            employee = mapper.Map<Employee>(request);
            employee.UpdatedDate = DateTime.UtcNow;

            await employeeRepository.UpdateAsync(employee);

            return ApiResponse<bool>.SetSuccess(true);

        }
    }
}
