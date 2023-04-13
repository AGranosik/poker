using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public class CardResult : IComparable<CardResult>
    {
        public CardResult(Combination combination, List<ECardValue> cardsOrder)
        {
            CreationValidation(cardsOrder);
            AssignNeccessaryCardsForCombination(combination, cardsOrder);
            Combination = combination;
        }

        public Combination Combination { get; init; }
        public List<ECardValue> HighestCards { get; private set; }

        public int CompareTo(CardResult? other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Combination < other.Combination) return 1;
            if (Combination > other.Combination) return -1;

            for (int i = 0; i < HighestCards.Count; i++)
            {
                if (HighestCards[i] > other.HighestCards[i]) return 1;
                if (HighestCards[i] < other.HighestCards[i]) return -1;
            }
            return 0;
        }

        private void CreationValidation(List<ECardValue> cardsOrder)
        {
            if (cardsOrder is null || cardsOrder.Count == 0)
                throw new ArgumentException(nameof(cardsOrder));
        }

        private void AssignNeccessaryCardsForCombination(Combination combination, List<ECardValue> cardsOrder)
        {
            switch (combination)
            {
                case Combination.StraightFlush:
                case Combination.Straight:
                    HighestCards = cardsOrder.Take(1).ToList();
                    break;
                case Combination.FourOfKind:
                case Combination.FullHouse:
                    HighestCards = cardsOrder.Take(2).ToList();
                    break;
                case Combination.Flush:
                case Combination.HighCard:
                    HighestCards = cardsOrder.Take(5).ToList();
                    break;
                case Combination.ThreeOfKind:
                case Combination.TwoPair:
                    HighestCards = cardsOrder.Take(3).ToList();
                    break;
                case Combination.OnePair:
                    HighestCards = cardsOrder.Take(4).ToList();
                    break;
            }
        }
    }

    public enum Combination
    {
        StraightFlush,
        FourOfKind,
        FullHouse,
        Flush,
        Straight,
        ThreeOfKind,
        TwoPair,
        OnePair,
        HighCard
    }
}
