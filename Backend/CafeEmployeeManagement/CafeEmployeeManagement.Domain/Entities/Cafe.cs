using System.ComponentModel.DataAnnotations;

namespace CafeEmployeeManagement.Domain.Entities
{
    public class Cafe : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        public string Name { get; set; }

        public string Description { get; set; }
        
        public string? Logo { get; set; }

        public string Location { get; set; }

        // Navigation property
        public ICollection<Employee> Employees { get; set; }
    }
}
