﻿using AGPoker.Entites.Game.Game.Players;
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
            GetIntoNextTurn(_secondTurn, 0);
        }

        private void GetIntoNextTurn(Turn turn, int firstPlayerIndex)
        {
            TurnHelper.EveryPlayerCall(turn, firstPlayerIndex, _players);
            TurnHelper.EveryPlayerCall(turn, firstPlayerIndex, _players);
            TurnHelper.EveryPlayerCall(turn, firstPlayerIndex, _players);
            TurnHelper.EveryPlayerCall(turn, firstPlayerIndex, _players, false);

            turn.NextTurn(_players);
        }

        [Test]
        public void NextTurn_CannotStartWithoutFourRounds_ThrowsException()
        {
            var func = () => _turn.NextTurn(_players);
            func.Should().Throw<CannotStartNextTurn>();
        }

        [Test]
        public void NextTurn_CannotStartWithoutFourRounds_ThrowsException2()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players);

            var func = () => _turn.NextTurn(_players);
            func.Should().Throw<CannotStartNextTurn>();
        }

        [Test]
        public void NextTurn_CannotStartBefore4thRoundIsInGame_Sucess()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players);

            for (int i = 1; i < 5; i++)
                _turn.Bet(_players[i], BetType.Call);

            var func = () => _turn.NextTurn(_players);
            func.Should().Throw<CannotStartNextTurn>();
        }

        [Test]
        public void NextTurn_DifferentTrio_Success()
        {
            var oldDealer = _turn.Dealer;
            var oldSmallBLind = _turn.SmallBlindPlayer;
            var oldBigBlind = _turn.BigBlindPlayer;
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, false);

            var func = () => _turn.NextTurn(_players);
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
            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players);
            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players);
            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players);
            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players, false);

            var func = () => _secondTurn.NextTurn(_players);
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

            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players);
            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players);
            TurnHelper.EveryPlayerCall(_secondTurn, 1, _players);
            _secondTurn.Bet(_players[2], BetType.Call);
            _secondTurn.Bet(_players[3], BetType.Fold);
            _secondTurn.Bet(_players[4], BetType.Call);
            _secondTurn.Bet(_players[0], BetType.Raise);
            _secondTurn.Bet(_players[1], BetType.Raise);
            _secondTurn.Bet(_players[2], BetType.Raise);
            _secondTurn.Bet(_players[4], BetType.Call);
            _secondTurn.Bet(_players[0], BetType.Raise);
            _secondTurn.Bet(_players[1], BetType.Call);
            _secondTurn.Bet(_players[2], BetType.Call);
            _secondTurn.Bet(_players[4], BetType.Fold);

            var func = () => _secondTurn.NextTurn(_players);
            func.Should().NotThrow();

            var thirdRoundDealer = _secondTurn.Dealer;
            (thirdRoundDealer == secondTurnDealer).Should().BeFalse();
            (thirdRoundDealer == _players[4]).Should().BeTrue();
        }

        [Test]
        public void NextTurn_DealersChosenCorrectly_Success()
        {
            var firstDealer = _players[2];
            var secondDealer = _players[3];
            var thirdDealer = _players[4];
            var fourthDealer = _players[0];
            var fifthDealer = _players[1];

            //3th turn
            GetIntoNextTurn(_secondTurn, 1);
            (_secondTurn.Dealer == thirdDealer).Should().BeTrue();

            //4th round
            GetIntoNextTurn(_secondTurn, 2);
            (_secondTurn.Dealer == fourthDealer).Should().BeTrue();

            GetIntoNextTurn(_secondTurn, 3);
            (_secondTurn.Dealer == fifthDealer).Should().BeTrue();

            GetIntoNextTurn(_secondTurn, 4);
            (_secondTurn.Dealer == firstDealer).Should().BeTrue();

            GetIntoNextTurn(_secondTurn, 0);
            (_secondTurn.Dealer == secondDealer).Should().BeTrue();

        }

        [Test]
        public void NextTurn_DifferentFirstPlayer_Success()
        {
            var firstPlayer = _players[1];
            var secondPlayer = _players[2];

            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, false);
            _turn.NextTurn(_players);
            var func = () => _turn.Bet(firstPlayer, BetType.Call);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void NextTurn_DifferentFirstPlayer_Success2()
        {
            var firstPlayer = _players[1];
            var secondPlayer = _players[2];

            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, false);
            _turn.NextTurn(_players);

            var func = () => _turn.Bet(secondPlayer, BetType.Call);
            func.Should().NotThrow();
        }

        [Test]
        public void NextTurn_3rdRound_Success2()
        {
            var firstPlayer = _players[1];
            var secondPlayer = _players[2];
            var thirdPlayer = _players[3];

            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, false);
            _turn.NextTurn(_players);


            TurnHelper.EveryPlayerCall(_turn, 1, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 1, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 1, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 1, _players, false);
            _turn.NextTurn(_players);

            var exceptionFunc = () => _turn.Bet(secondPlayer, BetType.Call);
            exceptionFunc.Should().Throw<CannotBetException>();

            var func = () => _turn.Bet(thirdPlayer, BetType.Call);
            func.Should().NotThrow();
        }
    }
}
