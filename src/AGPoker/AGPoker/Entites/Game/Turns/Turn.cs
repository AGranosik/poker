﻿using AGPoker.Core;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Exceptions;

namespace AGPoker.Entites.Game.Turns
{
    public class Turn
    {
        //turn -> 4 rounds
        private Turn(List<Player> players)
        {
            PlayersValidation(players);
            DealerIndexInit(players);
            StartTurn(players);
        }

        public Player Dealer { get; private set; }
        public Player SmallBlindPlayer { get; private set; }
        public Player BigBlindPlayer { get; private set; }
        private int MaximumMovesInRound => _playersInGame.Count - _allInPlayers.Count;

        private List<Player> _players = new();
        private List<int> _playersInGame = new();
        private List<int> _playersToRemove = new();
        private List<int> _allInPlayers = new();
        private int _currentPlayerIndex = 0;
        private int _dealerIndex = 0;
        private int _movesInTurn = 0;
        private int _roundNumber = 1;

        public static Turn Start(List<Player> players)
            => new(players);


        public TurnResult Bet(Player player, BetType betType)
        {
            if (!CanBetBeMade() || !IsThisPlayerTurn(player))
                throw new CannotBetException("Next move cannot be performed.");

            SetNextPlayerInRound();
            TakePlayerBetIntoAccountForFutureMoves(player, betType);
            SetRoundBet(betType);

            return GetTurnStatus();
        }

        private bool IsThisPlayerTurn(Player player)
        {
            var playerIndex = _players.IndexOf(player);
            return playerIndex >= 0 && _playersInGame.Contains(playerIndex) && _currentPlayerIndex == playerIndex;
        }

        public void NextRound()
        {
            if (!CanStartNextRound())
                throw new CannotStartNextRound();

            ResetTurnCounters();
            _roundNumber++;
        }

        public void NextTurn(List<Player> players)
        {
            if (!CanStartNextTurn())
                throw new CannotStartNextTurn("Cannot start next turn.");

            StartTurn(players);
        }

        public bool CanStartNextRound()
            => !IsTheLastRound() && EveryoneMadeMove() && !IsLastPlayerWithoutAutoBets();

        private static void PlayersValidation(List<Player> players)
        {
            if (players is null || players.Count < 3)
                throw new ArgumentException(nameof(players));
        }

        private void StartTurn(List<Player> players)
        {
            _players = players;
            _roundNumber = 1;
            SetPlayersInTurnGame();
            SetTurnPlayers();
            ResetTurnCounters();
        }

        private void DealerIndexInit(List<Player> players)
        {
            var dealer = Circle.GetPrevious(players.Last(), players, 3);
            _dealerIndex = players.IndexOf(dealer);
        }

        public bool CanStartNextTurn()
            => (IsTheLastRound() && EveryoneMadeMove()) || IsLastPlayerWithoutAutoBets();

        public TurnResult GetTurnStatus()
        {
            if(IsLastPlayerWithoutAutoBets() || (EveryoneMadeMove() && IsTheLastRound()))
            {
                var winenrs = _players.Where(p => _playersInGame.Contains(_players.IndexOf(p))).ToList();
                return TurnResult.Winners(winenrs);
            }

            return TurnResult.InProgress();
        }

        private bool IsTheLastRound()
            => _roundNumber == 4;

        private bool EveryoneMadeMove()
            => _movesInTurn == MaximumMovesInRound;

        private void SetRoundBet(BetType bidType)
        {
            if (bidType == BetType.Raise)
                _movesInTurn = 1;
            else if (bidType == BetType.Fold)
                return;
            else if (bidType == BetType.AllIn)
                _movesInTurn = 0;
            else
                _movesInTurn++;
        }

        private void TakePlayerBetIntoAccountForFutureMoves(Player player,BetType betType)
        {
            if (betType == BetType.Fold)
            {
                _playersInGame.Remove(_players.IndexOf(player));
            }
            else if (betType == BetType.AllIn)
            {
                _allInPlayers.Add(_players.IndexOf(player));
            }
        }

        private bool CanBetBeMade()
            => _movesInTurn < MaximumMovesInRound && !IsLastPlayerWithoutAutoBets();

        // case when player doesn't have to decide because is the last one who has money in game
        // it means turn finished and cards combination should decide
        private bool IsLastPlayerWithoutAutoBets()
            => _playersInGame.Count == _playersToRemove.Count + 1 || _playersInGame.Count == _allInPlayers.Count + 1;

        private void SetPlayersInTurnGame()
        {
            _playersInGame = Enumerable.Range(0, _players.Count).ToList();
        }

        private void ResetTurnCounters()
        {
            _movesInTurn = 0;
            SetFirstPlayer();
        }

        private void SetTurnPlayers()
        {
            SetDealer();
            SetSmallBlind();
            SetBigBlind();
            SetFirstPlayer();
        }

        private void SetFirstPlayer()
        {
            _currentPlayerIndex = _players.IndexOf(Circle.GetPrevious(BigBlindPlayer, _players, 3));
            while (!_playersInGame.Contains(_currentPlayerIndex))
            {
                var player = _players[_currentPlayerIndex];
                _currentPlayerIndex = _players.IndexOf(Circle.GetNextInCircle(player, _players));
            }
        }

        private void SetDealer()
        {
            var nextDealerIndex = Circle.GetNextInCircle(_dealerIndex, _playersInGame);
            Dealer = _players[nextDealerIndex];
            _dealerIndex = nextDealerIndex;
        }

        private void SetSmallBlind()
        {
            SmallBlindPlayer = _players[Circle.GetNextInCircle(_players.IndexOf(Dealer), _playersInGame)];
        }

        private void SetBigBlind()
        {
            BigBlindPlayer = _players[Circle.GetNextInCircle(_players.IndexOf(SmallBlindPlayer), _playersInGame)];
        }

        private void SetNextPlayerInRound()
        {
            _currentPlayerIndex = Circle.GetNextInCircle(_currentPlayerIndex, _playersInGame);
            if (_allInPlayers.Contains(_currentPlayerIndex))
            {
                SetNextPlayerInRound();
            }
        }

    }
}
