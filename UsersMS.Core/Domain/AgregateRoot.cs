using UsersMS.Core.Domain;

namespace UsersMS.Domain.Aggregates
{
    public abstract class AggregateRoot<T> : Entity<T> where T : ValueObject<T>
    {
        private readonly List<IDomainEvent> _events = new();

        protected AggregateRoot(T id) : base(id)
        {
        }

        protected abstract void ValidateState();

        public IReadOnlyList<IDomainEvent> GetEvents() => _events.AsReadOnly();

        public void ClearEvents() => _events.Clear();

        protected void Apply(IDomainEvent domainEvent, bool fromHistory = false)
        {
            ValidateState();

            if (!fromHistory)
                _events.Add(domainEvent);
        }

        protected void Hydrate(IEnumerable<IDomainEvent> history)
        {
            if (history == null || !history.Any())
                throw new InvalidOperationException("No events to hydrate the aggregate.");

            foreach (var domainEvent in history)
            {
                Apply(domainEvent, fromHistory: true);
            }
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            Apply(domainEvent);
        }

        public List<IDomainEvent> PullEvents()
        {
            var pulled = _events.ToList();
            _events.Clear();
            return pulled;
        }
    }
}
