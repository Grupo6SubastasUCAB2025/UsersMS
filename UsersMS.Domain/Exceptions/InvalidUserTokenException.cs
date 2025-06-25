using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserTokenException : DomainException
    {
        public InvalidUserTokenException(string token)
            : base($"Invalid user token: {token}")
        {
        }
    }
}
