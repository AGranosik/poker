using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Bid : ValueObject // refactor to larger folders
    {
        // check if its not all in
        // split into smaller ones.
        private Bid(Chips chips, Player player)
        {
            CreationValidation(chips, player);
            Chips = chips;
            Player = player;
        }

        public Chips Chips { get; init; }
        public Player Player { get; set; }


        public static Bid Create(Chips chips, Player player)
            => new(chips, player);

        private void CreationValidation(Chips chips, Player player)
        {
            if(chips is null)
                throw new ArgumentNullException(nameof(chips));

            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if(chips.Amount.Value <= 0)
                throw new ArgumentException(nameof(chips.Amount));
        }
    }
}
