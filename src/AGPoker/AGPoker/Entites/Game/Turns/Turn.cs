using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;

namespace AGPoker.Entites.Game.Turns
{
    public class Turn
    {
        private Turn(List<Player> players) //should be there any player for turn?
        {
            _players = players;
            StartTurn();
        }

        public Player Dealer { get; private set; } // first bet from player to the left so last in list
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

        public void Bet(Player player, BidType bidType) //
        {
            if (!CanMakeBid() || !IsThisPlayerTurn(player))
                throw new ArgumentException("Next move cannot be performed.");
            RemovePlayerFromTurnIfNeccessary(bidType);

            SetTurnMove(bidType);
            SetNextPlayerIndex();
        }

        private bool IsThisPlayerTurn(Player player)
        {
            var playerIndex = _players.IndexOf(player);
            return playerIndex >= 0 && _playersInGame.Contains(playerIndex) && _currentPlayerIndex == playerIndex;
        }

        public void NextRound()
        {
            if (IsTheLastRound())
                throw new ArgumentException("It was last round.");

            if (!EarlierRoundFinished())
                throw new ArgumentException("Earlier round doesnt finished.");

            if (IsTheLastOnePlayer())
                throw new ArgumentException("Not enough players.");

            SetTurnCounters();
            _roundNumber++;
        }

        public void NextTurn()
        {
            if (!CanStartNextTurn())
                throw new ArgumentException("Cannot start next turn.");

            StartTurn();
        }

        private void StartTurn()
        {
            _roundNumber = 1;
            SetPlayersInGame();
            SetTurnCounters();
            SetTrio();
        }

        private void SetDealerIndex()
        {
            if (_dealerIndex >= _playersInGame.Count - 1)
                _dealerIndex = 0;
            else
                _dealerIndex++;
        }

        private bool CanStartNextTurn()
            => IsTheLastRound() && EarlierRoundFinished();

        private bool IsTheLastRound()
            => _roundNumber == 4;

        private bool EarlierRoundFinished()
            => _movesInTurn == _maximumMovesInRound || IsTheLastOnePlayer();

        private void SetTurnMove(BidType bidType) // into another clas -> Circle
        {
            // should check how many players in game?
            // because right now someone will have no idea why its not counted as move.
            if (bidType == BidType.Raise)
                _movesInTurn = 1;
            else if (bidType == BidType.Fold)
                return;
            else
                _movesInTurn++;
        }

        private void RemovePlayerFromTurnIfNeccessary(BidType bidType)
        {
            if (bidType == BidType.Fold)
            {
                _playersInGame.Remove(_currentPlayerIndex);
                _maximumMovesInRound = _playersInGame.Count;
            }
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
            Dealer = _players[_players.Count -1];
        }

        private void SetSmallBlind()
        {
            SmallBlindPlayer = _players[_players.Count - 2];
        }

        private void SetBigBlind()
        {
            BigBlindPlayer = _players[_players.Count - 3];
        }

        private void SetNextPlayerIndex()
        {
            if(_currentPlayerIndex == _playersInGame.Count - 1)
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
