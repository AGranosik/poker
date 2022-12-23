using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class GameTests
    {
        [Test]
        public void Game_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Game.Create(null, null);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void Game_LimitCannotBeNull_ThrowsException()
        {
            var func = () => Game.Create(Player.Create("hehe", "fiu fiu"), null);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void Game_Creation_Success()
        {
            var func = () => Game.Create(Player.Create("hehe", "fiu fiu"), new GameLimit(3));
            func.Should().NotThrow();
        }
    }
}
