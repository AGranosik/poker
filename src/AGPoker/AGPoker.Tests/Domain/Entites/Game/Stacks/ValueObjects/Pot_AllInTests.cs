using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_AllInTests
    {
        private Player _player;
        private Player _secondPlayer;
        private Pot _pot;

        [SetUp]
        public void SetUp()
        {
            _player = Player.Create("hehe", "eheh", 100);
            _secondPlayer = Player.Create("heasdasdhe", "eheh", 200);
            _pot = Pot.Create();
        }

        [Test]
        public void CanTakeAllInBetPart_NoAllInBet_ThrowsException()
        {
            var foldBet = Bet.Fold(_player);
            var func = () => _pot.CanTakeAllInBetPart(foldBet);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CanTakeAllInBetPart_NoAllInBet_ThrowsException2()
        {
            var callBet = Bet.Call(_player, _player.Money);
            var func = () => _pot.CanTakeAllInBetPart(callBet);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CanTakeAllInBetPart_BetsHigherThanHighest_ReturnsTrue()
        {
            _pot.AllIn(_player);
            var allInBet = _secondPlayer.AllIn();
            var result = _pot.CanTakeAllInBetPart(allInBet);
            result.Should().BeTrue();
        }

        [Test]
        public void CanTakeAllInBetPart_BetsHigherEqualToHighest_ReturnsTrue()
        {
            _pot.Raise(_secondPlayer.Raise(Money.Create(90)));
            var allInBet = _player.AllIn();
            var result = _pot.CanTakeAllInBetPart(allInBet);
            result.Should().BeTrue();
        }

        [Test]
        public void CanTakeAllInBetPart_BetsSmallerThanHighest_ReturnsFalse()
        { 
            _pot.Raise(_secondPlayer.Raise(Money.Create(90)));
            var allInBet = _player.AllIn();
            var result = _pot.CanTakeAllInBetPart(allInBet);
            result.Should().BeFalse();
        }

        [Test]
        public void Validation_IsAllInBet_Success()
        {
            _pot.AllIn(_player);
            _pot.IsAllIn.Should().BeTrue();
        }
    }
}
