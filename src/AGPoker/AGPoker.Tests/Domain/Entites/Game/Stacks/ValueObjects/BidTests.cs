using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class BidTests
    {
        [Test]
        public void Bid_ChipsCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Create(null, null, BidType.Raise);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bid_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Create(Chips.Create(2), null, BidType.Raise);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bid_ChipsCannotBeLowerThan_ThrowsException()
        {
            var func = () => Bet.Create(Chips.Create(-2), Player.Create("hehe", "hehe"), BidType.Call);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bid_ChipsCanBe0_Sucess()
        {
            var func = () => Bet.Create(Chips.Create(0), Player.Create("hehe", "hehe"), BidType.Call);
            func.Should().NotThrow<ArgumentException>();
        }
    }
}
