using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    internal class CardsCombination
    {
        public void GetCOmbination(List<Card> cards)
        {

        }
    }

    internal class CardResult
    {
        public CardResult(Combination combination, ECardValue highestCard)
        {
            Combination = combination;
            HighestCard = highestCard;
        }

        public Combination Combination { get; init; }
        public ECardValue HighestCard { get; init; }
    }

    internal enum Combination
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
