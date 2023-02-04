using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
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
        public void MakeBet_CannotBeHigherThanCurrentAmount_ThrowsException()
        {
            var player = Player.Create("hehe", "hehe");
            var moneyToTake = Money.Create(520);
            var func = () => player.Raise(moneyToTake);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void MakeBid_Call_Success()
        {
            var player = Player.Create("hehe", "hehe");
            var bet = player.Call();
            bet.Should().NotBeNull();
            (bet.Chips.Amount.Value == 0).Should().BeTrue();
            bet.BidType.Should().Be(BidType.Call);
        }

        [Test]
        public void MakeBet_CanBeEqualToCurrentAmount_AllInFlag()
        {
            var player = Player.Create("hehe", "hehe");
            var moneyToTake = Money.Create(500);
            var bet = player.Raise(moneyToTake);
            bet.Should().NotBeNull();
            (bet.Player == player).Should().BeTrue();
            (bet.Chips.Amount == moneyToTake).Should().BeTrue();
            bet.AllIn.Should().BeTrue();
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

        [Test]
        public void TakeCards_CardsAssignedToPlayer_Success()
        {
            var player = Player.Create("hehe", "hehe");
            var cards = new List<Card>()
            {
                new Card(Card.PossibleSymbols.ElementAt(0), Card.PossibleValues.ElementAt(0)),
                new Card(Card.PossibleSymbols.ElementAt(0), Card.PossibleValues.ElementAt(1))
            };
            player.TakeCards(cards);

            var playerCards = player.Cards;
            cards.All(c => playerCards.Any(pc => c == pc))
                .Should().BeTrue();
        }
    }
}
