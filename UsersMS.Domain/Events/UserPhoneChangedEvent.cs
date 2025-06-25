using UsersMS.Core.Domain;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Events
{
    public class UserPhoneChangedEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public UserPhone NewPhone { get; }

        public string DispatcherId => UserId.ToString();
        public string EventName => nameof(UserPhoneChangedEvent);
        public DateTime OccurredOn { get; }
        public object? Context => this;

        public UserPhoneChangedEvent(UserId userId, UserPhone newPhone)
        {
            UserId = userId;
            NewPhone = newPhone;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
