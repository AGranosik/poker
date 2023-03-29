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
        public void CardsCombination_SimpleStraightFlush_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Seven),
                new Card('D', ECardValue.Five),
                new Card('D', ECardValue.Eight),
                new Card('D', ECardValue.Two),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.StraightFlush);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Eight);
        }

        [Test]
        public void CardsCombination_DifferentSymbolsStraightFlush_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Six),
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Seven),
                new Card('D', ECardValue.Five),
                new Card('C', ECardValue.Eight),
                new Card('C', ECardValue.Two),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.StraightFlush);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Seven);
        }

        [Test]
        public void CardsCombination_StraightFlushAceAtTheEnd_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ace),
                new Card('C', ECardValue.Three),
                new Card('D', ECardValue.King),
                new Card('C', ECardValue.Seven),
                new Card('D', ECardValue.Quenn),
                new Card('D', ECardValue.Jack),
                new Card('D', ECardValue.Ten),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.StraightFlush);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Ace);
        }

        [Test]
        public void CardsCombination_StraightFlushAceAtTheBeggining_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ace),
                new Card('D', ECardValue.Two),
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Five),
                new Card('D', ECardValue.Jack),
                new Card('D', ECardValue.Ten),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.StraightFlush);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Ace);
        }

        [Test]
        public void CardsCombination_NotStraightFlushWithAceAtTheBeginning_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ace),
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
                new Card('D', ECardValue.Five),
                new Card('D', ECardValue.Six),
                new Card('C', ECardValue.Jack),
                new Card('C', ECardValue.Ten),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().BeNull();
        }

        [Test]
        public void CardsCombination_FourOfKind_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('S', ECardValue.Ace),
                new Card('C', ECardValue.Ace),
                new Card('D', ECardValue.Six),
                new Card('C', ECardValue.Jack),
                new Card('C', ECardValue.Ten),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.FourOfKind);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Ace);
            var secondCard = result.HighestCards.ElementAt(1);
            secondCard.Should().Be(ECardValue.Jack);
        }

        [Test]
        public void CardsCombination_FourOfKind_Success2()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ten),
                new Card('C', ECardValue.Ten),
                new Card('D', ECardValue.Six),
                new Card('H', ECardValue.Ten),
                new Card('C', ECardValue.Jack),
                new Card('S', ECardValue.Ten),
                new Card('C', ECardValue.Eight),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.FourOfKind);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Ten);
            var secondCard = result.HighestCards.ElementAt(1);
            secondCard.Should().Be(ECardValue.Jack);
        }

        [Test]
        public void CardsCombination_FullHouse_Success()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ten),
                new Card('C', ECardValue.Ten),
                new Card('D', ECardValue.Six),
                new Card('H', ECardValue.Ten),
                new Card('C', ECardValue.Jack),
                new Card('S', ECardValue.Jack),
                new Card('C', ECardValue.Eight),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.FullHouse);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Ten);
            var secondCard = result.HighestCards.ElementAt(1);
            secondCard.Should().Be(ECardValue.Jack);
        }

        [Test]
        public void CardsCombination_FullHouse_Success2()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ten),
                new Card('C', ECardValue.Ten),
                new Card('D', ECardValue.Ten),
                new Card('H', ECardValue.Eight),
                new Card('C', ECardValue.Jack),
                new Card('S', ECardValue.Jack),
                new Card('D', ECardValue.Jack),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.FullHouse);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Jack);
            var secondCard = result.HighestCards.ElementAt(1);
            secondCard.Should().Be(ECardValue.Ten);
        }

        [Test]
        public void CardsCombination_FullHouse_Success3()
        {
            var cards = new List<Card>
            {
                new Card('D', ECardValue.Ten),
                new Card('C', ECardValue.Ten),
                new Card('D', ECardValue.Ten),
                new Card('H', ECardValue.Ace),
                new Card('C', ECardValue.Two),
                new Card('S', ECardValue.Two),
                new Card('D', ECardValue.Two),
            };

            var result = CardsCombination.GetCombination(cards);
            result.Should().NotBeNull();
            result.Combination.Should().Be(Combination.FullHouse);
            var highestCard = result.HighestCards.First();
            highestCard.Should().Be(ECardValue.Ten);
            var secondCard = result.HighestCards.ElementAt(1);
            secondCard.Should().Be(ECardValue.Two);
        }
    }
}
