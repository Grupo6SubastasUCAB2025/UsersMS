using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserEmailException : DomainException
    {

        public InvalidUserEmailException()
            : base("Invalid user email")
        {
        }

        public InvalidUserEmailException(string email)
            : base($"Invalid user email: {email}")
        {
        }
    }
}
