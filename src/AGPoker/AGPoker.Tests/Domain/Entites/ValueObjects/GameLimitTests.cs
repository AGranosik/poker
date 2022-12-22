using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.ValueObjects
{
    [TestFixture]
    internal class GameLimitTests
    {
        [Test]
        public void GameLimit_CannotBeNegative_ThrowsException()
        {
            var func = () => new GameLimit(-2);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void GameLimit_CannotBeOne_ThrowException()
        {
            var func = () => new GameLimit(1);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void GameLimit_CannotGreaterThan14_ThrowException()
        {
            var func = () => new GameLimit(15);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void GameLimit_EqualToTwo_Success()
        {
            var func = () => new GameLimit(2);
            func.Should().NotThrow();
        }

        [Test]
        public void GameLimit_EqualToFourteen_Success()
        {
            var func = () => new GameLimit(14);
            func.Should().NotThrow();
        }
    }
}
