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
            var func = () => Bet.Raise(null, null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Bet.Raise(Money.Create(2), null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bet_ChipsCannotBeLowerThan_ThrowsException()
        {
            var func = () => Bet.Raise(Money.Create(-2), Player.Create("hehe", "hehe"));
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Bet_ChipsCanBe0_Sucess()
        {
            var func = () => Bet.Call(Player.Create("hehe", "hehe"), Money.None);
            func.Should().NotThrow<ArgumentException>();
        }

        [Test]
        public void Bet_AllInBetIsThatType_Success()
        {
            var bet = Bet.AllIn(Player.Create("hehe", "hehe"), Money.Create(20));
            bet.Should().NotBeNull();
            bet.BetType.Should().Be(BetType.AllIn);
        }
    }
}
