using System.Text.RegularExpressions;
using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserPhone : ValueObject<UserPhone>
    {
        public string Phone { get; }

        public string Value => Phone;

        public UserPhone(string phone)
        {
            if (!IsValid(phone))
                throw new InvalidUserPhoneException(phone);

            Phone = phone;
        }

        public override bool Equals(UserPhone other)
        {
            return Phone == other.Phone;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Phone;
        }

        private static bool IsValid(string value)
        {
            return Regex.IsMatch(value, @"^(0414|0424|0412|0416|0426)\d{7}$");
        }

        public override string ToString()
        {
            return Phone;
        }
    }
}
