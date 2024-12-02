using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeEmployeeManagement.Application.Common.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public IList<string> Errors { get; set; }

        private const string successMessage = "Request successful";
        private const string failureMessage = "Request failed";

        public ApiResponse()
        {
            Errors = new List<string>();
        }

        public static ApiResponse<T> SetSuccess(T data, string message = successMessage)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
            };
        }

        public static ApiResponse<T> SetFailure(List<string> errors, string message = failureMessage)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
            };
        }
    }
}
