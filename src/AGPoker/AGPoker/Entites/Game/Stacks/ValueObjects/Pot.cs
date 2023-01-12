using AGPoker.Common.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Pot : ValueObject
    {
        // all in pot
        // should create new pot if necessary 
        private List<Bid> _bids = new();
        private Money _highestBid = Money.Create(0);

        private Pot() { }

        //save every bid
        // highest bid
        // player passed do not count to highest bid

        public void TakeABid(Bid bid)
        {
            var playerBids = TakePlayerBids(bid);
            var playerBidAmount = GetPlayerBidAmount(playerBids);
            BidValidation(playerBidAmount);
            SetHighestBidIfNeccessary(playerBidAmount);

            _bids.Add(bid);
        }

        public Money Value
            => Money.Create(_bids.Sum(b => b.Chips.Amount.Value));

        public static Pot Create()
            => new();

        private void BidValidation(int playerBidAmount)
        {
            if (playerBidAmount < _highestBid.Value)
                throw new ArgumentException();
        }

        private void SetHighestBidIfNeccessary(int playerBidAmount)
        {
            if (playerBidAmount > _highestBid.Value)
                _highestBid = Money.Create(playerBidAmount);
        }

        private int GetPlayerBidAmount(List<Bid> playerBids)
            => playerBids.Sum(b => b.Chips.Amount.Value);

        private List<Bid> TakePlayerBids(Bid currentBid)
        {
            var bids = _bids.Where(b => b.Player == currentBid.Player).ToList();
            bids.Add(currentBid);
            return bids;
        }
    }
}
