using AGPoker.Common;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Aggregates
{
    public class Game : IAggregateRoot // just to mark as aggregate root
    {
        private Game(Player owner, GameLimit limit)
        {
            CreateValidation(owner, limit);
            Owner = owner;
            Limit = limit;
        }

        public static Game Create(Player owner, GameLimit limit)
            => new(owner, limit);

        private List<Player> _players = new();
        public Player Owner { get; init; }
        public GameLimit Limit { get; init; }
        public int NumberOfPlayer => _players.Count;
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

    }
}
