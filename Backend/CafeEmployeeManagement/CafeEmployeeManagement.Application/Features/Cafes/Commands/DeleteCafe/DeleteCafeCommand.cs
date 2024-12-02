using CafeEmployeeManagement.Application.Common.Models;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.DeleteCafe
{
    public class DeleteCafeCommand : IRequest<ApiResponse<bool>>
    {
        public Guid CafeId { get; set; }
    }
}
