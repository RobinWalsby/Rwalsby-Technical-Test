using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class ProjectId : ValueObject
    {
        public Guid Value { get; }

        public static ProjectId Empty => new(Guid.Empty);

        public bool HasValue { get => Value != Guid.Empty; }

        private ProjectId(Guid value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static ProjectId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Project Id must be a valid Guid", nameof(Create));
            }

            return new ProjectId(value);
        }

        public static ProjectId Create(string value)
        {
            return Create(Guid.Parse(value));
        }
    }
}
