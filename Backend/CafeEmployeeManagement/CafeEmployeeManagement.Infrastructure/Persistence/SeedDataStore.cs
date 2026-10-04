using CafeEmployeeManagement.Domain.Constants;
using CafeEmployeeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEmployeeManagement.Infrastructure.Persistence
{
    public static class SeedDataStore
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cafe>().HasData(
                    new Cafe
                    {
                        Id = new Guid("cb93d19a-4019-4770-b4f5-5787bd3e7da7"),
                        Name = "Brew Heaven",
                        Location = "123 Orchid Avenue, #05-67, Singapore 567890",
                        Description = "Coffe heaven",
                        Logo = "Uploads/Logos/5d5ec5cd-4713-4733-8c4e-5eb94cf2684d.jpg",
                        CreatedDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        UpdatedDate = new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    new Cafe
                    {
                        Id = new Guid("3e58d4dd-3d2a-422f-a4c0-e13758fdc7ef"),
                        Name = "Mocha Muse",
                        Location = "45 Merlion Lane, #12-34, Singapore 098765",
                        Description = "Amazing coffe cafe",
                        Logo = "Uploads/Logos/4362ab34-42ad-4cbb-be84-217dcf69d4d5.jpg",
                        CreatedDate = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        UpdatedDate = new DateTime(2010, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    }
                );

            modelBuilder.Entity<Employee>().HasData(
                    new Employee
                    {
                        Id = "UIAbcDEfg",
                        CafeId = new Guid("cb93d19a-4019-4770-b4f5-5787bd3e7da7"),
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        PhoneNumber = 98881111,
                        Gender = Domain.Enums.Gender.Male,
                        CreatedDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        UpdatedDate = new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    new Employee
                    {
                        Id = "UIArfDEfg",
                        CafeId = new Guid("cb93d19a-4019-4770-b4f5-5787bd3e7da7"),
                        Name = "Jane Doe",
                        Email = "jane.doe@example.com",
                        PhoneNumber = 98881221,
                        Gender = Domain.Enums.Gender.Female,
                        CreatedDate = new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        UpdatedDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    },
                    new Employee
                    {
                        Id = "UIBrfrEfg",
                        CafeId = new Guid("3e58d4dd-3d2a-422f-a4c0-e13758fdc7ef"),
                        Name = "Steve Smith",
                        Email = "steve.smith@example.com",
                        PhoneNumber = 88855221,
                        Gender = Domain.Enums.Gender.Male,
                        CreatedDate = new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                        UpdatedDate = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                    }
                );

            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "6f0f3a7e-6a43-4c55-9e27-3c1f0d2a9b10",
                        Name = Roles.Admin,
                        NormalizedName = Roles.Admin.ToUpperInvariant(),
                        ConcurrencyStamp = "b3b7f1c2-1d6e-4f0a-8a35-0c9e5d7b2a41",
                    },
                    new IdentityRole
                    {
                        Id = "2c8d5b91-47e3-4a8f-b6d2-91e4f7a3c5d8",
                        Name = Roles.User,
                        NormalizedName = Roles.User.ToUpperInvariant(),
                        ConcurrencyStamp = "e7a2c4d9-5b1f-4e86-9c3a-6d8b0f2e1a57",
                    }
                );
        }
    }
}
