using AGPoker.Entites.Game.Game.Players;

namespace AGPoker.Entites.Game.Tables
{
    internal class Table
    {
        private readonly List<Player> _players;

        public Table(List<Player> players)
        {
            _players = players;
        }

        public static Table PreFlop(List<Player> players)
            => new(players);
    }
}
