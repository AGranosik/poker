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
        // tests + join player case

        private List<Player> _players = new();
        public Player Owner { get; init; }
        public GameLimit Limit { get; init; }

        private void CreateValidation(Player owner, GameLimit limit)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (limit is null)
                throw new ArgumentNullException(nameof(limit));
        }

    }
}
