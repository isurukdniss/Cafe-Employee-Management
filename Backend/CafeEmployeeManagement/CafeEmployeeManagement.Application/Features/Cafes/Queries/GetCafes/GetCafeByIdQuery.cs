using CafeEmployeeManagement.Application.Common.Models;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes
{
    public class GetCafeByIdQuery : IRequest<ApiResponse<CafeResponseDto>>
    {
        public Guid CafeId { get; set; }
    }

}
