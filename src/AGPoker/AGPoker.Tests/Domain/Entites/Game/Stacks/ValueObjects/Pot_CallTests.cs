using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_CallTests
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
            _pot.Raise(_secondPlayer.Raise(Money.Create(15)));
        }

        [Test]
        public void Call_ChipsSHouldBeTakenToTHeHighestBid_Success()
        {
            var func = () => _pot.Call(_player);
            func.Should().NotThrow();

            _pot.Value.Value.Should().Be(30);
        }

        [Test]
        public void Call_ChipsShouldBeTakenFromPlayer_Success()
        {
            var moneyBeforeBet = _player.Money.Value;

            var func = () => _pot.Call(_player);
            func.Should().NotThrow();

            var moneyAfterBet = _player.Money.Value;
            moneyAfterBet.Should().Be(485);
        }

        [Test]
        public void Call_OnlyMissingChipsShouldBeTaken_Success()
        {
            var bet = _player.Raise(Money.Create(30));
            _pot.Raise(bet);

            var func = () => _pot.Call(_secondPlayer);
            func.Should().NotThrow();

            _player.Money.Value.Should().Be(470);
        }
    }
}
