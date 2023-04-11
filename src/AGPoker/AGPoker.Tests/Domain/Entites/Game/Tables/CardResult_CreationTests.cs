using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Tables.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class CardResult_CreationTests
    {
        [Test]
        [TestCase(Combination.StraightFlush)]
        [TestCase(Combination.FourOfKind)]
        [TestCase(Combination.FullHouse)]
        [TestCase(Combination.Flush)]
        [TestCase(Combination.Straight)]
        [TestCase(Combination.ThreeOfKind)]
        [TestCase(Combination.TwoPair)]
        [TestCase(Combination.OnePair)]
        [TestCase(Combination.HighCard)]
        public void Creation_CardsOrderCannotBeNullDespiteCombination_ThrowsException(Combination combination)
        {
            var func = () => new CardResult(combination, null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase(Combination.StraightFlush)]
        [TestCase(Combination.FourOfKind)]
        [TestCase(Combination.FullHouse)]
        [TestCase(Combination.Flush)]
        [TestCase(Combination.Straight)]
        [TestCase(Combination.ThreeOfKind)]
        [TestCase(Combination.TwoPair)]
        [TestCase(Combination.OnePair)]
        [TestCase(Combination.HighCard)]
        public void Creation_CardsOrderCannotBeEmnptyDespiteCombination_ThrowsException(Combination combination)
        {
            var func = () => new CardResult(combination, new List<ECardValue>());
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_StraightFlushOnlyOneCardCount_Success()
        {
            var result = new CardResult(Combination.StraightFlush, new List<ECardValue>() { ECardValue.Quenn, ECardValue.Jack, ECardValue.Ten, ECardValue.Nine, ECardValue.Eight});
            result.Should().NotBeNull();
            var highestCards = result.HighestCards;
            highestCards.Should().NotBeNull();
            highestCards.Should().HaveCount(1);
            highestCards.First().Should().Be(ECardValue.Quenn);
        }

        [Test]
        public void Creation_FourOfKind_2CardsCount_Success()
        {
            var result = new CardResult(Combination.FourOfKind, new List<ECardValue> { ECardValue.Quenn, ECardValue.Ace, ECardValue.Nine, ECardValue.Two });
            result.Should().NotBeNull();
            var highestCards = result.HighestCards;
            highestCards.Should().NotBeNull();
            highestCards.Should().HaveCount(2);
            highestCards.First().Should().Be(ECardValue.Quenn); 
            highestCards.Last().Should().Be(ECardValue.Ace); 
        }

        [Test]
        public void Creation_Flush_5CardsCount_Success()
        {
            var result = new CardResult(Combination.Flush, new List<ECardValue>() { ECardValue.King, ECardValue.Jack, ECardValue.Ten, ECardValue.Nine, ECardValue.Eight });
            result.Should().NotBeNull();
            var highestCards = result.HighestCards;
            highestCards.Should().NotBeNull();
            highestCards.Should().HaveCount(5);
            highestCards.ElementAt(0).Should().Be(ECardValue.King);
            highestCards.ElementAt(1).Should().Be(ECardValue.Jack);
            highestCards.ElementAt(2).Should().Be(ECardValue.Ten);
            highestCards.ElementAt(3).Should().Be(ECardValue.Nine);
            highestCards.ElementAt(4).Should().Be(ECardValue.Eight);
        }
    }
}
