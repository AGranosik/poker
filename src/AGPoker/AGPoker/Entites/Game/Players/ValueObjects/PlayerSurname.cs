using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.Players.ValueObjects
{
    public class PlayerSurname : Name
    {
        private PlayerSurname(string name) : base(name)
        {
        }

        public static PlayerSurname Create(string name)
            => new(name);
    }
}
