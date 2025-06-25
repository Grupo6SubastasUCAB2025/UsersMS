namespace UsersMS.Core.Domain
{
    public abstract class Entity<T> where T : ValueObject<T>
    {
        public T Id { get; private set; }

        protected Entity(T id)
        {
            Id = id;
        }

        public T GetId()
        {
            return Id;
        }

        public bool Equals(Entity<T> other)
        {
            return Id.Equals(other.GetId());
        }
    }
}
