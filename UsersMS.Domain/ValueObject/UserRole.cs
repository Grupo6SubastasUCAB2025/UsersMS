using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserRoles : ValueObject<UserRoles>
    {
        public UserRole Role { get; }

        public UserRoles(UserRole role)
        {
            if (!IsValid(role))
                throw new InvalidUserRoleException("Rol de usuario inválido.");

            Role = role;
        }

        public override bool Equals(UserRoles other)
        {
            return Role == other.Role;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Role;
        }

        private static bool IsValid(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }

        public override string ToString()
        {
            return Role.ToString();
        }
    }

    public enum UserRole
    {
        Administrator,
        Auctioneer,
        Bidder,
        TechnicalSupport
    }
}
