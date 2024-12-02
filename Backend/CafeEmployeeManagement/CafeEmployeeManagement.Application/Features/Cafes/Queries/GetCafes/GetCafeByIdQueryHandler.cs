using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes
{
    public class GetCafeByIdQueryHandler : IRequestHandler<GetCafeByIdQuery, ApiResponse<CafeResponseDto>>
    {
        private readonly IRepository<Cafe, Guid> _repository;
        private readonly IMapper _mapper;

        public GetCafeByIdQueryHandler(IRepository<Cafe,Guid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CafeResponseDto>> Handle(GetCafeByIdQuery request, CancellationToken cancellationToken)
        {

            var cafe = await _repository.GetByIdAsync(request.CafeId, c => c.Employees);

            if (cafe == null)
            {
                return ApiResponse<CafeResponseDto>.SetFailure(["Cafe not found"]);
            }

            var employeeDto = _mapper.Map<CafeResponseDto>(cafe);

            return ApiResponse<CafeResponseDto>.SetSuccess(employeeDto);
        }
    }
}
