using AGPoker.Entites.Game.Decks;
using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public class Flop: TableStage
    {
        public Flop(List<Card> cards, int n) : base(cards, n)
        {
        }

        public static Flop Create(Deck deck)
            => new(deck.TakeNextCards(3), 3);
    }
}
