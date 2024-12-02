using CafeEmployeeManagement.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes
{
    public class GetCafesQuery : IRequest<ApiResponse<IEnumerable<CafeResponseDto>>>
    {
        public string? Location { get; set; }
    }
}
