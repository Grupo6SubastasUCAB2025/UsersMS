using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserNameException : DomainException
    {
        public InvalidUserNameException()
            : base("Invalid user name")
        {
        }

        public InvalidUserNameException(string id)
            : base($"Invalid user name: {id}")
        {
        }
    }
}
