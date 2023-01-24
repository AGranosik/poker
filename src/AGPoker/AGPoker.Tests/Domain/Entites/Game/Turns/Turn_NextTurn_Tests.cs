using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    [TestFixture]
    internal class Turn_NextTurn_Tests
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

            };
            _turn = Turn.Start(_players);
        }

        [Test]
        public void NextTurn_CannotStartWithoutFourRounds_ThrowsException()
        {
            var func = () => _turn.NextTurn();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextTurn_CannotStartWithoutFourRounds_ThrowsException2()
        {
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();

            var func = () => _turn.NextTurn();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextTurn_CannotStartBefore4thRoundIsInGame_Sucess()
        {
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();

            for (int i = 0; i < 3; i++)
                _turn.Bet(BidType.Call);

            var func = () => _turn.NextTurn();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextTurn_CanStartNextTurnIfTheLastPlayerInGame_Success()
        {
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();

            for (int i = 0; i < 3; i++)
                _turn.Bet(BidType.Fold);

            var func = () => _turn.NextTurn();
            func.Should().NotThrow<ArgumentException>();
        }


        private void EveryPlayerCall()
        {
            for (int i = 0; i < _players.Count; i++)
                _turn.Bet(BidType.Call);

            _turn.NextRound();
        }
    }
}
