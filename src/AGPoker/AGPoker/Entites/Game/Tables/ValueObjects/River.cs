using AGPoker.Entites.Game.Decks;
using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public class River : TableStage
    {
        private River(List<Card> cards, int n) : base(cards, n)
        {
        }

        public static River Create(Deck deck)
            => new(deck.TakeNextCards(1), 1);
    }
}
