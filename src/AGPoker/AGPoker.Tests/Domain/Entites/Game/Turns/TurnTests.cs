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
        private List<Player> _playerInOrder;

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

            _playerInOrder = new List<Player>
            {
                _players[4],
                _players[0],
                _players[1],
                _players[2],
                _players[3]
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
        public void Next_EveryoneCalled_Success()
        {
            var bidTypeChecked = BidType.Call;
            for(int i =0; i < _players.Count; i++)
                _turn.Bet(_players[i], bidTypeChecked);
        }

        [Test]
        public void Next_EveryoneCalledExtraTurn_ThrowsException()
        {
            var bidTypeChecked = BidType.Call;
            for (int i = 0; i < _players.Count; i++)
                _turn.Bet(_players[i], bidTypeChecked);

            var func = () => _turn.Bet(_players[0], bidTypeChecked);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_EveryoneFoldedExceptLastPlayer_Success()
        {
            var bidTypeFolded = BidType.Fold;
            for (int i = 0; i < _players.Count - 1; i++)
                _turn.Bet(_players[i], bidTypeFolded);
        }

        [Test]
        public void Next_LastPlayerCannotPass_ThrowsException()
        {
            var bidTypePassed = BidType.Fold;
            for (int i = 0; i < _players.Count - 1; i++)
                _turn.Bet(_players[i], bidTypePassed);

            var func = () => _turn.Bet(_playerInOrder[3], bidTypePassed);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_OneOfThemMakeBiggerBedCircleNotClosedRestCalledUntilThen_Success()
        {
            var foldedBet = BidType.Call;
            for (int i = 0; i < 2; i++)
                _turn.Bet(_players[i], foldedBet);

            _turn.Bet(_players[2], BidType.Raise);

            _turn.Bet(_players[3], foldedBet);
            _turn.Bet(_players[4], foldedBet);
            _turn.Bet(_players[0], foldedBet);
            _turn.Bet(_players[1], foldedBet);

            var func = () => _turn.Bet(_players[2], foldedBet);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_HigherBidAfterAnotherThanCircleClosed_Success()
        {
            var checkedBid = BidType.Call;
            var higherBid = BidType.Raise;
            for (int i = 0; i < 2; i++)
                _turn.Bet(_players[i], checkedBid);

            _turn.Bet(_players[2], higherBid);
            _turn.Bet(_players[3], checkedBid);
            _turn.Bet(_players[4], higherBid);

            for (int i = 0; i < _players.Count-1; i++)
                _turn.Bet(_players[i], checkedBid);

            var func = () => _turn.Bet(_players[4], checkedBid);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_AllInDoesntChangeCircleFlow_Success()
        {
            var checkedBid = BidType.Call;
            var allIn = BidType.AllIn;

            for (int i = 0; i < 3; i++)
                _turn.Bet(_players[i], checkedBid);

            _turn.Bet(_players[3], allIn);
            _turn.Bet(_players[4], checkedBid);

            var func = () => _turn.Bet(_players[0], checkedBid); // to make sure circle is closed
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
                _turn.Bet(_players[i], BidType.Fold);

            _turn.Bet(_players[3], BidType.Call);

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
            _turn.Bet(_players[0], BidType.Fold);
            for (int i = 1; i < _players.Count - 1; i++)
                _turn.Bet(_players[i], BidType.Call);

            _turn.NextRound();

            for (int i = 0; i < 4; i++)
                _turn.Bet(_players[i+1], BidType.Call);

            var func = () => _turn.Bet(_playerInOrder[1], BidType.Call);
            func.Should().Throw<ArgumentException>();

        }

        [Test]
        public void NextRound_SomeBettingBeforeSecondRoundClosure_Success()
        {
            AllToTheNextOneWithoutOne();

            _turn.Bet(_playerInOrder[0], BidType.Raise); // 1
            _turn.Bet(_playerInOrder[1], BidType.Call); // 2
            _turn.Bet(_playerInOrder[2], BidType.Raise); //1
            _turn.Bet(_playerInOrder[3], BidType.Fold); // 2
            _turn.Bet(_playerInOrder[0], BidType.Call); // 3

            var func = () => _turn.Bet(_playerInOrder[1], BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_OnePlayerFoldedAfterSomeRaisingBets_Success()
        {
            EveryPlayerCall();

            _turn.Bet(_playerInOrder[0], BidType.Call);// 1
            _turn.Bet(_playerInOrder[1], BidType.Raise); // 1
            _turn.Bet(_playerInOrder[2], BidType.Call); //2
            _turn.Bet(_playerInOrder[3], BidType.Call); //3
            _turn.Bet(_playerInOrder[0], BidType.Raise); // 1
            _turn.Bet(_playerInOrder[1], BidType.Raise); // 1
            _turn.Bet(_playerInOrder[2], BidType.Fold); // 2
            _turn.Bet(_playerInOrder[3], BidType.Raise); // 1
            _turn.Bet(_playerInOrder[0], BidType.Call); // 2-3
            _turn.Bet(_playerInOrder[1], BidType.Call); //4


            var func = () => _turn.Bet(_playerInOrder[3], BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_FirstPlayerFoldThenSomeRaises_Success()
        {
            EveryPlayerCall();

            _turn.Bet(_playerInOrder[0], BidType.Fold); //1
            _turn.Bet(_playerInOrder[1], BidType.Call); //2
            _turn.Bet(_playerInOrder[2], BidType.Call); //3
            _turn.Bet(_playerInOrder[3], BidType.Raise); //1
            _turn.Bet(_playerInOrder[0], BidType.Raise); //1
            _turn.Bet(_playerInOrder[1], BidType.Raise); //1
            _turn.Bet(_playerInOrder[2], BidType.Call); //2
            _turn.Bet(_playerInOrder[3], BidType.Call); //3


            var func = () => _turn.Bet(_playerInOrder[0], BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }


        [Test]
        public void Next_TwoPlayersFoldDuringSomeRaisesFlow_Success()
        {
            EveryPlayerCall();

            _turn.Bet(_playerInOrder[0], BidType.Call); //1
            _turn.Bet(_playerInOrder[1], BidType.Call); //2
            _turn.Bet(_playerInOrder[2], BidType.Call); //3
            _turn.Bet(_playerInOrder[3], BidType.Raise); //1
            _turn.Bet(_playerInOrder[0], BidType.Raise); //1
            _turn.Bet(_playerInOrder[1], BidType.Fold); //2
            _turn.Bet(_playerInOrder[2], BidType.Raise); //1
            _turn.Bet(_playerInOrder[3], BidType.Fold); //2
            _turn.Bet(_playerInOrder[0], BidType.Call); //3


            var func = () => _turn.Bet(_playerInOrder[2], BidType.Call); // circle should be closed
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Next_LastPlayerFold_Success()
        {
            EveryPlayerCall();

            TwoPlayersLastAfterBetting();
            var func = () => _turn.Bet(_playerInOrder[0], BidType.Call); // circle should be closed
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

            TwoPlayersLastAfterBetting();

            var func = () => _turn.NextRound();
            func.Should().Throw<ArgumentException>();
        }

        private void TwoPlayersLastAfterBetting()
        {
            _turn.Bet(_playerInOrder[0], BidType.Call); //1
            _turn.Bet(_playerInOrder[1], BidType.Fold); //2
            _turn.Bet(_playerInOrder[2], BidType.Call); //3
            _turn.Bet(_playerInOrder[3], BidType.Raise); //1
            _turn.Bet(_playerInOrder[0], BidType.Raise); //1
            _turn.Bet(_playerInOrder[2], BidType.Call); //2
            _turn.Bet(_playerInOrder[3], BidType.Raise); //1
            _turn.Bet(_playerInOrder[0], BidType.Call); //2
            _turn.Bet(_playerInOrder[2], BidType.Fold); //3
        }

        private void EveryPlayerCall()
        {
            for (int i = 0; i < _players.Count; i++)
                _turn.Bet(_players[i], BidType.Call);

            _turn.NextRound();
        }

        private void AllToTheNextOneWithoutOne()
        {
            for (int i = 0; i < _players.Count-1; i++)
                _turn.Bet(_playerInOrder[i], BidType.Call);

            _turn.Bet(_playerInOrder[3], BidType.Fold);
            _turn.NextRound();
        }
    }
}
