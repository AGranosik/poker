using AGPoker.Common.ValueObjects;
using AGPoker.Core.Exceptions;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Pot : ValueObject
    {
        // all in pot
        // should create new pot if necessary 
        private List<Bet> _bids = new();
        private Money _highestBid = Money.Create(0);

        private Pot() { }

        //save every bid
        // highest bid
        // player passed do not count to highest bid

        public Money Value
            => Money.Create(_bids.Sum(b => b.Chips.Amount.Value));


        public void Fold(Bet bet)
        {
            if (!IsFoldBet(bet))
                throw new NotProperKindOfBetException();

            if (IsPlayerFoldedBefore(bet.Player))
                throw new PlayerFoldedBeforeException();

            _bids.Add(bet);
        }

        public void Raise(Bet bid)
        {
            var playerBids = TakePlayerBids(bid);
            var playerBidAmount = GetPlayerBidAmount(playerBids);
            RaiseValidation(playerBidAmount);
            SetHighestBidIfNeccessary(playerBidAmount);

            _bids.Add(bid);
        }

        public void Call(Player player)
        {
            var playerMoney = Money.Create(_bids.Where(b => b.Player == player)
                .Sum(b => b.Chips.Amount.Value));

            Bet bid;
            if(ShouldPlayerGiveChips(playerMoney))
            {
                var missingChips = _highestBid - playerMoney;
                bid = Bet.Create(Chips.Create(missingChips.Value), player, BidType.Call);
            }
            else
            {
                bid = Bet.Call(player);
            }
            _bids.Add(bid);
        }

        public static Pot Create()
            => new();


        private bool ShouldPlayerGiveChips(Money playerMoney)
        {
            return playerMoney < _highestBid;
        }

        private void RaiseValidation(int playerBidAmount)
        {
            if (playerBidAmount < _highestBid.Value)
                throw new ArgumentException();
        }

        private void SetHighestBidIfNeccessary(int playerBidAmount)
        {
            if (playerBidAmount > _highestBid.Value)
                _highestBid = Money.Create(playerBidAmount);
        }

        private int GetPlayerBidAmount(List<Bet> playerBids)
            => playerBids.Sum(b => b.Chips.Amount.Value);

        private List<Bet> TakePlayerBids(Bet currentBid)
        {
            var bids = _bids.Where(b => b.Player == currentBid.Player).ToList();
            bids.Add(currentBid);
            return bids;
        }

        private bool IsFoldBet(Bet bet)
            => bet.BidType == BidType.Fold && bet.Chips.Amount.Value == 0; //compare with value objects

        private bool IsPlayerFoldedBefore(Player player)
            => _bids.Any(b => BidType.Fold == b.BidType && b.Player == player);
    }
}
