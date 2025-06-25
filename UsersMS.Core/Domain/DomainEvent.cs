namespace UsersMS.Core.Domain
{
    public interface IDomainEvent
    {
        string DispatcherId { get; }
        string EventName { get; }
        DateTime OccurredOn { get; }
        object? Context { get; }
    }

    public static class DomainEventFactory
    {
        public static IDomainEvent Create<T>(string eventName, string dispatcherId, T context)
        {
            return new DomainEventImpl<T>(dispatcherId, eventName, DateTime.UtcNow, context);
        }

        private class DomainEventImpl<T> : IDomainEvent
        {
            public string DispatcherId { get; }
            public string EventName { get; }
            public DateTime OccurredOn { get; }
            public object? Context { get; }

            public DomainEventImpl(string dispatcherId, string eventName, DateTime occurredOn, T context)
            {
                DispatcherId = dispatcherId ?? throw new ArgumentNullException(nameof(dispatcherId), "DispatcherId cannot be null.");
                EventName = eventName ?? throw new ArgumentNullException(nameof(eventName), "EventName cannot be null.");
                OccurredOn = occurredOn;
                Context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null.");
            }
        }
    }
}
