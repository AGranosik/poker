using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Exceptions;

namespace AGPoker.Entites.Game.Turns
{
    public class Turn
    {
        private Turn(List<Player> players) //should be there any player for turn?
        {
            _players = players;
            _dealerIndex = _players.Count - 4;
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
                throw new CannotBetException("Next move cannot be performed.");

            SetTurnMove(bidType);
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
            if (IsTheLastRound())
                throw new CannotStartNextRound("It was last round.");

            if (!EarlierRoundFinished())
                throw new CannotStartNextRound("Earlier round doesnt finished.");

            if (IsTheLastOnePlayer())
                throw new CannotStartNextRound("Not enough players.");

            SetTurnCounters();
            _roundNumber++;
        }

        public void NextTurn()
        {
            if (!CanStartNextTurn())
                throw new CannotStartNextTurn("Cannot start next turn.");

            StartTurn();
        }

        private void StartTurn()
        {
            _roundNumber = 1;
            SetPlayersInGame();
            SetTurnCounters();
            SetTrio();
            SetDealerIndex();
        }

        private void SetDealerIndex()
        {
            _dealerIndex = GetNextPlayerIndex(_dealerIndex);
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

        private void RemovePlayerFromTurnIfNeccessary(Player player,BidType bidType)
        {
            if (bidType == BidType.Fold)
            {
                _playersInGame.Remove(_players.IndexOf(player));
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
            Dealer = _players[GetNextPlayerIndex(_dealerIndex)];
        }

        private void SetSmallBlind()
        {
            SmallBlindPlayer = _players[GetNextPlayerIndex(_players.IndexOf(Dealer))];
        }

        private void SetBigBlind()
        {
            BigBlindPlayer = _players[GetNextPlayerIndex(_players.IndexOf(BigBlindPlayer))];
        }

        private void SetNextPlayerIndex()
        {
            var index = _playersInGame.IndexOf(_currentPlayerIndex);
            _currentPlayerIndex = GetNextPlayerIndex(index);
        }

        private int GetNextPlayerIndex(int index)
        {
            if (index == _playersInGame.Count - 1)
            {
                return _playersInGame[0];
            }
            else
            {
                return _playersInGame[index + 1];
            }
        }

    }
}
