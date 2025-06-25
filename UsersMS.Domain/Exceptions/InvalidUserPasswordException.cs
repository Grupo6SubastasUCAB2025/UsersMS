using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserPasswordException : DomainException
    {
        public InvalidUserPasswordException(string password)
            : base($"Invalid Password: {password}")
        {
        }
    }
}
