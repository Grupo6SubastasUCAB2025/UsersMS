using System.Text.RegularExpressions;
using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserPassword : ValueObject<UserPassword>
    {
        public string Password { get; }

        public string Value => Password;

        public UserPassword(string password)
        {
            if (!IsValid(password))
                throw new InvalidUserPasswordException(password);

            Password = password;
        }

        public override bool Equals(UserPassword other)
        {
            return Password == other.Password;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Password;
        }

        private static bool IsValid(string value)
        {
            // Debe tener entre 8 y 20 caracteres
            if (string.IsNullOrWhiteSpace(value) || value.Length < 8 || value.Length > 20)
                return false;

            // Debe contener al menos una mayúscula, una minúscula, un número y un carácter especial
            return Regex.IsMatch(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"",.<>/?\\|]).{8,20}$");
        }

        public override string ToString()
        {
            return new string('*', Password.Length); // Para no mostrar la contraseña en texto plano
        }
    }
}

