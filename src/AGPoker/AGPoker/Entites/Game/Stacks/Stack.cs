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
            => _pots.AsReadOnly();

        public List<PotWinner> GetWinners()
            => _pots.Select(p => p.GetWinners()).ToList();

        //public void AllIn(Player player)
        //{
        //    var bet = player
        //}

        public void Raise(Bet bet)
        {
            if (bet.IsAllIn())
            {

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
