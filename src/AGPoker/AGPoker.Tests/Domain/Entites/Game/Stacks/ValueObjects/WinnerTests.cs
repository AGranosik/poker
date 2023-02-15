using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class WinnerTests
    {
        [Test]
        public void Winner_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Winner.Create(null, null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Winner_WinningPrizeCannotBeNull_ThrowsException()
        {
            var func = () => Winner.Create(Player.Create("hehe", "ssada"), null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Winner_CannotBe0_ThrowsException()
        {
            var func = () => Winner.Create(Player.Create("hehe", "ssada"), Money.Create(0));
            func.Should().Throw<ArgumentException>();
        }
    }
}
