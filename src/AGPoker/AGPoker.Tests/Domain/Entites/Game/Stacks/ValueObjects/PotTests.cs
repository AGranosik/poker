using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class PotTests
    {
        private Pot _pot;
        private Player _player = Player.Create("hehe", "hehe");
        private Player _player2 = Player.Create("hehe2", "hehe2");

        [SetUp]
        public void SetUp()
            => _pot = Pot.Create();

        [Test]
        public void Pot_TakeSingleBid_Success()
        {
            var bid = Bid.Create(Chips.Create(20), _player);
            _pot.TakeABid(bid);
            _pot.Value.Value.Should().Be(20);
        }

        [Test]
        public void TakeBid_CannotBeLowerThanHighest_ThrowsException()
        {
            var bid = Bid.Create(Chips.Create(20), _player);
            _pot.TakeABid(bid);
            bid = Bid.Create(Chips.Create(10), _player2);
            var func = () => _pot.TakeABid(bid);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void TakeBid_CannotCheckIfNotEqualToHighestBid_ThrowsException()
        {
            var bid = Bid.Create(Chips.Create(20), _player);
            _pot.TakeABid(bid);
            bid = _player2.Check();
            var func = () => _pot.TakeABid(bid);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void TakeBid_CanCheckIfEqualToHighest_Success()
        {
            var bid = Bid.Create(Chips.Create(20), _player);
            _pot.TakeABid(bid);
            bid = Bid.Create(Chips.Create(20), _player2);
            _pot.TakeABid(bid);
            bid = _player.Check();
            var func = () => _pot.TakeABid(bid);
            func.Should().NotThrow<ArgumentException>();

        }

        [Test]
        public void TakeBid_NextBidCanBeHigherThanFirst_Success()
        {
            var bid = Bid.Create(Chips.Create(10), _player);
            _pot.TakeABid(bid);
            bid = Bid.Create(Chips.Create(20), _player2);
            var func = () => _pot.TakeABid(bid);
            func.Should().NotThrow();
            _pot.Value.Value.Should().Be(30); // remove many lower invocation like .Value.Value
        }

        [Test]
        public void TakeBid_NextBidCanBeEqualToFirst_Success()
        {
            var bid = Bid.Create(Chips.Create(20), _player);
            _pot.TakeABid(bid);
            bid = Bid.Create(Chips.Create(20), _player2);
            var func = () => _pot.TakeABid(bid);
            func.Should().NotThrow();
            _pot.Value.Value.Should().Be(40);
        }
    }
}
