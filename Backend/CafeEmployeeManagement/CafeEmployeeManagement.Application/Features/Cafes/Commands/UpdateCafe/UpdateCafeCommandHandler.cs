using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.UpdateCafe
{
    public class UpdateCafeCommandHandler : IRequestHandler<UpdateCafeCommand, ApiResponse<bool>>
    {
        private readonly ICafeRepository cafeRepository;
        private readonly IMapper mapper;

        public UpdateCafeCommandHandler(ICafeRepository cafeRepository, IMapper mapper)
        {
            this.cafeRepository = cafeRepository;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateCafeCommand request, CancellationToken cancellationToken)
        {
            var cafe = await cafeRepository.GetByIdAsync(request.Id);
            if (cafe == null)
            {
                return ApiResponse<bool>.SetFailure(["Cafe not found"]);
            }

            string? logoFilePath = null;

            if (request.LogoFile != null)
            {
                var logoFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.LogoFile.FileName)}";
                logoFilePath = "Uploads/Logos" + "/" + logoFileName;

                using (var stream = new FileStream(logoFilePath, FileMode.Create))
                {
                    await request.LogoFile.CopyToAsync(stream);
                }
            }

            cafe = mapper.Map<Cafe>(request);
            cafe.Logo = logoFilePath;

            await cafeRepository.UpdateAsync(cafe);

            return ApiResponse<bool>.SetSuccess(true);
        }
    }
}
