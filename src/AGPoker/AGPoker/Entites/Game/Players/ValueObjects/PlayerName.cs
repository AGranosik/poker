using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.Players.ValueObjects
{
    internal class PlayerName : Name
    {
        private PlayerName(string name) : base(name)
        {
        }

        internal static PlayerName Create(string name)
            => new(name);

    }
}
