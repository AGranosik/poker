using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Bet : ValueObject
    {
        private Bet(Money money, Player player, BetType betType, Bet splitedFrom = null)
        {
            CreationValidation(money, player);
            Money = money;
            Player = player;
            BetType = betType;
            SplitedFrom = splitedFrom;
        }

        public Bet SplitedFrom { get; init; }

        public Money Money { get; init; }
        public Player Player { get; set; }
        public BetType BetType { get; init; }



        public bool IsFolded()
            => BetType.Fold == BetType;
        public bool IsAllIn()
            => BetType.AllIn == BetType;

        public static Bet AllIn(Player player, Money money)
            => new(money, player, BetType.AllIn);

        public static Bet Raise(Money money, Player player)
        {
            RaiseValidation(money);
            return new(money, player, BetType.Raise);
        }

        public static Bet Call(Player player, Money money)
            => new(money, player, BetType.Call);

        public static Bet Fold(Player player)
            => new(Money.None, player, BetType.Fold);

        public Bet Split(Money moneyToTakeOut)
        {
            Money.Split(moneyToTakeOut);//shoud be a flag to leave some history
            var result = new Bet(moneyToTakeOut, Player, BetType, this);
            return result;
        }



        private static void CreationValidation(Money money, Player player)
        {
            if(money is null)
                throw new ArgumentNullException(nameof(money));

            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if(money.Value < 0)
                throw new ArgumentException(nameof(money.Value));
        }

        private static void RaiseValidation(Money money)
        {
            if (money is null || !money.Any)
                throw new ArgumentException();
        }
    }

    public enum BetType
    {
        Fold,
        Call,
        Raise,
        AllIn
    }
}
