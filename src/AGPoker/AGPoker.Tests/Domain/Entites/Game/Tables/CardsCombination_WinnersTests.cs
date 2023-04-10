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
        public void Winners_CardsFromTableCountCannotBeDiffThanFive_Success()
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

        [Test]
        public void Winners_SinglePlayer_Success()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });
            var result = CardsCombination.GetWinners(new List<Player>() { player }, new List<Card>()
            {
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
                new Card('H', ECardValue.Ace),
            });
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
            result.First().Should().Be(player);
        }

        [Test]
        public void Winners_DifferentCombinations_Success()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.King),
                new Card('C', ECardValue.Nine)
            });

            var player2 = Player.Create("hehe2", "hehe");
            player2.TakeCards(new List<Card>
            {
                new Card('H', ECardValue.King),
                new Card('H', ECardValue.Nine)
            });

            var player3 = Player.Create("hehe3", "hehe");
            player3.TakeCards(new List<Card>
            {
                new Card('H', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });

            var result = CardsCombination.GetWinners(new List<Player>() { player, player2, player3 }, new List<Card>()
            {
                new Card('C', ECardValue.Ace),
                new Card('C', ECardValue.Quenn),
                new Card('C', ECardValue.Ten),
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
            });

            result.Should().NotBeNullOrEmpty();
            result.First().Should().Be(player);
        }

        [Test]
        public void Winners_DifferentCombinations_Success2()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.King),
                new Card('C', ECardValue.Nine)
            });

            var player2 = Player.Create("hehe2", "hehe");
            player2.TakeCards(new List<Card>
            {
                new Card('H', ECardValue.King),
                new Card('H', ECardValue.Nine)
            });

            var player3 = Player.Create("hehe3", "hehe");
            player3.TakeCards(new List<Card>
            {
                new Card('H', ECardValue.Three),
                new Card('C', ECardValue.Three)
            });

            var result = CardsCombination.GetWinners(new List<Player>() { player, player2, player3 }, new List<Card>()
            {
                new Card('C', ECardValue.Six),
                new Card('C', ECardValue.Quenn),
                new Card('C', ECardValue.Ten),
                new Card('D', ECardValue.Three),
                new Card('D', ECardValue.Four),
            });

            result.Should().NotBeNullOrEmpty();
            result.First().Should().Be(player);
        }

        [Test]
        public void Winners_StraightFlushHigherCardDecide_Success()
        {
            var player = Player.Create("hehe", "hehe");
            player.TakeCards(new List<Card>
            {
                new Card('H', ECardValue.Six),
                new Card('C', ECardValue.Two)
            });

            var player2 = Player.Create("hehe2", "hehe");
            player2.TakeCards(new List<Card>
            {
                new Card('H', ECardValue.King),
                new Card('H', ECardValue.Nine)
            });

            var player3 = Player.Create("hehe3", "hehe");
            player3.TakeCards(new List<Card>
            {
                new Card('C', ECardValue.King),
                new Card('C', ECardValue.Nine)

            });

            var result = CardsCombination.GetWinners(new List<Player>() { player, player2, player3 }, new List<Card>()
            {
                new Card('C', ECardValue.Ace),
                new Card('C', ECardValue.Quenn),
                new Card('C', ECardValue.Five),
                new Card('C', ECardValue.Three),
                new Card('C', ECardValue.Four),
            });

            result.Should().NotBeNullOrEmpty();
            result.First().Should().Be(player3);
        }
    }
}
