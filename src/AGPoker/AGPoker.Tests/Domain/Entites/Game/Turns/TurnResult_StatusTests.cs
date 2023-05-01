using System.Numerics;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    [TestFixture]
    internal class TurnResult_StatusTests
    {
        private Turn _turn;
        private List<Player> _players;

        [SetUp]
        public void SetUp()
        {
            _players = new List<Player>
            {
                Player.Create("hehe2", "hehe2"),
                Player.Create("hehe3", "hehe2"),
                Player.Create("hehe4", "hehe2"),
                Player.Create("hehe5", "hehe2"),
                Player.Create("hehe6", "hehe6"),
            };

            _turn = Turn.Start(_players);
        }

        [Test]
        public void Status_AfterStart_InProgress()
        {
            var status = _turn.GetTurnStatus();
            status.Should().NotBeNull();
            status.Status.Should().Be(TurnStatus.InProgress);
        }

        [Test]
        public void Status_DuringRound_InProgress()
        {
            for(int i=1; i < _players.Count; i++)
            {
                var player = _players[i];
                _turn.Bet(player, BetType.Call);
            }

            var status = _turn.GetTurnStatus();
            status.Should().NotBeNull();
            status.Status.Should().Be(TurnStatus.InProgress);
        }

        [Test]
        public void Status_FirstRoundFinished_StatusInProgress()
        {
            for (int i = 1; i < _players.Count; i++)
            {
                var player = _players[i];
                _turn.Bet(player, BetType.Call);
            }
            var turnStatus = _turn.Bet(_players[0], BetType.Call);
            turnStatus.Status.Should().Be(TurnStatus.InProgress);
        }

        [Test]
        public void Status_AllFolded_WinnersStatus()
        {
            for (int i = 1; i < _players.Count; i++)
            {
                var player = _players[i];
                _turn.Bet(player, BetType.Fold);
            }

            var status = _turn.GetTurnStatus();
            status.Should().NotBeNull();
            status.Status.Should().Be(TurnStatus.Winners);
        }

        [Test]
        public void Status_AllIn_WinnersStatus()
        {
            for (int i = 1; i < _players.Count; i++)
            {
                var player = _players[i];
                _turn.Bet(player, BetType.AllIn);
            }

            var status = _turn.GetTurnStatus();
            status.Should().NotBeNull();
            status.Status.Should().Be(TurnStatus.Winners);
        }

        [Test]
        public void Status_Call_lastRound()
        {
            EveryPlayerCall();
            _turn.NextRound();
            EveryPlayerCall();
            _turn.NextRound();
            EveryPlayerCall();
            _turn.NextRound();

            for(int i = 1; i < _players.Count; i++)
            {
                var player = _players[i];
                _turn.Bet(player, BetType.Call);
            }
            //instead of returning just call a method?
            var turnResult = _turn.Bet(_players[0], BetType.Call);
            turnResult.Status.Should().Be(TurnStatus.Winners);
        }

        private void EveryPlayerCall()
        {
            for(int i=1; i < _players.Count; i++)
            {
                var player = _players[i];
                _turn.Bet(player, BetType.Call);
            }
            _turn.Bet(_players[0], BetType.Call);
        }
    }
}
