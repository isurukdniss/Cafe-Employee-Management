using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;
using System.Globalization;
using System.Linq.Expressions;

namespace CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes
{
    public class GetCafesQueryHandler : IRequestHandler<GetCafesQuery, ApiResponse<IEnumerable<CafeResponseDto>>>
    {
        private readonly IRepository<Cafe, Guid> _repository;
        private readonly IMapper mapper;

        public GetCafesQueryHandler(IRepository<Cafe, Guid> repository, IMapper mapper)
        {
            this._repository = repository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CafeResponseDto>>> Handle(GetCafesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Cafe, bool>> filter = null;

            if (!string.IsNullOrEmpty(request.Location))
            {
                filter = x => x.Location.ToLower().Contains(request.Location.ToLower());
            }

            var cafeList = await _repository.GetAllAsync(filter, c => c.Employees);

            var result = mapper.Map<IEnumerable<CafeResponseDto>>(cafeList)
                .OrderByDescending(x => x.Employees);

            return ApiResponse<IEnumerable<CafeResponseDto>>.SetSuccess(result);

        }
    }
}
