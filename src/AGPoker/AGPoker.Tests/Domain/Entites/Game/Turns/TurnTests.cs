using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
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

            };
            _turn = Turn.Start(_players);
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
        public void Next_EveryoneChecked_Success()
        {
            var bidTypeChecked = BidType.Call;
            for(int i =0; i < _players.Count; i++)
                _turn.Bet(bidTypeChecked);
        }

        [Test]
        public void Next_EveryoneCheckedExtraTurn_ThrowsException()
        {
            var bidTypeChecked = BidType.Call;
            for (int i = 0; i < _players.Count; i++)
                _turn.Bet(bidTypeChecked);

            var func = () => _turn.Bet(bidTypeChecked);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_EveryonePassedExceptFirstPlayer_Success()
        {
            var bidTypePassed = BidType.Fold;
            for (int i = 0; i < _players.Count - 1; i++)
                _turn.Bet(bidTypePassed);
        }

        [Test]
        public void Next_LastPlayerCannotPass_ThrowsException()
        {
            var bidTypePassed = BidType.Fold;
            for (int i = 0; i < _players.Count - 1; i++)
                _turn.Bet(bidTypePassed);

            var func = () => _turn.Bet(bidTypePassed);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_OneOfThemMakeBiggerBidCircleNotClosedRestCalledUntilThen_Success()
        {
            var foldedBet = BidType.Call;
            for (int i = 0; i < 2; i++)
                _turn.Bet(foldedBet);

            _turn.Bet(BidType.Raise);

            for (int i = 0; i < _players.Count-1; i++)
                _turn.Bet(foldedBet);

            var func = () => _turn.Bet(foldedBet);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_HigherBidAfterAnotherThamCircleClosed_Success()
        {
            var checkedBid = BidType.Call;
            var higherBid = BidType.Raise;
            for (int i = 0; i < 2; i++)
                _turn.Bet(checkedBid);

            _turn.Bet(higherBid);
            _turn.Bet(checkedBid);
            _turn.Bet(higherBid);

            for (int i = 0; i < _players.Count-1; i++)
                _turn.Bet(checkedBid);

            var func = () => _turn.Bet(checkedBid);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_AllInDoesntChangeCircleFlow_Success()
        {
            var checkedBid = BidType.Call;
            var allIn = BidType.AllIn;

            for (int i = 0; i < 2; i++)
                _turn.Bet(checkedBid);

            _turn.Bet(allIn);
            _turn.Bet(checkedBid);

            var func = () => _turn.Bet(checkedBid); // to make sure circle is closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextRound_CannotStartWhenEarlierNotFinished_ThrwosException()
        {
            var func = () => _turn.NextRound();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextRound_CannotStartIfThereIsNotMoreThan1PlayerLeft_ThrowsException()
        {
            for (int i = 0; i < _players.Count-2; i++)
                _turn.Bet(BidType.Fold);

            _turn.Bet(BidType.Call);

            var func = () => _turn.NextRound();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextRound_EveryOneGetIntoNextRoundAndCanCall_ThrowsException()
        {
            EveryPlayerCall();

            EveryPlayerCall();

        }

        [Test]
        public void NextRound_OnlyPlayersInGameStayAtNextRound_Success()
        {
            _turn.Bet(BidType.Fold);
            for (int i = 0; i < _players.Count - 1; i++)
                _turn.Bet(BidType.Call);

            _turn.NextRound();

            for (int i = 0; i < 3; i++)
                _turn.Bet(BidType.Call);

            var func = () => _turn.Bet(BidType.Call);
            func.Should().Throw<ArgumentException>();

        }

        [Test]
        public void NextRound_SomeBettingBeforeSecondRoundClosure_Success()
        {
            AllToTheNextOneWithoutOne();

            _turn.Bet(BidType.Raise); // 1
            _turn.Bet(BidType.Call); // 2
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Fold); // 2
            _turn.Bet(BidType.Call); // 3

            var func = () => _turn.Bet(BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_OnePlayerFoldedAfterSomeRaisingBets_Success()
        {
            EveryPlayerCall();

            _turn.Bet(BidType.Call);// 1
            _turn.Bet(BidType.Raise); // 1
            _turn.Bet(BidType.Call); //2
            _turn.Bet(BidType.Call); //3
            _turn.Bet(BidType.Raise); // 1
            _turn.Bet(BidType.Raise); // 1
            _turn.Bet(BidType.Fold); // 2
            _turn.Bet(BidType.Raise); // 1
            _turn.Bet(BidType.Call); // 2-3
            _turn.Bet(BidType.Call); //4


            var func = () => _turn.Bet(BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_FirstPlayerFoldThenSomeRaises_Success()
        {
            EveryPlayerCall();

            _turn.Bet(BidType.Fold); //1
            _turn.Bet(BidType.Call); //2
            _turn.Bet(BidType.Call); //3
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Call); //2
            _turn.Bet(BidType.Call); //3


            var func = () => _turn.Bet(BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }


        [Test]
        public void Next_TwoPlayersFoldDuringSomeRaisesFlow_Success()
        {
            EveryPlayerCall();

            _turn.Bet(BidType.Call); //1
            _turn.Bet(BidType.Call); //2
            _turn.Bet(BidType.Call); //3
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Fold); //2
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Fold); //2
            _turn.Bet(BidType.Call); //3


            var func = () => _turn.Bet(BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_LastPlayerFold_Success()
        {
            EveryPlayerCall();

            TwoPlayersLastAfterBetting();
            var func = () => _turn.Bet(BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextRound_2ndRound_Sucess()
        {
            EveryPlayerCall();
            TwoPlayersLastAfterBetting();
            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_CannotStart3rdRoundWithoutFullCircle_Sucess()
        {
            EveryPlayerCall();

            TwoPlayersLastAfterBetting();
            _turn.NextRound();

            var func = () => _turn.NextRound();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NextRound_CanStart3rdRoundAfterFullCircle_Success()
        {
            EveryPlayerCall();
            EveryPlayerCall();
            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_CanStartFinalRound_Success()
        {
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();
            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().NotThrow();
        }

        [Test]
        public void NextRound_CannotStart5thRound_Success()
        {
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();

            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().Throw<ArgumentException>();
        }

        private void TwoPlayersLastAfterBetting()
        {
            _turn.Bet(BidType.Call); //1
            _turn.Bet(BidType.Fold); //2
            _turn.Bet(BidType.Call); //3
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Call); //2
            _turn.Bet(BidType.Raise); //1
            _turn.Bet(BidType.Call); //2
            _turn.Bet(BidType.Fold); //3
        }

        private void EveryPlayerCall()
        {
            for (int i = 0; i < _players.Count; i++)
                _turn.Bet(BidType.Call);

            _turn.NextRound();
        }

        private void AllToTheNextOneWithoutOne()
        {
            for (int i = 0; i < _players.Count-1; i++)
                _turn.Bet(BidType.Call);

            _turn.Bet(BidType.Fold);
            _turn.NextRound();
        }
    }
}
