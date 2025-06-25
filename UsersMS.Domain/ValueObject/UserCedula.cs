using System.Text.RegularExpressions;
using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserCedula : ValueObject<UserCedula>
    {
        public string Cedula { get; }

        public string Value => Cedula;

        public UserCedula(string cedula)
        {
            if (!IsValid(cedula))
                throw new InvalidUserCedulaException(cedula);

            Cedula = cedula;
        }

        public override bool Equals(UserCedula other)
        {
            return Cedula == other.Cedula;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Cedula;
        }

        private static bool IsValid(string value)
        {
            return Regex.IsMatch(value, @"^[VPEJG]-\d{1,9}$");
        }

        public override string ToString()
        {
            return Cedula;
        }
    }
}
