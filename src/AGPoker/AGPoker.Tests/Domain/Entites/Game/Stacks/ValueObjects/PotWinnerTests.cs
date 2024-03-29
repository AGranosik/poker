﻿using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class PotWinnerTests
    {
        private Pot _pot;
        private List<Player> _players = new List<Player>
        {
            Player.Create("1", "1"),
            Player.Create("2", "2"),
            Player.Create("3", "3")
        };

        [SetUp]
        public void SetUp()
            => _pot = Pot.Create();

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

        [Test]
        public void Winner_NoBetsNoWinners_ReturnsEmptyList()
        {
            var func = () => _pot.GetWinners();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winner_SingleBetSingleWinner_ReturnsList()
        {
            _pot.Raise(Bet.Raise(Money.Create(12), _players[0]));
            var winners = _pot.GetWinners();
            winners.Should().NotBeNull();
            winners.Winners.Count.Should().Be(1);
            winners.WinningPrize.Value.Should().Be(12);
            (winners.Winners.First() == _players[0]).Should().BeTrue();
        }

        [Test]
        public void Winner_MultipleBetsOneIsWnable_ReturnsList()
        {
            _pot.Raise(Bet.Raise(Money.Create(12), _players[0]));
            _pot.Raise(Bet.Raise(Money.Create(13), _players[1]));
            _pot.Call(_players[0]);
            _pot.Raise(Bet.Raise(Money.Create(14), _players[2]));


            var winners = _pot.GetWinners();
            winners.Should().NotBeNull();
            winners.Winners.Count.Should().Be(1);
            winners.WinningPrize.Value.Should().Be(40);
            (winners.Winners.First() == _players[2]).Should().BeTrue();
        }

        [Test]
        public void Winner_MultipleBetsOneIsWnable_ReturnsList2()
        {
            _pot.Raise(Bet.Raise(Money.Create(12), _players[0]));
            _pot.Raise(Bet.Raise(Money.Create(33), _players[1]));
            _pot.Call(_players[0]);
            _pot.Raise(Bet.Raise(Money.Create(50), _players[2]));
            _pot.Raise(Bet.Raise(Money.Create(80), _players[1]));
            _pot.Fold(Bet.Fold(_players[2]));


            var winners = _pot.GetWinners();
            winners.Should().NotBeNull();
            winners.Winners.Count.Should().Be(1);
            winners.WinningPrize.Value.Should().Be(196);
            (winners.Winners.First() == _players[1]).Should().BeTrue();
        }
    }
}
