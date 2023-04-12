using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;

namespace AGPoker.Entites.Game.Tables.ValueObjects
{
    public static class CardsCombination
    {
        public static IReadOnlyCollection<Player> GetWinners(List<Player> players, List<Card> cardsFromTable)
        {
            GetWinnersValidation(players, cardsFromTable);
            var playersWithGreatestCombination = players.Select(p =>
            {
                var allCards = new List<Card>();
                allCards.AddRange(p.Cards);
                allCards.AddRange(cardsFromTable);
                return new
                {
                    Player = p,
                    Combination = GetCombination(allCards)
                };
            })
            .GroupBy(pc => pc.Combination)
            .OrderBy(pc => pc.Key)
            .First();

            return playersWithGreatestCombination.Select(p => p.Player).ToList();
        }

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

            return valueCombination;
        }

        private static void GetWinnersValidation(List<Player> players, List<Card> cardsFromTable)
        {
            if(!AreAllPlayersHaveCards(players) && ArePlayersEmpty(players))
                throw new ArgumentException(nameof(players));

            if(!AreENoughCardsInTable(cardsFromTable))
                throw new ArgumentException(nameof(cardsFromTable));
        }

        private static bool AreENoughCardsInTable(List<Card> cardsFromTable)
            => cardsFromTable is not null && cardsFromTable.Count == 5;

        private static bool AreAllPlayersHaveCards(List<Player> players)
            => players.All(p => p.Cards.Count == 2);

        private static bool ArePlayersEmpty(List<Player> players)
            => players is null || players.Count == 0;

        private static CardResult? GetCardValueCombination(List<Card> cards)
        {
            var groupped = cards.GroupBy(c => c.Value);

            var fourOfKind = IsFourOfKind(groupped);
            if (fourOfKind is not null)
                return fourOfKind;

            var threeOfKind = IsThreeOfKind(groupped);

            if (threeOfKind is not null)
            {
                var twoOfKindExceptThreeCardsAlreadyCHecked = IsPair(groupped.Where(g => g.Key != threeOfKind.HighestCards.First()));
                if(twoOfKindExceptThreeCardsAlreadyCHecked is not null)
                {
                    return new CardResult(Combination.FullHouse, threeOfKind.HighestCards);
                }

                return threeOfKind;
            }

            var pair = IsPair(groupped);
            if (pair is not null)
            {
                var secondPair = IsPair(groupped.Where(g => g.Key != pair.HighestCards.First()));
                if(secondPair is not null)
                {
                    var fithHighestCard = cards
                        .Where(c => c.Value != pair.HighestCards.First()
                             && secondPair.HighestCards.First() != c.Value)
                        .OrderByDescending(c => c.Value)
                        .First();
                    return new CardResult(Combination.TwoPair, new List<ECardValue>
                    {
                        pair.HighestCards.First(),
                        secondPair.HighestCards.First(),
                        fithHighestCard.Value
                    });
                }

                return pair;
            }

            return new CardResult(Combination.HighCard, cards.OrderByDescending(c => c.Value).Select(c => c.Value).ToList());
        }

        private static CardResult? IsPair(IEnumerable<IGrouping<ECardValue, Card>> groupped)
            => IsExactNumberOfCards(groupped, 2, Combination.OnePair);

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
            var orderedByValue = cardsValues.OrderBy(c => c).Distinct().ToList();
            if (orderedByValue.Count < 5)
                return null;

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
    }

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

            if(Combination < other.Combination) return 1;
            if(Combination > other.Combination) return -1;
            throw new NotImplementedException();
        }

        private void CreationValidation(List<ECardValue> cardsOrder)
        {
            if(cardsOrder is null || cardsOrder.Count == 0)
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
