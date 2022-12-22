namespace AGPoker.Common.ValueObjects
{
    internal abstract class Name : ValueObject
    {
        protected Name(string name)
        {
            Validate(name);
            Value = name;
        }

        public string Value { get; init; }

        private static void Validate(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                throw new Exception("Cannot be null or empty");
        }
    }
}
