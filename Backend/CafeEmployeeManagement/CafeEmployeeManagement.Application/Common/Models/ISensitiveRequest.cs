namespace CafeEmployeeManagement.Application.Common.Models
{
    /// <summary>
    /// Marks a request whose payload and response (passwords, tokens) must not be logged.
    /// </summary>
    public interface ISensitiveRequest
    {
    }
}
