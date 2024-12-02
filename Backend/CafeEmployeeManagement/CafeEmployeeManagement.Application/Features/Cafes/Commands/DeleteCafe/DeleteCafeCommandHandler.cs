using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.DeleteCafe
{
    public class DeleteCafeCommandHandler : IRequestHandler<DeleteCafeCommand, ApiResponse<bool>>
    {
        private readonly ICafeRepository cafeRepository;

        public DeleteCafeCommandHandler(ICafeRepository cafeRepository)
        {
            this.cafeRepository = cafeRepository;
        }
        public async Task<ApiResponse<bool>> Handle(DeleteCafeCommand request, CancellationToken cancellationToken)
        {
            var cafe = await cafeRepository.GetByIdAsync(request.CafeId);

            if (cafe == null)
            {
                return ApiResponse<bool>.SetFailure(["Cafe not found"]);
            }

            await cafeRepository.DeleteAsync(cafe.Id);

            return ApiResponse<bool>.SetSuccess(true);
        }
    }
}
