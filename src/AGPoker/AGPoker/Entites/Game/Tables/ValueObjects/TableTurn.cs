using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Decks;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public class TableTurn: TableStage
    {
        public TableTurn(List<Card> cards, int n) : base(cards, n)
        {
        }

        public static TableTurn Create(Deck deck)
            => new(deck.TakeNextCards(1), 1);
    }
}
