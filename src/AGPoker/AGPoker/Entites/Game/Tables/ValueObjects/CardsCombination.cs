using AGPoker.Entites.Game.Decks.ValueObjects;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public static class CardsCombination
    {
        public static CardResult GetCombination(List<Card> cards)
        {
            CardsValidation(cards);
            var isStraighFLush = IsStraightFlush(cards);
            if (isStraighFLush is not null)
                return isStraighFLush;

            return null;
        }

        private static void CardsValidation(List<Card> cards)
        {
            if(cards is null)
                throw new ArgumentNullException(nameof(cards));

            if (cards.Count != 7)
                throw new InvalidOperationException();
        }

        private static CardResult? IsStraightFlush(List<Card> cards)
        {
            var isStraight = IsStraight(cards);
            var isFlush = IsFlush(cards);
            if(isStraight is not null && isFlush is not null)
            {
                if (isStraight.HighestCard > isFlush.HighestCard)
                    return new CardResult(Combination.StraightFlush, isStraight.HighestCard);
                
                return new CardResult(Combination.StraightFlush, isFlush.HighestCard);
            }

            return null;
        }

        private static CardResult? IsStraight(List<Card> cards)
        {
            var orderedByValue = cards.OrderByDescending(x => x.Value).ToList();
            int cardsInOrder = 1;
            var highestCard = ECardValue.Two;

            for(int i = 0; i < orderedByValue.Count -1 && cardsInOrder < 5; i++)
            { 
                var currentCard = orderedByValue[i];
                var nextCard = orderedByValue[i + 1];

                //next card has to be Two because its ordered by value
                if(currentCard.Value == ECardValue.Three && cards.Any(c => c.Value == ECardValue.Ace))
                {
                    cardsInOrder+=2;
                    highestCard = ECardValue.Five;
                }
                else if ((currentCard.Value - nextCard.Value) == 1)
                {
                    cardsInOrder++;
                    highestCard = nextCard.Value;
                }
                else
                {
                    cardsInOrder = 1;
                    highestCard = ECardValue.Two;
                }
            }

            return cardsInOrder >= 5 ? new CardResult(Combination.Straight, highestCard) : null;
        }

        private static CardResult? IsFlush(List<Card> cards)
        {
            var flushCards = cards.GroupBy(c => c.Symbol)
                .Where(s => s.Count() >= 5)
                .SelectMany(s => s)
                .ToList();

            if (!flushCards.Any())
                return null;



            return new CardResult(Combination.Flush, GetHighestValue(flushCards));
        }

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
