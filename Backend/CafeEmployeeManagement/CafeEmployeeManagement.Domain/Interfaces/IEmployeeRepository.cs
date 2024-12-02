using CafeEmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEmployeeManagement.Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee, string>
    {
    }
}
