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

            };
            _turn = Turn.Start(_players);
            _secondTurn = Turn.Start(_players);

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

        [Test]
        public void NextTurn_DifferentTrio_Success()
        {
            var oldDealer = _turn.Dealer;
            var oldSmallBLind = _turn.SmallBlindPlayer;
            var oldBigBlind = _turn.BigBlindPlayer;
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall();
            EveryPlayerCall(false);

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
        public void NextTurn_EveryPlayerrCall_Success()
        {

        }

        // get these methods into some helpers class
        private void TwoPlayersLastAfterBetting(Turn turn)
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

        private void AllToTheNextOneWithoutOne()
        {
            for (int i = 0; i < _players.Count - 1; i++)
                _turn.Bet(BidType.Call);

            _turn.Bet(BidType.Fold);
            _turn.NextRound();
        }

        private void EveryPlayerCall(bool startNextRound = true)
        {
            for (int i = 0; i < _players.Count; i++)
                _turn.Bet(BidType.Call);

            if(startNextRound)
                _turn.NextRound();
        }
    }
}
