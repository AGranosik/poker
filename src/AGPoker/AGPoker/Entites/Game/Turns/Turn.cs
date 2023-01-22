﻿using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;

namespace AGPoker.Entites.Game.Turns
{
    public class Turn
    {
        private Turn(List<Player> players) //should be there any player for turn?
        {
            _players = players;
            SetPlayersInGame();
            SetTurnCounters();
            SetTrio();
        }

        public Player Dealer { get; private set; }
        public Player SmallBlindPlayer { get; private set; }
        public Player BigBlindPlayer { get; private set; }

        private List<Player> _players = new();
        private List<int> _playersInGame = new();
        private List<int> _playersToRemove = new();
        private int _currentPlayerIndex = -1;
        private int _dealerIndex = 0;
        private int _movesInTurn = 1;
        private int _maximumMovesInRound = 0;

        public static Turn Start(List<Player> players)
            => new(players);

        public void Bet(BidType bidType)
        {
            if (!CanMakeBid())
                throw new ArgumentException("Next move cannot be performed.");
            RemovePlayerFromTurnIfNeccessary(bidType);

            SetTurnMove(bidType);
            SetNextPlayerIndex();
        }

        public bool IsThisPlayerTurn(Player player)
        {
            var playerIndex = _players.IndexOf(player);
            return playerIndex >= 0 && _playersInGame.Contains(playerIndex) && _currentPlayerIndex == playerIndex;
        }

        public void NextRound()
        {
            if (!EarlierRoundFinished())
                throw new ArgumentException("Earlier round doesnt finished.");

            if (IsTheLastOnePlayer())
                throw new ArgumentException("Not enough players.");

            SetTurnCounters();
        }

        private bool EarlierRoundFinished()
            => _movesInTurn == _maximumMovesInRound;

        private void SetTurnMove(BidType bidType, int ) // into another clas -> Circle
        {// skip some players when they folded oraz lower maximum moves in turn
            if (bidType == BidType.Raise)
                _movesInTurn = 1;
            else
                _movesInTurn++;
        }

        private void RemovePlayerFromTurnIfNeccessary(BidType bidType)
        {
            if (bidType == BidType.Fold)
                _playersToRemove.Add(_currentPlayerIndex);
        }

        private bool CanMakeBid()
            => _movesInTurn < _maximumMovesInRound && !IsTheLastOnePlayer();

        private bool IsTheLastOnePlayer()
            => _playersInGame.Count == _playersToRemove.Count+1;

        private void SetPlayersInGame()
        {
            _playersInGame = Enumerable.Range(0, _players.Count).ToList();
        }

        private void SetTurnCounters()
        {
            RemoveFoldedPlayersFromTurn();
            _maximumMovesInRound = _playersInGame.Count;
            _movesInTurn = 0;
        }

        private void RemoveFoldedPlayersFromTurn()
        {
            _playersInGame = Enumerable.Range(0, _players.Count).Where(p => !_playersToRemove.Contains(p)).ToList();
            _playersToRemove.Clear();
        }

        private void SetTrio()
        {
            SetDealer();
            SetSmallBlind();
            SetBigBlind();
            SetNextPlayerIndex();
        }

        private void SetDealer()
        {
            SetNextPlayerIndex();
            Dealer = _players[_currentPlayerIndex];
        }

        private void SetSmallBlind()
        {
            SetNextPlayerIndex();
            SmallBlindPlayer = _players[_currentPlayerIndex];
        }

        private void SetBigBlind()
        {
            SetNextPlayerIndex();
            BigBlindPlayer = _players[_currentPlayerIndex];
        }

        private void SetNextPlayerIndex()
        {
            if(_currentPlayerIndex >= _playersInGame.Count - 1)
            {
                _currentPlayerIndex = 0;
            }
            else
            {
                _currentPlayerIndex++;
            }
        }

    }
}