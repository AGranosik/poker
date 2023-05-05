using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class PotWinner: ValueObject
    {
        private PotWinner(List<Player> players, Money winningPrize)
        {
            CreationValidation(players, winningPrize);
            _winners = players;
            WinningPrize = winningPrize;
        }

        public static PotWinner Create(List<Player> players, Money winningPrize)
            => new(players, winningPrize);

        private List<Player> _winners;
        public IReadOnlyCollection<Player> Winners
            => _winners.AsReadOnly();
        public Money WinningPrize { get; init; }

        private static void CreationValidation(List<Player> players, Money winningPrize)
        {
            if(players is null)
                throw new ArgumentNullException(nameof(players));

            if (players.Count == 0)
                throw new ArgumentException(nameof(players));

            if (winningPrize is null)
                throw new ArgumentNullException(nameof(winningPrize));

            if (winningPrize.Value <= 0)
                throw new ArgumentException(nameof(winningPrize));
        }

    }
}
