using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Tables.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class CardsCombinationTests
    {

        [Test]
        public void CardsCombination_CardsCannotBeNull_ThrowsException()
        {
            var func = () => CardsCombination.GetCombination(null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CardsCombination_CardsCannotBeEmpty_ThrowsException()
        {
            var func = () => CardsCombination.GetCombination(new List<Card>());
            func.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void CardsCombination_NoLessThan7Cards_ThrowsException()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Five),
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Seven)
            };
            var func = () => CardsCombination.GetCombination(cards);
            func.Should().Throw<InvalidOperationException>();
        }


        [Test]
        public void CardsCombination_NoMoreThan7Cards_ThrowsException()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Five),
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Seven)
            };
            var func = () => CardsCombination.GetCombination(cards);
            func.Should().Throw<InvalidOperationException>();
        }
        [Test]
        public void CardsCombination_StraightFlush_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Five),
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Seven),
                new Card('D', ECardValue.Eight)
            };
        }
    }
}
