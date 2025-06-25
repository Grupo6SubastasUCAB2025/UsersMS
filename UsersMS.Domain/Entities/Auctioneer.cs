using UsersMS.Core.Domain;
using UsersMS.Domain.Aggregates;
using UsersMS.Domain.Events;
using UsersMS.Domain.Exceptions;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Entities
{
    public class Auctioneer : AggregateRoot<UserId>
    {
        public UserName Name { get; private set; }
        public UserEmail Email { get; private set; }
        public UserPhone Phone { get; private set; }
        public UserCedula Cedula { get; private set; }
        public UserToken? Token { get; private set; }
        public bool IsVerified { get; private set; }

        public Auctioneer(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null)
            : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "El subastador debe tener un nombre.");
            Email = email ?? throw new ArgumentNullException(nameof(email), "El subastador debe tener un email.");
            Phone = phone ?? throw new ArgumentNullException(nameof(phone), "El subastador debe tener un teléfono.");
            Cedula = cedula ?? throw new ArgumentNullException(nameof(cedula), "El subastador debe tener una cédula.");
            IsVerified = isVerified;
            Token = token;

            ValidateState();
            AddDomainEvent(new RecordAuctioneerDataEvent(id, name, email, phone, cedula, isVerified));
        }

        protected override void ValidateState()
        {
            if (Name == null) throw new InvalidUserNameException("El subastador debe tener un nombre.");
            if (Email == null) throw new InvalidUserEmailException("El subastador debe tener un email.");
            if (Phone == null) throw new InvalidUserPhoneException("El subastador debe tener un teléfono.");
            if (Cedula == null) throw new InvalidUserCedulaException("El subastador debe tener una cédula.");
        }

        public void ChangeName(UserName newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            ValidateState();
        }

        public void ChangePhone(UserPhone newPhone)
        {
            Phone = newPhone ?? throw new ArgumentNullException(nameof(newPhone));
            ValidateState();
        }

        public void ChangeCedula(UserCedula newCedula)
        {
            Cedula = newCedula ?? throw new ArgumentNullException(nameof(newCedula));
            ValidateState();
            AddDomainEvent(new UserCedulaChangedEvent(Id, newCedula));
        }

        public void ChangeToken(UserToken newToken)
        {
            Token = newToken;
        }

        public void SetVerified(bool verified)
        {
            IsVerified = verified;
        }
    }
}
