using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public abstract class TableStage
    {
        private readonly int _numberOfCards;
        public List<Card> Cards { get; init; }
        protected TableStage(List<Card> cards, int n)
        {
            Validation(cards, n);
            Cards = cards;
        }

        private void Validation(List<Card> cards, int n)
        {
            if (cards is null || cards.Count == 0)
                throw new ArgumentException();

            if(n <= 0)
                throw new ArgumentOutOfRangeException();

            if(cards.Count != n || cards.Distinct().Count() != n)
                throw new ArgumentException();
        }
    }
}
