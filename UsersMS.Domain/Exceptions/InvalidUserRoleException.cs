using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserRoleException : DomainException
    {
        public InvalidUserRoleException()
            : base("Invalid user role...")
        {
        }
        public InvalidUserRoleException(string id)
            : base($"Invalid user role: {id}")
        {
        }
    }
}
