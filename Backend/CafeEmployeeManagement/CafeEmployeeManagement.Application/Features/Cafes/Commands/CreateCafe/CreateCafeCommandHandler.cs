using AutoMapper;
using CafeEmployeeManagement.Application.Common.Models;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using MediatR;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.CreateCafe
{
    public class CreateCafeCommandHandler : IRequestHandler<CreateCafeCommand, ApiResponse<Guid>>
    {
        private readonly ICafeRepository cafeRepository;
        private readonly IMapper mapper;

        public CreateCafeCommandHandler(ICafeRepository cafeRepository, IMapper mapper)
        {
            this.cafeRepository = cafeRepository;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateCafeCommand request, CancellationToken cancellationToken)
        {
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

            var cafe = mapper.Map<Cafe>(request);

            if (logoFilePath != null)
            {
                cafe.Logo = logoFilePath;
            }

            await cafeRepository.AddAsync(cafe);

            return ApiResponse<Guid>.SetSuccess(cafe.Id);
        }

    }
}
