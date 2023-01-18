using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Bid : ValueObject // refactor to larger folders
    {
        // check if its not all in
        // split into smaller ones.
        private Bid(Chips chips, Player player, BidType bidType) // set type of bid?
        {
            CreationValidation(chips, player);
            Chips = chips;
            Player = player;
            BidType = bidType;
        }

        public Chips Chips { get; init; }
        public Player Player { get; set; }
        public BidType BidType { get; init; }
        public bool AllIn
             => BidType == BidType.AllIn;


        public static Bid Create(Chips chips, Player player, BidType bidType = BidType.Equal)
            => new(chips, player, bidType);


        public static Bid Check(Player player)
            => new(Chips.Create(0), player, BidType.Check);

        private void CreationValidation(Chips chips, Player player)
        {
            if(chips is null)
                throw new ArgumentNullException(nameof(chips));

            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if(chips.Amount.Value < 0)
                throw new ArgumentException(nameof(chips.Amount));
        }
    }

    public enum BidType
    {
        Pass,
        Check,
        Equal,
        Higher,
        AllIn
    }
}
