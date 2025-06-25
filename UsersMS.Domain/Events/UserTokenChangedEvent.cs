using UsersMS.Core.Domain;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Events
{
    public class UserTokenChangedEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public UserToken NewToken { get; }

        public string DispatcherId => UserId.ToString();
        public string EventName => nameof(UserTokenChangedEvent);
        public DateTime OccurredOn { get; }
        public object? Context => this;

        public UserTokenChangedEvent(UserId userId, UserToken newToken)
        {
            UserId = userId;
            NewToken = newToken;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
