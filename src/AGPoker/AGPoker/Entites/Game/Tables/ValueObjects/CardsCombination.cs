using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public static class CardsCombination
    {
        public static CardResult GetCombination(List<Card> cards)
        {
            CardsValidation(cards);
            var isStraight = IsStraight(cards);
            var isFLush = IsFlush(cards);

            if (isStraight && isFLush)
                return new CardResult(Combination.StraightFlush, ECardValue.Four);

            return null;
        }

        private static void CardsValidation(List<Card> cards)
        {
            if(cards is null)
                throw new ArgumentNullException(nameof(cards));

            if (cards.Count != 7)
                throw new InvalidOperationException();
        }

        private static bool IsStraight(List<Card> cards)
        {
            var orderedByValue = cards.OrderByDescending(x => x.Value).ToList();
            int cardsInOrder = 0;
            for(int i = 0; i < orderedByValue.Count -1; i++)
            { 
                var currentCard = orderedByValue[i];
                var nextCard = orderedByValue[i + 1];

                if ((currentCard.Value - nextCard.Value) == 1)
                    cardsInOrder++;
                else
                    cardsInOrder = 0;
            }

            return cardsInOrder >= 5;
        }

        private static bool IsFlush(List<Card> cards)
            => cards.GroupBy(c => c.Symbol)
                .Any(s => s.Count() >= 5);

        private static ECardValue GetHighestValue(List<Card> cards)
            => cards.Max(c => c.Value);
    }

    public class CardResult
    {
        public CardResult(Combination combination, ECardValue highestCard)
        {
            Combination = combination;
            HighestCard = highestCard;
        }

        public Combination Combination { get; init; }
        public ECardValue HighestCard { get; init; }
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
