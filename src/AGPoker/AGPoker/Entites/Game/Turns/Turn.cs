using AGPoker.Entites.Game.Game.Players;

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
        private int _currentPlayerIndex = 0;
        private int _dealerIndex = 0;

        public static Turn Start(List<Player> players)
            => new(players);

        public bool IsThisPlayerTurn(Player player)
        {
            var playerIndex = _players.IndexOf(player);
            return playerIndex >= 0 && _playersInGame.Contains(playerIndex);
        }

        private void SetPlayersInGame()
        {
            _playersInGame = Enumerable.Range(0, _players.Count).ToList();
        }

        private void SetTrio()
        {
            SetDealer();
            SetSmallBlind();
            SetBigBlind();
        }

        private void SetDealer()
        {
            Dealer = _players[_currentPlayerIndex++];
        }

        private void SetSmallBlind()
        {
            SmallBlindPlayer = _players[_currentPlayerIndex++];
        }

        private void SetBigBlind()
        {
            BigBlindPlayer = _players[_currentPlayerIndex++];
        }

    }
}
