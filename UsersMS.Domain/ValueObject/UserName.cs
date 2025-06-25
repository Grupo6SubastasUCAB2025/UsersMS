using System.Text.RegularExpressions;
using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserName : ValueObject<UserName>
    {
        public string Name { get; }

        public string Value => Name;

        public UserName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 50)
                throw new InvalidUserNameException();

            Name = name;
        }

        public override bool Equals(UserName other)
        {
            return Name == other.Name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
