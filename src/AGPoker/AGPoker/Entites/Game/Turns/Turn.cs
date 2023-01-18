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
            SetTrio();
        }

        public Player Dealer { get; private set; }
        public Player SmallBlindPlayer { get; private set; }
        public Player BigBlindPlayer { get; private set; }

        private List<Player> _players = new();
        private List<int> _playersInGame = new();
        private int _currentPlayerIndex = -1;
        private int _dealerIndex = 0;
        private int _movesInTurn = 0;

        public static Turn Start(List<Player> players)
            => new(players);

        public void Next(BidType bidType)
        {
            if (!CanMakeBid())
                throw new ArgumentException("Next move cannot be performed.");
            RemovePlayerFromTurnIfNeccessary(bidType);
            GetNextPlayerIndex();
            _movesInTurn++;
        }

        public bool IsThisPlayerTurn(Player player)
        {
            var playerIndex = _players.IndexOf(player);
            return playerIndex >= 0 && _playersInGame.Contains(playerIndex) && _currentPlayerIndex == playerIndex;
        }

        private void RemovePlayerFromTurnIfNeccessary(BidType bidType)
        {
            if (bidType == BidType.Pass)
                _playersInGame.Remove(_currentPlayerIndex);
        }

        private bool CanMakeBid()
            => _movesInTurn < _players.Count && !IsTheLastOnePlayer();

        private bool IsTheLastOnePlayer()
            => _playersInGame.Count == 1;

        private void SetPlayersInGame()
        {
            _playersInGame = Enumerable.Range(0, _players.Count).ToList();
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
