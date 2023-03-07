using AGPoker.Common;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks
{
    //remove from winners
    public class Stack : Entity
    {
        private List<Pot> _pots = new();
        private Stack()
        {
            _pots.Add(Pot.Create());
        }

        public static Stack Create()
            => new();

        public Money Worth
            => Money.Create(_pots.Sum(p => p.Value.Value));

        public void Call(Player player)
            => _pots.First().Call(player);

        public IReadOnlyCollection<Pot> Pots
            => _pots.OrderBy(p => p.HighestBet.Value).ToList().AsReadOnly();

        public List<PotWinner> GetWinners()
            => _pots.Select(p => p.GetWinners()).ToList();

        public void AllIn(Player player)
        {
            var bet = player.AllIn();
            var pots = Pots.OrderByDescending(p => p.IsAllIn)
                .ThenBy(p => p.HighestBet.Value).ToList();

            Pot lastPot;
            for(int i =0; i < pots.Count && bet.Money.Any; i++)
            {
                lastPot = pots[i];
                var isAllInPot = lastPot.IsAllIn;
                if (isAllInPot && CanTakePartOfBet(bet, lastPot))
                {
                    lastPot.TakePartOfAllInBet(bet);
                }
                else
                {
                    var bets = lastPot.AllIn(bet);
                    if (bets.Any())
                    {
                        _pots.Add(Pot.Create(bets));
                    }
                    return;
                }
            }
            if (bet.Money.Any)
                _pots.Add(Pot.Create(new List<Bet> { bet }));
        }

        public void Raise(Bet bet)
        {
            if (bet.IsAllIn())
            {
                //_pots.First().AllIn(bet.Player);
            }
            else
            {
                _pots.First().Raise(bet);
            }
        }

        public void Fold(Bet bet)
            => _pots.First().Fold(bet);

        private bool CanTakePartOfBet(Bet bet, Pot pot)
            =>  pot.CanTakeAllInBetPart(bet);
    }
}
