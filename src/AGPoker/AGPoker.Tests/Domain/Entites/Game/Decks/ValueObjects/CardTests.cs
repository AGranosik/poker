using AGPoker.Entites.Game.Decks.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Decks.ValueObjects
{
    [TestFixture]
    internal class CardTests
    {
        [Test]
        [TestCase('A')]
        [TestCase('B')]
        [TestCase('E')]
        [TestCase('F')]
        [TestCase('G')]
        [TestCase('A')]
        [TestCase('Y')]
        [TestCase('A')]
        public void Card_NotValidSymbol_ThrowsException(char notValidSymbol)
        {
            var func = () => new Card(notValidSymbol, ECardValue.Three);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase('D')]
        [TestCase('H')]
        [TestCase('S')]
        [TestCase('C')]
        public void Card_AllValidSymbol_ThrowsException(char validSymbol)
        {
            var func = () => new Card(validSymbol, ECardValue.Three);
            func.Should().NotThrow();
        }

        [Test]
        [TestCase(ECardValue.Ace)]
        [TestCase(ECardValue.King)]
        [TestCase(ECardValue.Quenn)]
        [TestCase(ECardValue.Jack)]
        [TestCase(ECardValue.Ten)]
        [TestCase(ECardValue.Nine)]
        [TestCase(ECardValue.Eight)]
        [TestCase(ECardValue.Seven)]
        [TestCase(ECardValue.Six)]
        [TestCase(ECardValue.Five)]
        [TestCase(ECardValue.Four)]
        [TestCase(ECardValue.Three)]
        [TestCase(ECardValue.Two)]
        public void Card_AllValidValues_ThrowsException(ECardValue validValue)
        {
            var func = () => new Card('D', validValue);
            func.Should().NotThrow();
        }
    }
}
