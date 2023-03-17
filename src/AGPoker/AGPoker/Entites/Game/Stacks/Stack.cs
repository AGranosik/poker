using AGPoker.Common;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks
{
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
            => _pots.ForEach(p => p.Call(player));

        public IReadOnlyCollection<Pot> Pots
            => _pots.OrderBy(p => p.HighestBet.Value).ToList().AsReadOnly();

        public List<PotWinner> GetWinners()
            => _pots.Select(p => p.GetWinners()).ToList();

        public void AllIn(Bet bet)
        {
            var pots = Pots.OrderByDescending(p => p.IsAllIn())
                .ThenBy(p => p.HighestBet.Value).ToList();

            Pot lastPot;
            for(int i =0; i < pots.Count && bet.Money.Any; i++)
            {
                lastPot = pots[i];
                var isAllInPot = lastPot.IsAllIn();
                if (lastPot.CanTakeBetPart(bet))
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
                AllIn(bet);
            }
            else
            {
                Pot lastPot;
                var pots = Pots.OrderByDescending(p => p.IsAllIn())
                    .ThenBy(p => p.HighestBet.Value).ToList();
                for (int i = 0; i < _pots.Count && !bet.Money.Any; i++)
                {
                    lastPot= pots[i];

                    if (lastPot.IsAllIn() && lastPot.CanTakeBetPart(bet))
                    {
                        lastPot.TakePartOfAllInBet(bet);
                    }
                    else
                    {
                        lastPot.Raise(bet);
                        bet = Bet.Fold(bet.Player);
                    }
                    lastPot = pots[i];
                }
                if (bet.Money.Any)
                    _pots.Add(Pot.Create(new List<Bet> { bet }));
            }
        }

        public void Fold(Bet bet)
            => _pots.ForEach(p => p.Fold(bet));
    }
}
