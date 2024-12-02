using CafeEmployeeManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CafeEmployeeManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Employee()
        {
            Id = GenerateUniqueId();
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public Gender Gender { get; set; }


        public Guid CafeId { get; set; }
        public Cafe Cafe { get; set; }


        private string GenerateUniqueId()
        {
            const string prefix = "UI";
            const string alphaNumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; 
            const int length = 7;

            Random random = new();

            string randomString = new string(Enumerable.Range(0, length)
                .Select(_ => alphaNumericChars[random.Next(alphaNumericChars.Length)]).ToArray());

            return string.Concat(prefix, randomString);
        }
    }
}
