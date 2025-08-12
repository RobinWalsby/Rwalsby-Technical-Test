namespace Technical_Test.Domain.Base
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public static bool operator !=(ValueObject? a, ValueObject? b)
        {
            return !(a == b);
        }

        public static bool operator ==(ValueObject? a, ValueObject? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public override bool Equals(object? obj)
        {
            return obj is ValueObject other && EqualsCore(other);
        }

        public bool Equals(ValueObject? other) => EqualsCore(other);

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate(17, (current, hash) => current * 31 + hash);
        }

        protected abstract IEnumerable<object?> GetEqualityComponents();

        private bool EqualsCore(ValueObject? obj)
        {
            return obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
    }
}
