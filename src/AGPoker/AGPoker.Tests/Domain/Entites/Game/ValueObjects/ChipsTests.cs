using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.ValueObjects
{
    [TestFixture]
    public class ChipsTests
    {
        [Test]
        public void Amount_CannotBeNegative_ThrowsException()
        {
            var func = () => Chips.Create(-2);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Amount_CannotBe0_ThrowsException()
        {
            var func = () => Chips.Create(0);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Amount_CHigherThan0_Success()
        {
            var chips = Chips.Create(2);
            chips.Should().NotBeNull();
            chips.Amount.Value.Should().Be(2);
        }
    }
}
