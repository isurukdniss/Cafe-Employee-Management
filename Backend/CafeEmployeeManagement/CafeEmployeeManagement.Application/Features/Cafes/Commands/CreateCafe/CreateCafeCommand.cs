using CafeEmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CafeEmployeeManagement.Application.Features.Cafes.Commands.CreateCafe
{
    public class CreateCafeCommand : IRequest<ApiResponse<Guid>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Logo { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string Location { get; set; }
    }
}
