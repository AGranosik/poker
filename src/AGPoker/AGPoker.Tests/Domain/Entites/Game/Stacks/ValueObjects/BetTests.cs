using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class BetTests
    {
        private Player _player = Player.Create("hehe", "hehe");
        [Test]
        public void Bet_ChipsCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Raise(null, null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Raise(Money.Create(2), null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bet_ChipsCannotBeLowerThan_ThrowsException()
        {
            var func = () => Bet.Raise(Money.Create(-2), Player.Create("hehe", "hehe"));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_ChipsCanBe0_Sucess()
        {
            var func = () => Bet.Call(Player.Create("hehe", "hehe"), Money.None);
            func.Should().NotThrow<ArgumentException>();
        }

        [Test]
        public void Bet_AllInBetIsThatType_Success()
        {
            var bet = Bet.AllIn(Player.Create("hehe", "hehe"), Money.Create(20));
            bet.Should().NotBeNull();
            bet.BetType.Should().Be(BetType.AllIn);
        }

        [Test]
        public void Bet_FoldedBetCannotBeCut_ThrowsException()
        {
            var foldedBet = _player.Fold();
            var func = () => foldedBet.CutIntoSmaller(Money.Create(20));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_CutCannotBeHigherThanBetItself_ThrowsException()
        {
            var betToCut = _player.Raise(Money.Create(10));
            var func = () => betToCut.CutIntoSmaller(Money.Create(20));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_CannotCutWholeBet_ThrowsException()
        {
            var betToCut = _player.Raise(Money.Create(10));
            var func = () => betToCut.CutIntoSmaller(Money.Create(10));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_CutIntoSmaller_Success()
        {
            var betToCut = Bet.Call(_player, Money.Create(30));
            var cutBet = betToCut.CutIntoSmaller(Money.Create(20));
            cutBet.Should().NotBeNull();
            betToCut.Money.Value.Should().Be(10);
            cutBet.Money.Value.Should().Be(20);

            (betToCut.Player == _player 
                && cutBet.Player == _player).Should().BeTrue();
        }
    }
}
