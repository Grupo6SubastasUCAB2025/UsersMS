using System.Text.RegularExpressions;
using UsersMS.Core.Domain;
using UsersMS.Domain.Exceptions;

namespace UsersMS.Domain.ValueObject
{
    public class UserId : ValueObject<UserId>
    {
        public Guid Id { get; }

        public UserId(Guid id)
        {
            if (id == Guid.Empty)
                throw new InvalidUserIdException();

            Id = id;
        }

        public UserId(string id)
        {
            if (!Guid.TryParse(id, out Guid parsedId) || parsedId == Guid.Empty)
                throw new InvalidUserIdException();

            Id = parsedId;
        }

        public Guid Value => Id;

        public override bool Equals(UserId other)
        {
            return Id == other.Id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
