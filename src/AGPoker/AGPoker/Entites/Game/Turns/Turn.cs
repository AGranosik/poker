using AGPoker.Entites.Game.Game.Players;
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
            GetNextPlayerIndex();
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

        private void SetTurnMove(BidType bidType) // into another clas -> Circle
        {
            if (bidType == BidType.Raise)
                _movesInTurn = 1;
            else
                _movesInTurn++;
        }

        private void RemovePlayerFromTurnIfNeccessary(BidType bidType)
        {
            if (bidType == BidType.Fold)
                _playersInGame.Remove(_currentPlayerIndex);
        }

        private bool CanMakeBid()
            => _movesInTurn < _maximumMovesInRound && !IsTheLastOnePlayer();

        private bool IsTheLastOnePlayer()
            => _playersInGame.Count == 1;

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
            GetNextPlayerIndex();
        }

        private void SetDealer()
        {
            Dealer = _players[GetNextPlayerIndex()];
        }

        private void SetSmallBlind()
        {
            SmallBlindPlayer = _players[GetNextPlayerIndex()];
        }

        private void SetBigBlind()
        {
            BigBlindPlayer = _players[GetNextPlayerIndex()];
        }

        private int GetNextPlayerIndex()
        {
            if(_currentPlayerIndex >= _playersInGame.Count - 1)
            {
                _currentPlayerIndex = 0;
            }
            else
            {
                _currentPlayerIndex++;
            }

            return _currentPlayerIndex;
        }

    }
}
