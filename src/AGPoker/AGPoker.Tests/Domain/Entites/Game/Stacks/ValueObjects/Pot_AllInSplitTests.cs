using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_AllInSplitTests
    {
        private Pot _pot;
        private Player _player;
        private Player _player2;
        private Player _player3;

        [SetUp]
        public void SetUp()
        {
            _pot = Pot.Create();
            _player = Player.Create("hehe", "heheh", 100);
            _player2 = Player.Create("hehe2", "heheh", 200);
            _player3 = Player.Create("hehe3", "heheh", 300);
        }

        [Test]
        public void Validation_CheckIfNotAllInAlready_ThrowsExcepton()
        {
            _pot.AllIn(_player.AllIn());
            var func = () => _pot.AllIn(_player2.AllIn());
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Split_AnyBetIsHigher_ReturnsEmptyList()
        {
            var allInPLayer = Player.Create("hehe", "sss", 40);
            _pot.Raise(Bet.Raise(Money.Create(30), _player));
            var result = _pot.AllIn(allInPLayer.AllIn());
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Split_BetIsHigher_ThrowsException2()
        {
            var allInPLayer = Player.Create("hehe", "sss", 60);
            _pot.Raise(_player.Raise(Money.Create(5)));
            _pot.Raise(allInPLayer.Raise(Money.Create(10)));
            _pot.Raise(_player3.Raise(Money.Create(10)));
            _pot.Raise(_player.Raise(Money.Create(15)));
            _pot.Raise(allInPLayer.Raise(Money.Create(10)));
            _pot.Raise(_player3.Raise(Money.Create(25)));

            var result = _pot.AllIn(allInPLayer.AllIn());
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Split_EqualsBets_ReturnsList()
        {
            var allInPLayer = Player.Create("hehe", "sss", 80);
            _pot.Raise(_player.Raise(Money.Create(5)));
            _pot.Raise(allInPLayer.Raise(Money.Create(10)));
            _pot.Raise(_player3.Raise(Money.Create(10)));
            _pot.Raise(_player.Raise(Money.Create(15)));
            _pot.Raise(allInPLayer.Raise(Money.Create(30)));
            _pot.Raise(_player3.Raise(Money.Create(30)));
            _pot.Raise(_player.Raise(Money.Create(20)));

            var result = _pot.AllIn(allInPLayer.AllIn());
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Split_ReturnsBets()
        {
            _pot.Raise(_player2.Raise(Money.Create(120)));
            var bets = _pot.AllIn(_player.AllIn());
            bets.Should().NotBeNull();
            bets.Count.Should().Be(1);

            var bet = bets[0];
            bet.Money.Value.Should().Be(20);
        }
    }
}
