using CafeEmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.UpdateCafe
{
    public class UpdateCafeCommand : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Logo { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string Location { get; set; }
    }
}
