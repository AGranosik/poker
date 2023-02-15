using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class PotWinnerTests
    {
        [Test]
        public void PotWinner_PlayersCannotBeNull_ThrowsException()
        {
            var func = () => PotWinner.Create(null, null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void PotWinner_PlayersCannotBeEmpty_ThrowsException()
        {
            var func = () => PotWinner.Create(new List<Player>(), null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PotWinner_WinningPrizeCannotBeNull_ThrowsException()
        {
            var func = () => PotWinner.Create(new List<Player>() { Player.Create("hehe", "ssada") }, null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void PotWinner_CannotBe0_ThrowsException()
        {
            var func = () => PotWinner.Create(new List<Player>() { Player.Create("hehe", "ssada") }, Money.Create(0));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PotWinner_Creation_Success()
        {
            var potWinners = PotWinner.Create(new List<Player>() { Player.Create("hehe", "ssada") }, Money.Create(20));
            potWinners.Should().NotBeNull();
            potWinners.Winners.Count.Should().Be(1);
            potWinners.WinningPrize.Value.Should().Be(20);
        }
    }
}
