using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Pots.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Pots.ValueObjects
{
    [TestFixture]
    internal class BidTests
    {
        [Test]
        public void Bid_ChipsCannotBeNull_ThrowsException()
        {
            var func = () => Bid.Create(null, null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bid_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Bid.Create(Chips.Create(2), null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bid_ChipsCannotBeLowerThan_ThrowsException()
        {
            var func = () => Bid.Create(Chips.Create(-2), Player.Create("hehe", "hehe"));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bid_ChipsCannotBe0_ThrowsException()
        {
            var func = () => Bid.Create(Chips.Create(0), Player.Create("hehe", "hehe"));
            func.Should().Throw<ArgumentException>();
        }
    }
}
