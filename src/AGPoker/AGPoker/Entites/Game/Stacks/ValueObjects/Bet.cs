using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Bet : ValueObject // refactor to larger folders
    {
        // check if its not all in
        // split into smaller ones.
        private Bet(Chips chips, Player player, BidType bidType) // set type of bid?
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

        public static Bet Create(Chips chips, Player player, BidType bidType)
            => new(chips, player, bidType);

        public static Bet Call(Player player)
            => new(Chips.Create(0), player, BidType.Call);

        public static Bet Fold(Player player)
            => new(Chips.Create(0), player, BidType.Fold);

        private static void CreationValidation(Chips chips, Player player)
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
        Fold,
        Call,
        Raise,
        AllIn
    }
}
