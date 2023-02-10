﻿using AGPoker.Core;
using AGPoker.Entites.Game.Game.Players;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Core
{
    [TestFixture]
    internal class CircleTests
    {
        private readonly List<Player> _players = new()
        { 
            Player.Create("1", "2"),
            Player.Create("3", "4"),
            Player.Create("4", "5"),
            Player.Create("5", "6")
        };

        [Test]
        public void Circle_CurrentElementCannotBeNull_ThrowsException()
        {
            var func = () => Circle.GetNextInCircle<Player>(null, null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Circle_ListCannotBeNull_ThrowsException()
        {
            var func = () => Circle.GetNextInCircle<Player>(Player.Create("hehe", "hehe"), null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Circle_CurrentPlayerNotInList_ThrowsException()
        {
            var playerNotInList = Player.Create("hehe", "hehe");
            var func = () => Circle.GetNextInCircle<Player>(playerNotInList, _players);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Circle_JustNExtElementInList_Success()
        {
            var nextPlayer = Circle.GetNextInCircle(_players[0], _players);
            var nextPlayerIndex = _players.IndexOf(nextPlayer);
            nextPlayerIndex.Should().Be(1);
        }

        [Test]
        public void Circle_OneBeforeLastNextElementInList_Success()
        {
            var nextPlayer = Circle.GetNextInCircle(_players[2], _players);
            var nextPlayerIndex = _players.IndexOf(nextPlayer);
            nextPlayerIndex.Should().Be(3);
        }

        [Test]
        public void Circle_lastPlayerInListNextOneSHouldBeFirst_Success()
        {
            var nextPlayer = Circle.GetNextInCircle(_players[3], _players);
            var nextPlayerIndex = _players.IndexOf(nextPlayer);
            nextPlayerIndex.Should().Be(0);
        }
    }
}
