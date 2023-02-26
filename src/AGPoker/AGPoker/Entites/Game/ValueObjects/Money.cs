using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.ValueObjects
{
    public class Money : ValueObject
    {
        private Money(int value) {
            CreationValidation(value);
            Value = value;
        }

        public int Value { get; private set; }

        public static Money None
            => new(0);
        public static Money Create(int value)
            => new(value);

        public void Split(Money amount)
        {
            if (amount == this)
                throw new ArgumentException();

            SplitValidation(amount);
            Value -= amount.Value;
        }

        public Money TakeAll()
        {
            var result = Create(Value);
            Value = 0;

            return result;
        }

        private void CreationValidation(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException(nameof(value));
            }
        }

        private void SplitValidation(Money amount)
        {
            if(amount.Value > Value)
                throw new ArgumentException(nameof(amount));
        }

        public static Money operator +(Money money1, Money money2)
            => Create(money1.Value + money2.Value);

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

        public bool Any
            => Value != 0;
    }
}
