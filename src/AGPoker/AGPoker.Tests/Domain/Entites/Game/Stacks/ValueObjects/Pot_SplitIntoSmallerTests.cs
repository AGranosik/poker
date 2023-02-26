using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_SplitIntoSmallerTests
    {
        private Pot _pot;
        private Player _player;
        private Player _player2;
        private Player _player3;

        [SetUp]
        public void SetUp()
        {
            _pot = Pot.Create();
            _player = Player.Create("hehe", "heheh");
            _player2 = Player.Create("hehe2", "heheh");
            _player3 = Player.Create("hehe3", "heheh");
        }

        [Test]
        public void Split_NoPlayers_ThrowsException()
        {
            var func = () => _pot.SplitIntoSmaller(Bet.Raise(Money.Create(20), _player));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Split_AnyBetIsHigher_ReturnsEmptyList()
        {
            _pot.Raise(Bet.Raise(Money.Create(30), _player));
            var result = _pot.SplitIntoSmaller(Bet.Raise(Money.Create(40), _player2));
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Split_AnyBetIsHigher_ThrowsException2()
        {
            _pot.Raise(Bet.Raise(Money.Create(5), _player));
            _pot.Raise(Bet.Raise(Money.Create(10), _player2));
            _pot.Raise(Bet.Raise(Money.Create(10), _player3));
            _pot.Raise(Bet.Raise(Money.Create(15), _player));
            _pot.Raise(Bet.Raise(Money.Create(10), _player2));
            _pot.Raise(Bet.Raise(Money.Create(25), _player3));

            var result = _pot.SplitIntoSmaller(Bet.Raise(Money.Create(40), _player2));
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Split_EqualsBets_ReturnsList()
        {
            _pot.Raise(Bet.Raise(Money.Create(5), _player));
            _pot.Raise(Bet.Raise(Money.Create(10), _player2));
            _pot.Raise(Bet.Raise(Money.Create(10), _player3));
            _pot.Raise(Bet.Raise(Money.Create(15), _player));
            _pot.Raise(Bet.Raise(Money.Create(30), _player2));
            _pot.Raise(Bet.Raise(Money.Create(30), _player3));
            _pot.Raise(Bet.Raise(Money.Create(20), _player));

            var result = _pot.SplitIntoSmaller(Bet.Raise(Money.Create(40), _player2));
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void Split_SimpleHigherBets_ReturnList() // simple = no need to split
        {
            _pot.Raise(Bet.Raise(Money.Create(20), _player));
            _pot.Raise(Bet.Raise(Money.Create(20), _player2));
            _pot.Raise(Bet.Raise(Money.Create(20), _player));
            _pot.Raise(Bet.Raise(Money.Create(20), _player2));

            var result = _pot.SplitIntoSmaller(Bet.Raise(Money.Create(20), _player2));
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.All(b => (b.Player == _player && b.Money.Value == 20) || (b.Player == _player2 && b.Money.Value == 20))
                .Should().BeTrue();
        }
    }
}
