using UsersMS.Core.Domain;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Events
{
    public class RecordBidderDataEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public UserName UserName { get; }
        public UserEmail Email { get; }
        public UserPhone Phone { get; }
        public UserCedula Cedula { get; }
        public bool IsVerified { get; }

        public RecordBidderDataEvent(
            UserId userId,
            UserName userName,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Cedula = cedula ?? throw new ArgumentNullException(nameof(cedula));
            IsVerified = isVerified;
            OccurredOn = DateTime.UtcNow;
        }

        public string DispatcherId => UserId.ToString();
        public string EventName => nameof(RecordBidderDataEvent);
        public DateTime OccurredOn { get; }
        public object? Context => this;
    }
}
