using AGPoker.Common;
using AGPoker.Entites.Game.Players.ValueObjects;

namespace AGPoker.Entites.Game.Game.Players
{
    internal class Player: Entity
    {
        private Player(PlayerName playerName, PlayerSurname playerSurname)
        {
            PlayerName = playerName;
            PlayerSurname = playerSurname;
        }

        public PlayerName PlayerName { get; init; }
        public PlayerSurname PlayerSurname { get; init; }

        static Player Create(string playerName, string playerSurname)
            => new(PlayerName.Create(playerName), PlayerSurname.Create(playerSurname));
    }
}
