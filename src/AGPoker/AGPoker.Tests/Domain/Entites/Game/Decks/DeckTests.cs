using AGPoker.Entites.Game.Decks;
using AGPoker.Entites.Game.Decks.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Decks
{
    [TestFixture]
    internal class DeckTests
    {
        private Deck _deck;

        [SetUp]
        public void SetUp()
            => _deck = Deck.Create();

        [Test]
        public void Deck_GetNextCard_NotThrowsException()
        {
            var func = () => _deck.GetNextCard();
            func.Should().NotThrow();
        }

        [Test]
        public void Deck_GetNextCard_NotNull()
        {
            var card = _deck.GetNextCard();
            card.Should().NotBeNull();
        }

        [Test]
        // no duplicates
        public void Deck_GetAllCards_Success()
        {
            var alreadyDrawnCards = new List<Card>(52);
            for(int i =0; i < 52; i++)
            {
                var card = _deck.GetNextCard();
                if (!alreadyDrawnCards.Any(c => c ==card))
                    alreadyDrawnCards.Add(card);
            }

            alreadyDrawnCards.Count.Should().Be(52);
        }

        [Test]
        public void Deck_GetMoreCards_ThrowsException()
        {
            var alreadyDrawnCards = new List<Card>(53);
            for (int i = 0; i < 52; i++)
            {
                var card = _deck.GetNextCard();
                alreadyDrawnCards.Add(card);
            }

            var func = () => _deck.GetNextCard();
            func.Should().Throw<Exception>();
        }
    }
}
