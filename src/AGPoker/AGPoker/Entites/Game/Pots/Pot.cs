using AGPoker.Common;
using AGPoker.Entites.Game.Pots.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Pots
{
    public class Pot : Entity
    {
        private List<Bid> _bids = new();
        private Money _highestBid = Money.Create(0);

        private Pot() { }

        //save every bid
        // highest bid
        // player passed do not count to highest bid

        public void TakeABid(Bid bid)
        {
            BidValidation(bid);
            SetHighestBidIfNeccessary(bid);

            _bids.Add(bid);
        }

        public Money Value
            => Money.Create(_bids.Sum(b => b.Chips.Amount.Value));

        public static Pot Create()
            => new();

        private void BidValidation(Bid bid)
        {
            if(bid.Chips.Amount < _highestBid)
                throw new ArgumentException(nameof(bid));
        }

        private void SetHighestBidIfNeccessary(Bid bid)
        {
            if(bid.Chips.Amount > _highestBid)
                _highestBid = bid.Chips.Amount;
        }
    }
}
