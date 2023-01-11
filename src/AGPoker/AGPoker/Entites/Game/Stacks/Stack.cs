using AGPoker.Common;
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

        public Money Value
            => Money.Create(_pots.Sum(p => p.Value.Value));

        public void TakeABid(Bid bid)
            => _pots.First().TakeABid(bid);
    }
}
