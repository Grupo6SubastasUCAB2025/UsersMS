using UsersMS.Core.Domain;
using UsersMS.Domain.Aggregates;
using UsersMS.Domain.Events;
using UsersMS.Domain.Exceptions;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Entities
{
    public class Administrator : AggregateRoot<UserId>
    {
        public UserName Name { get; private set; }
        public UserEmail Email { get; private set; }
        public UserPhone Phone { get; private set; }
        public UserCedula Cedula { get; private set; }
        public UserToken? Token { get; private set; }
        public bool IsVerified { get; private set; }

        public Administrator(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null)
            : base(id)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "El administrador debe tener un nombre.");
            Email = email ?? throw new ArgumentNullException(nameof(email), "El administrador debe tener un email.");
            Phone = phone ?? throw new ArgumentNullException(nameof(phone), "El administrador debe tener un teléfono.");
            Cedula = cedula ?? throw new ArgumentNullException(nameof(cedula), "El administrador debe tener una cédula.");
            IsVerified = isVerified;
            Token = token;

            ValidateState();
            AddDomainEvent(new RecordAdministratorDataEvent(id, name, email, phone, cedula, isVerified));
        }

        protected override void ValidateState()
        {
            if (Name == null) throw new InvalidUserNameException("El administrador debe tener un nombre.");
            if (Email == null) throw new InvalidUserEmailException("El administrador debe tener un email.");
            if (Phone == null) throw new InvalidUserPhoneException("El administrador debe tener un teléfono.");
            if (Cedula == null) throw new InvalidUserCedulaException("El administrador debe tener una cédula.");
        }

        public void ChangeName(UserName newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            ValidateState();
            AddDomainEvent(new UserNameChangedEvent(Id, newName));
        }

        public void ChangePhone(UserPhone newPhone)
        {
            Phone = newPhone ?? throw new ArgumentNullException(nameof(newPhone));
            ValidateState();
            AddDomainEvent(new UserPhoneChangedEvent(Id, newPhone));
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
            AddDomainEvent(new UserTokenChangedEvent(Id, newToken));
        }

        public void SetVerified(bool verified)
        {
            IsVerified = verified;
            AddDomainEvent(new UserVerificationStatusChangedEvent(Id, verified));
        }
    }
}

