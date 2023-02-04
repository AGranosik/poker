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

        public Chips TakeAwayChips(Money amount)
        {
            CanTakeAwayPartOfChips(amount);
            Amount -= amount;
            return Create(amount.Value);
        }

        public static Chips Create(int amount) // money definetely
            => new (Money.Create(amount));
        
        private void CanTakeAwayPartOfChips(Money amount)
        {
            if (amount > Amount || amount.Value < 0)
                throw new ArgumentNullException("Cannot take away that amount of chips.");
        }
    }
}
