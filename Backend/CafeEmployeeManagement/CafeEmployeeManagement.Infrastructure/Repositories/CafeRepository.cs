using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Domain.Interfaces;
using CafeEmployeeManagement.Infrastructure.Persistence;

namespace CafeEmployeeManagement.Infrastructure.Repositories
{
    public class CafeRepository : Repository<Cafe, Guid>, ICafeRepository
    {
        public CafeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
