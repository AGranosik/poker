using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.ValueObjects
{
    public class Money : ValueObject
    {
        private Money(int value) {
            CreationValidation(value);
            Value = value;
        }

        public int Value { get; init; }

        public static Money Create(int value)
            => new(value);

        private void CreationValidation(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException(nameof(value));
            }
        }

        public static Money operator -(Money money1, Money money2)
            => Create(money1.Value - money2.Value);

        public static bool operator ==(Money money1, Money money2)
            =>  money1.Value == money2.Value;

        public static bool operator !=(Money money1, Money money2)
            => !(money1 == money2);

        public static bool operator >(Money money1, Money money2)
            => money1.Value > money2.Value;

        public static bool operator <(Money money1, Money money2)
            => money1.Value < money2.Value;

        public static bool operator >=(Money money1, Money money2)
            => money1.Value >= money2.Value;

        public static bool operator <=(Money money1, Money money2)
            => money1.Value <= money2.Value;
    }
}
