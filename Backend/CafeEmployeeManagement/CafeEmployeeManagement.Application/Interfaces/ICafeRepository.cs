using CafeEmployeeManagement.Domain.Entities;

namespace CafeEmployeeManagement.Application.Interfaces
{
    public interface ICafeRepository : IRepository<Cafe, Guid>
    {
    }
}
