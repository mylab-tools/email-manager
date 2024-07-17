namespace MyLab.EmailManager.Domain.ValueObjects
{
    public class DatedValue<T>
        where T : struct
    {
        //public static DatedValue<T> Unset = new()
        //{
        //    DateTime = null,
        //    Value = default
        //};

        public static DatedValue<T> CreateUnset() => new()
        {
            DateTime = null,
            Value = default
        };

        public T Value { get; private set; }
        public DateTime? DateTime { get; private set; }

        public static DatedValue<T> CreateSet(T value, DateTime dateTime)
        {
            return new DatedValue<T>
            {
                Value = value,
                DateTime = dateTime
            };
        }

        public static DatedValue<T> CreateSet(T value)
        {
            return CreateSet(value, System.DateTime.Now);
        }
    }
}
