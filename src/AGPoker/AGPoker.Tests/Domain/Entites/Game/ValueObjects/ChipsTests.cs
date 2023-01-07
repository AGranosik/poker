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
        public void Amount_HigherThan0_Success()
        {
            var chips = Chips.Create(2);
            chips.Should().NotBeNull();
            chips.Amount.Value.Should().Be(2);
        }

        [Test]
        public void TakeAwayChips_CannotBeLargerThanCurrentAmount_ThrowsException()
        {
            var chips = Chips.Create(2);
            var func = () => chips.TakeAwayChips(Money.Create(4));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void TakeAwayChips_CannotTake0_ThrowsException()
        {
            var chips = Chips.Create(2);
            var func = () => chips.TakeAwayChips(Money.Create(0));
            func.Should().Throw<ArgumentException>();
        }
    }
}
