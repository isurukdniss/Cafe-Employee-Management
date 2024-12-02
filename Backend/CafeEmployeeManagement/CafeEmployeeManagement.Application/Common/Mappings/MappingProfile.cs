using AutoMapper;
using CafeEmployeeManagement.Application.Features.Cafes.Commands.CreateCafe;
using CafeEmployeeManagement.Application.Features.Cafes.Commands.UpdateCafe;
using CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes;
using CafeEmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using CafeEmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees;
using CafeEmployeeManagement.Domain.Entities;

namespace CafeEmployeeManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(x => x.DaysWorked, options =>
                {
                    options.MapFrom(e => (DateTime.UtcNow - e.CreatedDate).Days);
                }
            );
            CreateMap<CreateEmployeeCommand, Employee>();
            CreateMap<UpdateEmployeeCommand, Employee>();

            CreateMap<Cafe, CafeResponseDto>()
                .ForMember(x => x.Employees, options =>
                {
                    options.MapFrom(c => c.Employees.Count);
                });

            CreateMap<CreateCafeCommand, Cafe>();
            CreateMap<UpdateCafeCommand, Cafe>();
        }
    }
}
