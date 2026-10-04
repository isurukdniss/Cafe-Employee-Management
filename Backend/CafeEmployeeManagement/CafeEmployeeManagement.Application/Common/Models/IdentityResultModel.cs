namespace CafeEmployeeManagement.Application.Common.Models
{
    public class IdentityResultModel
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static IdentityResultModel Success() => new IdentityResultModel { Succeeded = true };

        public static IdentityResultModel Failure(IEnumerable<string> errors) =>
            new IdentityResultModel { Succeeded = false, Errors = errors.ToList() };
    }
}
