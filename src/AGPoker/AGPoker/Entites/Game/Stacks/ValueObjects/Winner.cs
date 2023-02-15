using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Winner: ValueObject
    {
        private Winner(Player player, Money winningPrize)
        {
            CreationValidation(player, winningPrize);
            Player = player;
            WinningPrize = winningPrize;
        }

        public static Winner Create(Player player, Money winningPrize)
            => new(player, winningPrize);

        public Player Player { get; init; }
        public Money WinningPrize { get; init; }

        private void CreationValidation(Player player, Money winningPrize)
        {
            if(player is null)
                throw new ArgumentNullException(nameof(player));

            if (winningPrize is null)
                throw new ArgumentNullException(nameof(winningPrize));

            if (winningPrize.Value <= 0)
                throw new ArgumentException(nameof(winningPrize));
        }

    }
}
