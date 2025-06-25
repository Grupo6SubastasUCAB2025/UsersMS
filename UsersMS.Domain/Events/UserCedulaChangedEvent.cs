using UsersMS.Core.Domain;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Events
{
    public class UserCedulaChangedEvent : IDomainEvent
    {
        public UserId UserId { get; }
        public UserCedula NewCedula { get; }

        public string DispatcherId => UserId.ToString();
        public string EventName => nameof(UserCedulaChangedEvent);
        public DateTime OccurredOn { get; }
        public object? Context => this;

        public UserCedulaChangedEvent(UserId userId, UserCedula newCedula)
        {
            UserId = userId;
            NewCedula = newCedula;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
