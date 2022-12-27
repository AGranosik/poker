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
            var func = () => new Card(notValidSymbol, "3");
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase('D')]
        [TestCase('H')]
        [TestCase('S')]
        [TestCase('C')]
        public void Card_AllValidSymbol_ThrowsException(char validSymbol)
        {
            var func = () => new Card(validSymbol, "3");
            func.Should().NotThrow();
        }

        [Test]
        [TestCase("1")]
        [TestCase("Z")]
        [TestCase("F")]
        [TestCase("E")]
        [TestCase("D")]
        [TestCase("C")]
        [TestCase("B")]
        [TestCase("14")]
        [TestCase("13")]
        [TestCase("12")]
        [TestCase("11")]
        public void Card_NotAllValidValues_ThrowsException(string notValidValue)
        {
            var func = () => new Card('D', notValidValue);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase("A")]
        [TestCase("K")]
        [TestCase("Q")]
        [TestCase("J")]
        [TestCase("10")]
        [TestCase("9")]
        [TestCase("8")]
        [TestCase("7")]
        [TestCase("6")]
        [TestCase("5")]
        [TestCase("4")]
        [TestCase("3")]
        [TestCase("2")]
        public void Card_AllValidValues_ThrowsException(string validValue)
        {
            var func = () => new Card('D', validValue);
            func.Should().NotThrow();
        }
    }
}
