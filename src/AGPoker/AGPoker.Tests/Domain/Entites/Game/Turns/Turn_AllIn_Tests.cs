using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
using AGPoker.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    [TestFixture]
    internal class Turn_AllIn_Tests
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
                Player.Create("hehe6", "hehe2")
            };

            _turn = Turn.Start(_players);
        }

        [Test]
        public void AllIn_ResetCircle_Success()
        {
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[3], BetType.AllIn);
            _turn.Bet(_players[4], BetType.Call);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);

            var func = () => _turn.Bet(_players[3], BetType.Call); //shouldnt be able to bet
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void AllIn_ResetCircleAndOmittedDuringRaises_ShouldBeAbleToStartNextRound()
        {
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[3], BetType.AllIn);
            _turn.Bet(_players[4], BetType.Call);
            _turn.Bet(_players[0], BetType.Raise);
            _turn.Bet(_players[1], BetType.Raise);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[4], BetType.Raise);
            _turn.Bet(_players[0], BetType.AllIn);
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[4], BetType.Call);


            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_AllInCannotBet_ThrowsException()
        {
            GetIntoNextRoundWithAllIn();
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);

            var func = () => _turn.Bet(_players[3], BetType.Fold);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void NextRound_NextRoundWithAllInPlayer_Success()
        {
            GetIntoNextRoundWithAllIn();
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[0], BetType.Call);

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void ThirdRound_NextAllInPlayer_StartNExtRound()
        {
            GetIntoNextRoundWithAllIn();
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[0], BetType.AllIn);
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void FourthRound_NextAllInPlayer_StartNextRound()
        {
            GetIntoNextRoundWithAllIn();
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[0], BetType.Call);

            _turn.NextRound();
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.AllIn);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Call);

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        private void GetIntoNextRoundWithAllIn()
        {
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[3], BetType.AllIn);
            _turn.Bet(_players[4], BetType.AllIn);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);

            _turn.NextRound();
        }

    }
}
