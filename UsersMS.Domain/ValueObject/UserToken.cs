using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserToken : ValueObject<UserToken>
    {
        public string Token { get; }

        public string Value => Token;

        public UserToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidUserTokenException(token);

            Token = token;
        }

        public override bool Equals(UserToken other)
        {
            return Token == other.Token;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Token;
        }

        public override string ToString()
        {
            return Token;
        }
    }
}
