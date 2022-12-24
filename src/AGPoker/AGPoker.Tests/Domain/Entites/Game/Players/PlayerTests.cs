using AGPoker.Entites.Game.Game.Players;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Players
{
    [TestFixture]
    internal class PlayerTests
    {
        [Test]
        public void PlayerEquality_NamesHaveToBeTheSame_ReturnsFalse()
        {
            var player1 = Player.Create("hehe", "fiufui");
            var player2 = Player.Create("hehe2", "fiufui");

            (player1 == player2).Should().BeFalse();
        }

        [Test]
        public void PlayerEquality_SurnameHaveToBeTheSame_ReturnsFalse()
        {
            var player1 = Player.Create("hehe2", "fiufui");
            var player2 = Player.Create("hehe2", "fiufui2");

            (player1 == player2).Should().BeFalse();
        }

        [Test]
        public void PlayerEquality_SurnameAndNameEquals_ReturnsTrue()
        {
            var player1 = Player.Create("hehe2", "fiufui2");
            var player2 = Player.Create("hehe2", "fiufui2");

            (player1 == player2).Should().BeTrue();
        }

        [Test]
        public void PlayerEquality_ReferenceEquality_ReturnTrue()
        {
            var player1 = Player.Create("hehe2", "fiufui2");

            (player1 == player1).Should().BeTrue();
        }
    }
}
