using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserPhoneException : DomainException
    {
        public InvalidUserPhoneException(string phone)
            : base($"Invalid phone number: {phone}")
        {
        }
    }
}
