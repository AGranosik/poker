using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.ValueObjects
{
    public class Money: ValueObject
    {
        private Money(int value) {
            CreationValidation(value);
            Value= value;
        }

        public int Value { get; init; }

        public static Money Create(int value)
            => new(value);

        private void CreationValidation(int value)
        {
            if(value <= 0)
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
