using AGPoker.Core.Exceptions;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_RaiseTests
    {
        private Player _player;
        private Player _secondPlayer;
        private Pot _pot;

        [SetUp]
        public void SetUp()
        {
            _player = Player.Create("hehe", "eheh");
            _secondPlayer = Player.Create("heasdasdhe", "eheh");
            _pot = Pot.Create();
        }

        [Test]
        public void Raise_CannotRaiseIfFoldedBefore_ThrowsException()
        {
            var foldBet = _player.Fold();
            _pot.Fold(foldBet);
            var raiseBet = _player.Raise(Money.Create(60));
            var func = () => _pot.Raise(raiseBet);
            func.Should().Throw<PlayerFoldedBeforeException>();
        }

        [Test]
        public void Raise_RaiseCannotBeLowerThanActualHighestBet_ThrowsException()
        {
            var highestBet = Money.Create(30);
            _pot.Raise(_secondPlayer.Raise(highestBet));

            var lowerBet = Money.Create(10);
            var func = () => _pot.Raise(_player.Raise(lowerBet));
            func.Should().Throw<BetTooLowException>();

        }

        [Test]
        public void Raise_CanBeEqualToTheHighestBet_Success()
        {
            var chipsBeforeBet = _player.Money.Value;

            var highestBet = Money.Create(30);
            _pot.Raise(_secondPlayer.Raise(highestBet));

            var lowerBet = Money.Create(30);
            var func = () => _pot.Raise(_player.Raise(lowerBet));
            func.Should().NotThrow();

            ((chipsBeforeBet - 30) == _player.Money.Value).Should().BeTrue();
        }

        [Test]
        public void Raise_CanBeEqualInTwoBets_Success()
        {
            var chipsBeforeBet = _player.Money.Value;
            var lowerBet = Money.Create(10);
            _pot.Raise(_player.Raise(lowerBet));

            var highestBet = Money.Create(30);
            _pot.Raise(_secondPlayer.Raise(highestBet));

            var toEqualBet = Money.Create(20);
            var func = () => _pot.Raise(_player.Raise(toEqualBet));
            func.Should().NotThrow();

            ((chipsBeforeBet - 30) == _player.Money.Value).Should().BeTrue();
        }

        [Test]
        public void Raise_SimpleRaise_Success()
        {
            var chipsBeforeBet = _player.Money.Value;
            var lowerBet = Money.Create(10);
            _pot.Raise(_player.Raise(lowerBet));

            var highestBet = Money.Create(30);
            _pot.Raise(_player.Raise(highestBet));

            ((chipsBeforeBet - 40) == _player.Money.Value).Should().BeTrue();
            _pot.Value.Value.Should().Be(40);
        }

        [Test]
        public void Raise_MorePlayersBettingPreviousBetsTakenIntoHighestBetCannotBeLower_ThrowsException()
        {
            var chipsBeforeBet = _player.Money.Value;
            var lowerBet = Money.Create(10);
            _pot.Raise(_player.Raise(lowerBet));

            var highestBet = Money.Create(30);
            _pot.Raise(_secondPlayer.Raise(highestBet));

            var func = () => _pot.Raise(_player.Raise(lowerBet));
            func.Should().Throw<BetTooLowException>();
        }

        [Test]
        public void Raise_Raise_EarlierBetsTakenIntoAccount_Success()
        {
            var chipsBeforeBet = _player.Money.Value;
            var lowerBet = Money.Create(10);
            _pot.Raise(_player.Raise(lowerBet));

            var highestBet = Money.Create(30);
            _pot.Raise(_secondPlayer.Raise(highestBet));
            _pot.Raise(_player.Raise(Money.Create(80)));

            ((chipsBeforeBet - 90) == _player.Money.Value).Should().BeTrue();
            _pot.Value.Value.Should().Be(120);

            //to check if highest bet is equal to 90
            _pot.Call(_secondPlayer);
            _secondPlayer.Money.Value.Should().Be(410);
            _pot.Value.Value.Should().Be(180);

        }
    }
}
