using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.ValueObjects
{
    public class Chips : ValueObject // just value of chips
    {
        private Chips(Money amount)
        {
            Amount= amount;
        }

        public Money Amount { get; private set; }

        public static Chips Create(int amount)
            => new (Money.Create(amount));

    }
}
