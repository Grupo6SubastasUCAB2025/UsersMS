using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Commons.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public int ErrorCode { get; }
        public List<string> Errors { get; }

        public UnauthorizedException(string message) : base(message)
        {
            Errors = new List<string>();
        }

        public UnauthorizedException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
            Errors = new List<string>();
        }

        public UnauthorizedException(string message, List<string> errors) : base(message)
        {
            Errors = errors;
        }

        public UnauthorizedException(string message, int errorCode, List<string> errors) : base(message)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
            Errors = new List<string>();
        }

        public UnauthorizedException(string message, int errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            Errors = new List<string>();
        }

        public UnauthorizedException(string message, int errorCode, List<string> errors, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }

        public override string ToString()
        {
            var errorsString = string.Join(", ", Errors);
            return $"Error Code: {ErrorCode}, Message: {Message}, Errors: {errorsString}, Inner Exception: {InnerException}";
        }
    }
}
