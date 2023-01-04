using AGPoker.Common;
using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Decks
{
    public class Deck : Entity // each deck is unique for the game
    {
        private List<Card> _cards = new();
        private static Random _random = new Random();
        private Deck(List<Card> cards)
        {
            _cards = cards;
        }

        public static Deck Create()
            => new(GetFullDeck());

        public Card GetNextCard()
        {
            var index = _random.Next(0, _cards.Count);
            var card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }

        private static List<Card> GetFullDeck()
        {
            var symbols = Card.PossibleSymbols;
            var values = Card.PossibleValues;

            var notShuffledDeck = Combine(symbols, values);
            return Shuffle(notShuffledDeck);
        }

        private static List<Card> Combine(IReadOnlyCollection<char> symbols, IReadOnlyCollection<string> values)
        {
            var result = new List<Card>(52);
            foreach(var symbol in symbols)
            {
                foreach(var value in values)
                {
                    result.Add(new Card(symbol, value));
                }
            }

            return result;
        }

        private static List<Card> Shuffle(List<Card> cards)
        {
            for (var i = cards.Count - 1; i > 0; i--)
            {
                int indexToSwap = _random.Next(i + 1);
                (cards[indexToSwap], cards[i]) = (cards[i], cards[indexToSwap]);
            }

            return cards;
        }
    }
}
