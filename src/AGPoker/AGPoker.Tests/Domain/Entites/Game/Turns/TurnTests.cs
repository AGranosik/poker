using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
using AGPoker.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    [TestFixture]
    internal class TurnTests
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
        public void Creation_PlayersCannotBeNull_ThrowsException()
        {
            var func = () => Turn.Start(null);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_PlayersCannotBeEmpty_ThrowsException()
        {
            var func = () => Turn.Start(new List<Player>());
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_Minimum3Players_ThrwosException()
        {
            var func = () => Turn.Start(new List<Player>()
            {
                Player.Create("hehe", "sdasd")
            });
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Creation_WorksFor3Players_Success()
        {
            var players = new List<Player>
            {
                Player.Create("hehe2", "hehe2"),
                Player.Create("hehe3", "hehe2"),
                Player.Create("hehe4", "hehe2")
            };

            var func = () => Turn.Start(players);
            func.Should().NotThrow();
        }

        [Test]
        public void Start_SmallBlindPlayerSet_Success()
        {
            _turn.Dealer.Should().NotBeNull();
            _turn.SmallBlindPlayer.Should().NotBeNull();
        }

        [Test]
        public void Start_SmallBlindDifferentThanDealer_Success()
        {
            _turn.Dealer.Should().NotBeNull();
            _turn.SmallBlindPlayer.Should().NotBeNull();

            (_turn.Dealer == _turn.SmallBlindPlayer).Should().BeFalse();
        }

        [Test]
        public void Start_BigBlindPlayerSet_Success()
        {
            _turn.Dealer.Should().NotBeNull();
            _turn.SmallBlindPlayer.Should().NotBeNull();
            _turn.BigBlindPlayer.Should().NotBeNull();
        }

        [Test]
        public void Start_StartPlayersDifferFromEachOther_Success()
        {
            _turn.Dealer.Should().NotBeNull();
            _turn.SmallBlindPlayer.Should().NotBeNull();
            _turn.BigBlindPlayer.Should().NotBeNull();

            var dealerDifferentThanSmallBlind = _turn.Dealer != _turn.SmallBlindPlayer;
            var dealerDifferentThanBigBlind = _turn.Dealer != _turn.BigBlindPlayer;
            var smallBlindDifferThanBigBlind = _turn.SmallBlindPlayer != _turn.BigBlindPlayer;

            dealerDifferentThanBigBlind.Should().BeTrue();
            dealerDifferentThanSmallBlind.Should().BeTrue();
            smallBlindDifferThanBigBlind.Should().BeTrue();
        }

        [Test]
        public void Start_PlayerAfterBigBlindStarts_success()
        {
            var nextPlayer = _players[1];
            var expectionFunc = () => _turn.Bet(_players[0], BetType.Call);
            expectionFunc.Should().Throw<CannotBetException>();

            var rightBet = () => _turn.Bet(nextPlayer, BetType.Call);
            rightBet.Should().NotThrow();
        }

        [Test]
        public void Next_EveryoneCalled_Success()
        {
            var func = () => TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            func.Should().NotThrow();
        }

        [Test]
        public void Next_EveryoneCalledExtraBet_ThrowsException()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, false);

            var func = () => _turn.Bet(_players[1], BetType.Call);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void Next_EveryoneFoldedExceptLastPlayer_Success()
        {
            var betTypeFolded = BetType.Fold;
            for (int i = 1; i < _players.Count; i++)
                _turn.Bet(_players[i], betTypeFolded);
        }

        [Test]
        public void Next_LastPlayerCannotPass_ThrowsException()
        {
            var betTypePassed = BetType.Fold;
            for (int i = 1; i < _players.Count; i++)
                _turn.Bet(_players[i], betTypePassed);

            var func = () => _turn.Bet(_players[0], betTypePassed);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void Next_OneOfThemMakeBiggerBedCircleNotClosedRestCalledUntilThen_Success()
        {
            var foldedBet = BetType.Call;
            for (int i = 1; i < 3; i++)
                _turn.Bet(_players[i], foldedBet);

            _turn.Bet(_players[3], BetType.Raise);

            _turn.Bet(_players[4], foldedBet);
            _turn.Bet(_players[0], foldedBet);
            _turn.Bet(_players[1], foldedBet);
            _turn.Bet(_players[2], foldedBet);

            var func = () => _turn.Bet(_players[3], foldedBet);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void Next_HigherBidAfterAnotherThanCircleClosed_Success()
        {
            var checkedBid = BetType.Call;
            var higherBid = BetType.Raise;
            for (int i = 1; i < 3; i++)
                _turn.Bet(_players[i], checkedBid);

            _turn.Bet(_players[3], higherBid);
            _turn.Bet(_players[4], checkedBid);
            _turn.Bet(_players[0], higherBid);

            for (int i = 1; i < _players.Count; i++)
                _turn.Bet(_players[i], checkedBid);

            var func = () => _turn.Bet(_players[0], checkedBid);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void NextRound_CannotStartWhenEarlierNotFinished_ThrwosException()
        {
            var func = () => _turn.NextRound();
            func.Should().Throw<CannotStartNextRound>();
        }

        [Test]
        public void NextRound_CannotStartIfThereIsNotMoreThan1PlayerLeft_ThrowsException()
        {
            for (int i = 1; i < _players.Count - 1; i++)
                _turn.Bet(_players[i], BetType.Fold);

            _turn.Bet(_players[4], BetType.Call);

            var func = () => _turn.NextRound();
            func.Should().Throw<CannotStartNextRound>();
        }

        [Test]
        public void NextRound_EveryOneGetIntoNextRoundAndCanCall_ThrowsException()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            var func = () => TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_OnlyPlayersInGameStayAtNextRound_Success()
        {
            _turn.Bet(_players[1], BetType.Fold);
            for (int i = 2; i < _players.Count; i++)
                _turn.Bet(_players[i], BetType.Call);

            _turn.Bet(_players[0], BetType.Call);

            _turn.NextRound();

            for (int i = 2; i < _players.Count; i++)
                _turn.Bet(_players[i], BetType.Call);

            _turn.Bet(_players[0], BetType.Call);

            var func = () => _turn.Bet(_players[1], BetType.Call);
            func.Should().Throw<CannotBetException>();

        }

        [Test]
        public void NextRound_SomeBettingBeforeSecondRoundClosure_Success()
        {
            AllToTheNextOneWithoutOne();

            _turn.Bet(_players[2], BetType.Raise);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Fold);
            _turn.Bet(_players[0], BetType.Raise);
            _turn.Bet(_players[2], BetType.Call); 
            _turn.Bet(_players[3], BetType.Call);

            var func = () => _turn.Bet(_players[4], BetType.Call); // circle should be closed
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void Next_OnePlayerFoldedAfterSomeRaisingBets_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Raise);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Call);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Raise);
            _turn.Bet(_players[2], BetType.Raise);
            _turn.Bet(_players[3], BetType.Fold);
            _turn.Bet(_players[4], BetType.Raise);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);


            var func = () => _turn.Bet(_players[3], BetType.Call); // circle should be closed
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void Next_FirstPlayerFoldThenSomeRaises_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            _turn.Bet(_players[1], BetType.Fold);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Raise);
            _turn.Bet(_players[0], BetType.Raise);
            _turn.Bet(_players[2], BetType.Raise);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Call);
            _turn.Bet(_players[0], BetType.Call);


            var func = () => _turn.Bet(_players[2], BetType.Call); // circle should be closed
            func.Should().Throw<CannotBetException>();
        }


        [Test]
        public void Next_TwoPlayersFoldDuringSomeRaisesFlow_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Call);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Raise);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Raise);
            _turn.Bet(_players[2], BetType.Fold);
            _turn.Bet(_players[3], BetType.Raise);
            _turn.Bet(_players[4], BetType.Fold);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Call);


            var func = () => _turn.Bet(_players[2], BetType.Call); // circle should be closed
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void Next_LastPlayerFold_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            TwoPlayersLastAfterBetting();
            var func = () => _turn.Bet(_players[1], BetType.Call); // circle should be closed
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void NextRound_2ndRound_Sucess()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            TwoPlayersLastAfterBetting();
            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_CannotStart3rdRoundWithoutFullCircle_Sucess()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            TwoPlayersLastAfterBetting();
            _turn.NextRound();

            var func = () => _turn.NextRound();
            func.Should().Throw<CannotStartNextRound>();
        }

        [Test]
        public void NextRound_CanStart3rdRoundAfterFullCircle_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_CanStartFinalRound_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_CannotStart5thRound_Success()
        {
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);
            TurnHelper.EveryPlayerCall(_turn, 0, _players, true);

            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().Throw<CannotStartNextRound>();
        }

        private void TwoPlayersLastAfterBetting()
        {
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[2], BetType.Fold);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Raise);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Raise);
            _turn.Bet(_players[3], BetType.Call);
            _turn.Bet(_players[4], BetType.Raise);
            _turn.Bet(_players[0], BetType.Call);
            _turn.Bet(_players[1], BetType.Call);
            _turn.Bet(_players[3], BetType.Fold);
        }

        private void AllToTheNextOneWithoutOne()
        {
            _turn.Bet(_players[1], BetType.Fold);
            for (int i = 2; i < _players.Count; i++)
                _turn.Bet(_players[i], BetType.Call);

            _turn.Bet(_players[0], BetType.Call);
            _turn.NextRound();
        }
    }
}
