using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Tables.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class CardResult_CompareTests
    {
        [Test]
        public void CompareTo_AnotherCannotBeNull_ThrowsException()
        {
            var cardResult = new CardResult(Combination.Flush, new List<ECardValue>() { ECardValue.Three});
            var func = () => cardResult.CompareTo(null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CompareTo_GreaterCombination_Return1()
        {
            var greaterCombination = new CardResult(Combination.Flush, new List<ECardValue>() { ECardValue.Three });
            var lesserCombination = new CardResult(Combination.TwoPair, new List<ECardValue>() { ECardValue.Three });
            var greaterResult = greaterCombination.CompareTo(lesserCombination);
            greaterResult.Should().Be(1);

            var lessetResult = lesserCombination.CompareTo(greaterCombination);
            lessetResult.Should().Be(-1);
        }

        [Test]
        public void CompareTo_GreaterCombination_Return1_2()
        {
            var greaterCombination = new CardResult(Combination.OnePair, new List<ECardValue>() { ECardValue.Three });
            var lesserCombination = new CardResult(Combination.HighCard, new List<ECardValue>() { ECardValue.Three });
            var greaterResult = greaterCombination.CompareTo(lesserCombination);
            greaterResult.Should().Be(1);

            var lessetResult = lesserCombination.CompareTo(greaterCombination);
            lessetResult.Should().Be(-1);
        }

        [Test]
        public void CompareTo_SameCombinationHigherCardDecide_StraightFlush_Success() // test that only neccessary cards are taken && should take only neccesary cards when created
        {
            var greaterCombination = new CardResult(Combination.StraightFlush, new List<ECardValue>() { ECardValue.Ace, ECardValue.King, ECardValue.Quenn, ECardValue.Jack, ECardValue.Ten, });
            var lesserCombination = new CardResult(Combination.StraightFlush, new List<ECardValue>() { ECardValue.Three });
            var greaterResult = greaterCombination.CompareTo(lesserCombination);
            greaterResult.Should().Be(1);

            var lessetResult = lesserCombination.CompareTo(greaterCombination);
            lessetResult.Should().Be(-1);
        }
    }
}
