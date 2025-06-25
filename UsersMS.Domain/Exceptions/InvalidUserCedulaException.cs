using UsersMS.Core.Domain;

namespace UsersMS.Domain.Exceptions
{
    public class InvalidUserCedulaException : DomainException
    {
        public InvalidUserCedulaException(string cedula)
            : base($"Invalid cédula: {cedula}")
        {
        }
    }
}
