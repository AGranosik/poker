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
            var notAllInPot = _pots.FirstOrDefault(p => !p.IsAllIn);
            if (notAllInPot is null)
                throw new ArgumentException("Pot doesnt exist.");

            var pots = Pots;

            for(int i =0; i < pots.Count && bet.Money.Any; i++)
            {
                var pot = pots.ElementAt(i);
                if (pot.CanTakeAllInBetPart(bet))
                {
                    pot.TakePartOfAllInBet(bet);
                }
                else
                {

                }
            }

            if(bet.Money.Any)
            {
                _pots.Add(Pot.Create(bet));
            }

            //_pots.First().AllIn(bet.Player);
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
    }
}
