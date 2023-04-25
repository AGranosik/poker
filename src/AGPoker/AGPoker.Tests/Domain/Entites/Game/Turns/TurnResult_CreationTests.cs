using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Turns;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    [TestFixture]
    internal class TurnResult_CreationTests
    {
        [Test]
        public void Creation_InProgress_Success()
        {
            var creatioNResult = TurnResult.InProgress();
            creatioNResult.Should().NotBeNull();
            creatioNResult.Status.Should().Be(TurnStatus.InProgress);
        }

        [Test]
        public void Creation_WinnersCannotBeNull_ThrowsException()
        {
            var creationFunc = () => TurnResult.Winners(null);
            creationFunc.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_WinnerCannotBeEmpty_ThrowsException()
        {
            var creationFunc = () => TurnResult.Winners(new List<Player>());
            creationFunc.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Winners_Success()
        {
            var winners = new List<Player>()
            {
                Player.Create("hehe", "dsfgsdf")
            };
            var creationFunc = TurnResult.Winners(winners);
            creationFunc.Should().NotBeNull();

            creationFunc.Status.Should().Be(TurnStatus.Winners);
            creationFunc.WinnerPlayers.Should().BeEquivalentTo(winners);
        }
    }
}
