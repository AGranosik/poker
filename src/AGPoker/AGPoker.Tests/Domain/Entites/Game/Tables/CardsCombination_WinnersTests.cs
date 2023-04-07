using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Tables.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class CardsCombination_WinnersTests
    {

        [Test]
        public void Winners_PlayersCannotBeNull_ThrowsException()
        {
            var func = () => CardsCombination.GetWinners(null, null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winners_PlayersCannotBeEmpty_ThrowsException()
        {
            var func = () => CardsCombination.GetWinners(new List<Player>(), null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winners_CardsFromTableCannotBeNull_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });
            var func = () => CardsCombination.GetWinners(new List<Player>() { player }, null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winners_CardsFromTableCannotBeEmpty_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });
            var func = () => CardsCombination.GetWinners(new List<Player>() { player }, new List<Card>());
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winners_CardsFromTableCOuntCannotBeDiffThanFive_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });
            var func = () => CardsCombination.GetWinners(new List<Player>() { player }, new List<Card>()
            {
                new Card('H', ECardValue.Ace)
            });
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winners_CardsFromTableCOuntCannotBeDiffThanFive_Success()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });
            var func = () => CardsCombination.GetWinners(new List<Player>() { player }, new List<Card>()
            {
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
            });
            func.Should().NotThrow();
        }
    }
}
