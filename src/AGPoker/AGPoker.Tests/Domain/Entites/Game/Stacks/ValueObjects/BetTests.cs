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
        [Test]
        public void Bet_ChipsCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Create(null, null, BetType.Raise);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bet_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Create(Chips.Create(2), null, BetType.Raise);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bet_ChipsCannotBeLowerThan_ThrowsException()
        {
            var func = () => Bet.Create(Chips.Create(-2), Player.Create("hehe", "hehe"), BetType.Call);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_ChipsCanBe0_Sucess()
        {
            var func = () => Bet.Create(Chips.Create(0), Player.Create("hehe", "hehe"), BetType.Call);
            func.Should().NotThrow<ArgumentException>();
        }
    }
}
