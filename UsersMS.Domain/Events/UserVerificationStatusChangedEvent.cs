using UsersMS.Core.Domain;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Events
{
    public class UserVerificationStatusChangedEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public bool IsVerified { get; }

        public string DispatcherId => UserId.ToString();
        public string EventName => nameof(UserVerificationStatusChangedEvent);
        public DateTime OccurredOn { get; }
        public object? Context => this;

        public UserVerificationStatusChangedEvent(UserId userId, bool isVerified)
        {
            UserId = userId;
            IsVerified = isVerified;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
