using AGPoker.Common;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Aggregates
{
    // minimal bid
    // minimal diff
    public class Game : IAggregateRoot // just to mark as aggregate root
    {
        private Game(Player owner, GameLimit limit)
        {
            CreateValidation(owner, limit);
            Owner = owner;
            Limit = limit;
        }

        public static Game Create(Player owner, GameLimit limit) // domain objects or not....
            => new(owner, limit);

        private List<Player> _players = new();
        public Player Owner { get; init; }
        public Player Dealer { get; private set; }
        public Player SmallBlindPlayer { get; private set; }
        public Player BigBlindPlayer { get; private set; }
        public GameLimit Limit { get; init; }

        private int _currentPlayerIndex = 0;
        public int NumberOfPlayer => _players.Count;

        public void Begin()
        {
            CanBegin();
            SetDealer();
            SetSmallBlindPlayer();
            SetBigBlindPlayer();
        }

        public void Join(Player player)
        {
            if(CanPlayerJoin(player))
                _players.Add(player);
        }

        private bool CanPlayerJoin(Player player)
        {
            if (NumberOfPlayer + 1 > Limit.Limit)
                throw new Exception("No more players can be added.");

            if (_players.Contains(player))
                throw new Exception("Player already in the game.");

            return true;
        }

        private void CreateValidation(Player owner, GameLimit limit)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (limit is null)
                throw new ArgumentNullException(nameof(limit));
        }

        private void SetDealer()
        {
            Dealer = GetNextPlayer();
        }

        private void SetSmallBlindPlayer()
        {
            SmallBlindPlayer= GetNextPlayer();
        }

        private void SetBigBlindPlayer()
        {
            BigBlindPlayer= GetNextPlayer();
        }
        private void CanBegin()
        {
            if (_players.Count <= 1) //use game limit
                throw new Exception("Not enough players.");
        }

        private Player GetNextPlayer()
        {
            if (_currentPlayerIndex == _players.Count)
                return _players[0];

            return _players[++_currentPlayerIndex];
        }
    }
}
