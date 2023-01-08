using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Players
{
    [TestFixture]
    internal class PlayerTests
    {
        [Test]
        public void PlayerEquality_NamesHaveToBeTheSame_ReturnsFalse()
        {
            var player1 = Player.Create("hehe", "fiufui");
            var player2 = Player.Create("hehe2", "fiufui");

            (player1 == player2).Should().BeFalse();
        }

        [Test]
        public void PlayerEquality_SurnameHaveToBeTheSame_ReturnsFalse()
        {
            var player1 = Player.Create("hehe2", "fiufui");
            var player2 = Player.Create("hehe2", "fiufui2");

            (player1 == player2).Should().BeFalse();
        }

        [Test]
        public void PlayerEquality_SurnameAndNameEquals_ReturnsTrue()
        {
            var player1 = Player.Create("hehe2", "fiufui2");
            var player2 = Player.Create("hehe2", "fiufui2");

            (player1 == player2).Should().BeTrue();
        }

        [Test]
        public void PlayerEquality_ReferenceEquality_ReturnTrue()
        {
            var player1 = Player.Create("hehe2", "fiufui2");

            (player1 == player1).Should().BeTrue();
        }

        [Test]
        public void MakeBid_CannotBeHigherThanCurrentAmount_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            var moneyToTake = Money.Create(520);
            var func = () => player.MakeABid(moneyToTake);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void MakeBid_CanBeEqualToCurrentAmount_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            var moneyToTake = Money.Create(500);
            var bid = player.MakeABid(moneyToTake);
            bid.Should().NotBeNull();
            (bid.Player == player).Should().BeTrue();
            (bid.Chips.Amount == moneyToTake).Should().BeTrue();
        }

        [Test]
        public void TakeCards_CannotBeNull_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            var func = () => player.TakeCards(null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void TakeCards_CannotBeEmpty_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            var func = () => player.TakeCards(new List<Card>());
            func.Should().Throw<ArgumentException>();
        }
    }
}
