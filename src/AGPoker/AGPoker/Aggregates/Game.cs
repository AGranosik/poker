using AGPoker.Common;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Aggregates
{
    internal class Game : IAggregateRoot // just to mark as aggregate root
    {
        private Game(Player owner, GameLimit limit)
        {
            Owner = owner;
            Limit = limit;
        }

        static Game Create(Player owner, GameLimit limit)
            => new(owner, limit);


        private List<Player> _players = new();
        public Player Owner { get; init; }
        public GameLimit Limit { get; init; }

    }
}
