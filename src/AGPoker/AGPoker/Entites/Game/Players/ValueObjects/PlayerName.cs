using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.Players.ValueObjects
{
    public class PlayerName : Name
    {
        private PlayerName(string name) : base(name)
        {
        }

        public static PlayerName Create(string name)
            => new(name);

    }
}
