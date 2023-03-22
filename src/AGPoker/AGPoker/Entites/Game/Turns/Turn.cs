using AGPoker.Core;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Exceptions;

namespace AGPoker.Entites.Game.Turns
{
    public class Turn
    {
        private Turn(List<Player> players)
        {
            PlayersValidation(players);
            _players = players;
            SetDealerIndexAtStart();
            StartTurn();
        }

        public Player Dealer { get; private set; }
        public Player SmallBlindPlayer { get; private set; }
        public Player BigBlindPlayer { get; private set; }

        private List<Player> _players = new();
        private List<int> _playersInGame = new();
        private List<int> _playersToRemove = new();
        private int _currentPlayerIndex = 0;
        private int _dealerIndex = 0;
        private int _movesInTurn = 1;
        private int _maximumMovesInRound = 0;
        private int _roundNumber = 1;

        public static Turn Start(List<Player> players)
            => new(players);

        public void Bet(Player player, BetType bidType)
        {
            if (!CanBetBeMade() || !IsThisPlayerTurn(player))
                throw new CannotBetException("Next move cannot be performed.");

            SetTurnBet(bidType);
            SetNextPlayerIndex();
            RemovePlayerFromTurnIfNeccessary(player, bidType);
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

        public void NextTurn()
        {
            if (!CanStartNextTurn())
                throw new CannotStartNextTurn("Cannot start next turn.");

            StartTurn();
        }

        public bool CanStartNextRound()
            => !IsTheLastRound() && EarlierRoundFinished() && !IsTheLastOnePlayer();

        private void PlayersValidation(List<Player> players)
        {
            if (players is null || players.Count < 3)
                throw new ArgumentException(nameof(players));
        }

        private void StartTurn()
        {
            _roundNumber = 1;
            SetPlayersInGame();
            ResetTurnCounters();
            SetTrio();
            SetDealerIndex();
        }

        private void SetDealerIndex()
        {
            _dealerIndex = Circle.GetNextInCircle(_dealerIndex, _playersInGame);
        }

        private void SetDealerIndexAtStart()
        {
            var dealer = Circle.GetPrevious(_players.Last(), _players, 3);
            _dealerIndex = _players.IndexOf(dealer);
        }

        public bool CanStartNextTurn()
            => IsTheLastRound() && EarlierRoundFinished();

        private bool IsTheLastRound()
            => _roundNumber == 4;

        private bool EarlierRoundFinished()
            => _movesInTurn == _maximumMovesInRound || IsTheLastOnePlayer();

        private void SetTurnBet(BetType bidType)
        {
            if (bidType == BetType.Raise)
                _movesInTurn = 1;
            else if (bidType == BetType.Fold)
                return;
            else
                _movesInTurn++;
        }

        private void RemovePlayerFromTurnIfNeccessary(Player player,BetType bidType)
        {
            if (bidType == BetType.Fold)
            {
                _playersInGame.Remove(_players.IndexOf(player));
                _maximumMovesInRound = _playersInGame.Count;
            }
        }

        private bool CanBetBeMade()
            => _movesInTurn < _maximumMovesInRound && !IsTheLastOnePlayer();

        private bool IsTheLastOnePlayer()
            => _playersInGame.Count == _playersToRemove.Count+1;

        private void SetPlayersInGame()
        {
            _playersInGame = Enumerable.Range(0, _players.Count).ToList();
        }

        private void ResetTurnCounters()
        {
            _maximumMovesInRound = _playersInGame.Count;
            _movesInTurn = 0;
        }

        private void SetTrio()
        {
            SetDealer();
            SetSmallBlind();
            SetBigBlind();
        }

        private void SetDealer()
        {
            Dealer = _players[Circle.GetNextInCircle(_dealerIndex, _playersInGame)];
        }

        private void SetSmallBlind()
        {
            SmallBlindPlayer = _players[Circle.GetNextInCircle(_players.IndexOf(Dealer), _playersInGame)];
        }

        private void SetBigBlind()
        {
            BigBlindPlayer = _players[Circle.GetNextInCircle(_players.IndexOf(SmallBlindPlayer), _playersInGame)];
        }

        private void SetNextPlayerIndex()
        {
            _currentPlayerIndex = Circle.GetNextInCircle(_currentPlayerIndex, _playersInGame);
        }

    }
}
