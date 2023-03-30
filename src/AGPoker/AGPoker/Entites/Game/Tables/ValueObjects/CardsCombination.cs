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

            var valueCombination = GetCardValueCombination(cards);

            if (valueCombination?.Combination == Combination.FourOfKind)
                return valueCombination;

            if(valueCombination?.Combination == Combination.FullHouse)
                return valueCombination;


            var flush = IsFlush(cards);
            if(flush is not null)
                return flush;

            var straight = IsStraight(cards);
            if (straight is not null)
                return straight;


            return null;
        }

        private static CardResult? GetCardValueCombination(List<Card> cards)
        {
            var groupped = cards.GroupBy(c => c.Value);

            var fourOfKind = IsFourOfKind(groupped);
            if (fourOfKind is not null)
                return fourOfKind;

            var threeOfKind = IsThreeOfKind(groupped);

            if (threeOfKind is not null)
            {
                var twoOfKindExceptThreeCardsAlreadyCHecked = IsTwoOfKind(groupped.Where(g => g.Key != threeOfKind.HighestCards.First()));
                if(twoOfKindExceptThreeCardsAlreadyCHecked is not null)
                {
                    return new CardResult(Combination.FullHouse, threeOfKind.HighestCards);
                }
            }

            return null;
        }

        private static CardResult? IsTwoOfKind(IEnumerable<IGrouping<ECardValue, Card>> groupped)
            => IsExactNumberOfCards(groupped, 2, Combination.TwoPair);

        private static CardResult? IsThreeOfKind(IEnumerable<IGrouping<ECardValue, Card>> groupped)
            => IsExactNumberOfCards(groupped, 3, Combination.ThreeOfKind);

        private static CardResult? IsFourOfKind(IEnumerable<IGrouping<ECardValue, Card>> groupped)
            => IsExactNumberOfCards(groupped, 4, Combination.FourOfKind);

        private static CardResult? IsExactNumberOfCards(IEnumerable<IGrouping<ECardValue, Card>> grouppedCards, int numberOfSameCards, Combination resultCombination)
        {
            //better name
            var sortedFromHighestToLowest = grouppedCards.OrderByDescending(gp => gp.Count())
                .ThenByDescending(gp => gp.Key);

            var sameCards = sortedFromHighestToLowest.FirstOrDefault(gp => gp.Count() >= numberOfSameCards);

            if(sameCards is not null)
            {
                var highestFromLowest = new List<ECardValue>
                {
                    sameCards.Key
                };
                highestFromLowest.AddRange(sortedFromHighestToLowest.Where(s => s.Key != sameCards.Key).Select(s => s.Key).ToList());
                return new CardResult(resultCombination, highestFromLowest);
            }

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
            var flush = IsFlush(cards);

            if (flush is null)
                return null;

            var straightInTheFlush = IsStraight(flush.HighestCards);

            if(straightInTheFlush is null)
                return null;

            if (straightInTheFlush.HighestCards.First() > flush.HighestCards.First())
                return new CardResult(Combination.StraightFlush, straightInTheFlush.HighestCards);
                
            return new CardResult(Combination.StraightFlush, flush.HighestCards);
        }

        private static CardResult? IsStraight(List<Card> cards)
            => IsStraight(cards.Select(c => c.Value).ToList());

        private static CardResult? IsStraight(List<ECardValue> cardsValues)
        {
            var orderedByValue = cardsValues.OrderBy(c => c).ToList();
            int cardsInOrder = 1;
            var highestCard = ECardValue.Two;

            for(int i = 0; i < orderedByValue.Count -1 && cardsInOrder < 5; i++)
            { 
                var currentValue = orderedByValue[i];
                var nextValue = orderedByValue[i + 1];

                if(currentValue == ECardValue.Two && cardsValues.Any(c => c == ECardValue.Ace))
                {
                    cardsInOrder+=2;
                }
                else if ((nextValue - currentValue) == 1)
                {
                    cardsInOrder++;
                    highestCard = nextValue;
                }
                else
                {
                    cardsInOrder = 1;
                    highestCard = ECardValue.Two;
                }
            }

            return cardsInOrder >= 5 ? new CardResult(Combination.Straight, new List<ECardValue> { highestCard }) : null;
        }

        private static CardResult? IsFlush(List<Card> cards)
        {
            var flushCards = cards.GroupBy(c => c.Symbol)
                .Where(s => s.Count() >= 5)
                .SelectMany(s => s)
                .ToList();

            if (!flushCards.Any())
                return null;



            return new CardResult(Combination.Flush, flushCards.OrderByDescending(c => c.Value).Select(c => c.Value).ToList());
        }

        private static ECardValue GetHighestValue(List<Card> cards)
            => cards.Max(c => c.Value);
    }

    public class CardResult
    {
        public CardResult(Combination combination, List<ECardValue> cardsOrder)
        {
            Combination = combination;
            HighestCards = cardsOrder;
        }

        public Combination Combination { get; init; }
        public List<ECardValue> HighestCards { get; init; }
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
