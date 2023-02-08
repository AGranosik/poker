using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
using AGPoker.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    [TestFixture]
    internal class Turn_NextTurn_Tests
    {
        private Turn _turn;
        private Turn _secondTurn;
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
                Player.Create("hehe6", "hehe2"),

            };

            _turn = Turn.Start(_players);
            _secondTurn = Turn.Start(_players);
            GetIntoNextTurn(_secondTurn);
        }

        private void GetIntoNextTurn(Turn turn)
        {
            EveryPlayerCall(turn);
            EveryPlayerCall(turn);
            EveryPlayerCall(turn);
            EveryPlayerCall(turn, false);

            turn.NextTurn();
        }

        [Test]
        public void NextTurn_CannotStartWithoutFourRounds_ThrowsException()
        {
            var func = () => _turn.NextTurn();
            func.Should().Throw<CannotStartNextTurn>();
        }

        [Test]
        public void NextTurn_CannotStartWithoutFourRounds_ThrowsException2()
        {
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn);

            var func = () => _turn.NextTurn();
            func.Should().Throw<CannotStartNextTurn>();
        }

        [Test]
        public void NextTurn_CannotStartBefore4thRoundIsInGame_Sucess()
        {
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn);

            for (int i = 0; i < 4; i++)
                _turn.Bet(_players[i], BetType.Call);

            var func = () => _turn.NextTurn();
            func.Should().Throw<CannotStartNextTurn>();
        }

        [Test]
        public void NextTurn_DifferentTrio_Success()
        {
            var oldDealer = _turn.Dealer;
            var oldSmallBLind = _turn.SmallBlindPlayer;
            var oldBigBlind = _turn.BigBlindPlayer;
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn);
            EveryPlayerCall(_turn, false);

            var func = () => _turn.NextTurn();
            func.Should().NotThrow();

            var newDealer = _turn.Dealer;
            var newSmallBlind = _turn.SmallBlindPlayer;
            var newBigBlind = _turn.BigBlindPlayer;

            (newDealer == oldDealer).Should().BeFalse();
            (newSmallBlind == oldSmallBLind).Should().BeFalse();
            (newBigBlind == oldBigBlind).Should().BeFalse();
        }

        [Test]
        public void NextTurn_EveryPlayerCall_Success()
        {
            var secondTurnDealer = _secondTurn.Dealer;
            (secondTurnDealer == _players.ElementAt(_players.Count - 2)).Should().BeTrue();

            // player order not changed
            EveryPlayerCall(_secondTurn);
            EveryPlayerCall(_secondTurn);
            EveryPlayerCall(_secondTurn);
            EveryPlayerCall(_secondTurn, false);

            var func = () => _secondTurn.NextTurn();
            func.Should().NotThrow();

            var thirdRoundDealer = _secondTurn.Dealer;
            (thirdRoundDealer == secondTurnDealer).Should().BeFalse();
            (thirdRoundDealer == _players.ElementAt(_players.Count - 1)).Should().BeTrue();
        }

        [Test]
        public void NextTurn_TwoLastPlayerUntilNextTurn()
        {
            var secondTurnDealer = _secondTurn.Dealer;
            (secondTurnDealer == _players[_players.Count - 2]).Should().BeTrue();

            EveryPlayerCall(_secondTurn);
            EveryPlayerCall(_secondTurn);
            EveryPlayerCall(_secondTurn);
            ThreePlayersLastAfterBetting(_secondTurn);

            var func = () => _secondTurn.NextTurn();
            func.Should().NotThrow();

            var thirdRoundDealer = _secondTurn.Dealer;
            (thirdRoundDealer == secondTurnDealer).Should().BeFalse();
            (thirdRoundDealer == _players[4]).Should().BeTrue();
        }

        [Test]
        public void NextTurn_DealersChosenCorrectrly_Success()
        {
            var firstDealer = _players[2];
            var secondDealer = _players[3];
            var thirdDealer = _players[4];
            var fourthDealer = _players[0];
            var fifthDealer = _players[1];

            //3th turn
            GetIntoNextTurn(_secondTurn);
            (_secondTurn.Dealer == thirdDealer).Should().BeTrue();

            //4th round
            GetIntoNextTurn(_secondTurn);
            (_secondTurn.Dealer == fourthDealer).Should().BeTrue();

            GetIntoNextTurn(_secondTurn);
            (_secondTurn.Dealer == fifthDealer).Should().BeTrue();

            GetIntoNextTurn(_secondTurn);
            (_secondTurn.Dealer == firstDealer).Should().BeTrue();

            GetIntoNextTurn(_secondTurn);
            (_secondTurn.Dealer == secondDealer).Should().BeTrue();

        }

        private void ThreePlayersLastAfterBetting(Turn turn)
        {
            turn.Bet(_players[0], BetType.Call);
            turn.Bet(_players[1], BetType.Fold);
            turn.Bet(_players[2], BetType.Call);
            turn.Bet(_players[3], BetType.Raise);
            turn.Bet(_players[4], BetType.Raise);
            turn.Bet(_players[0], BetType.Raise);
            turn.Bet(_players[2], BetType.Call);
            turn.Bet(_players[3], BetType.Raise);
            turn.Bet(_players[4], BetType.Call);
            turn.Bet(_players[0], BetType.Call);
            turn.Bet(_players[2], BetType.Fold);
        }

        private void AllToTheNextOneWithoutOne(Turn turn)
        {
            for (int i = 0; i < _players.Count - 1; i++)
                turn.Bet(_players[i], BetType.Call);

            turn.Bet(_players[4], BetType.Fold);
            turn.NextRound();
        }

        private void EveryPlayerCall(Turn turn, bool startNextRound = true)
        {
            for (int i = 0; i < _players.Count; i++)
                turn.Bet(_players[i], BetType.Call);

            if(startNextRound)
                turn.NextRound();
        }
    }
}
