using AGPoker.Entites.Game.Stacks.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_WinnerTest
    {
        private Pot _pot;

        [SetUp]
        public void SetUp()
            => _pot = Pot.Create();

        [Test]
        public void Winner_NoBetsNoWinners_ReturnsEmptyList()
        {
            var winners = _pot.Winners;
            winners.Should().NotBeNull();
            winners.Should().BeEmpty();
        }

        //[Test]
        //public void 
    }
}
