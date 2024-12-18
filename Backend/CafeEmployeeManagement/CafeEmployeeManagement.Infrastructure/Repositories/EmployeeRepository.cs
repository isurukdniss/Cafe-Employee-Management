﻿using CafeEmployeeManagement.Application.Interfaces;
using CafeEmployeeManagement.Domain.Entities;
using CafeEmployeeManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CafeEmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee, string>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
