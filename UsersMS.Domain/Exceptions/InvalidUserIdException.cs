using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserIdException : DomainException
    {
        public InvalidUserIdException()
            : base("Invalid user ID...")
        {
        }
        public InvalidUserIdException(string id)
            : base($"Invalid user ID: {id}")
        {
        }
    }
}
