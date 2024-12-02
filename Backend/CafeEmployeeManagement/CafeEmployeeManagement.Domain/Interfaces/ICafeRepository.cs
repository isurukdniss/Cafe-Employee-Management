using CafeEmployeeManagement.Domain.Entities;

namespace CafeEmployeeManagement.Domain.Interfaces
{
    public interface ICafeRepository : IRepository<Cafe, Guid>
    {
    }
}
