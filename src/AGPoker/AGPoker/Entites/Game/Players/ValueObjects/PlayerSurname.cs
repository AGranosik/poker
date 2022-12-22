using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.Players.ValueObjects
{
    internal class PlayerSurname : Name
    {
        private PlayerSurname(string name) : base(name)
        {
        }

        internal static PlayerSurname Create(string name)
            => new(name);
    }
}
