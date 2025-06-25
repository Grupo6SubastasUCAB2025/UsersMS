using UsersMS.Core.Domain;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Events
{
    public class UserNameChangedEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public UserName NewName { get; }

        public string DispatcherId => UserId.ToString();
        public string EventName => nameof(UserNameChangedEvent);
        public DateTime OccurredOn { get; }
        public object? Context => this;

        public UserNameChangedEvent(UserId userId, UserName newName)
        {
            UserId = userId;
            NewName = newName;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
