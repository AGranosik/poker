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
        private List<Bet> _bets = new();
        private Money _highestBid = Money.Create(0);

        private Pot() { }

        public Money Value
            => Money.Create(_bets.Sum(b => b.Money.Value));

        public bool IsAllIn
            => _bets.Any(b => b.IsAllIn());

        public List<Player> Winners
            => _bets
            .Where(b => b.Money == _highestBid)
            .Select(b => b.Player).ToList();

        public void Fold(Bet bet)
        {
            if (!IsFoldBet(bet))
                throw new NotProperKindOfBetException();

            if (IsPlayerFoldedBefore(bet.Player))
                throw new PlayerFoldedBeforeException();

            _bets.Add(bet);
        }

        public void Raise(Bet bet) // player and Money
        {
            var playerBids = TakePlayerBids(bet);
            var playerBidAmount = GetPlayerBetAmount(playerBids);
            RaiseValidation(playerBidAmount, bet.Player);
            SetHighestBidIfNeccessary(playerBidAmount);

            _bets.Add(bet);
        }

        public void Call(Player player)
        {
            var playerMoney = Money.Create(_bets.Where(b => b.Player == player)
                .Sum(b => b.Money.Value));

            Bet bet;
            if (ShouldPlayerGiveChips(playerMoney))
            {
                var missingChips = _highestBid - playerMoney;
                bet = player.Call(missingChips);
            }
            else
            {
                bet = player.Call();
            }
            _bets.Add(bet);
        }

        public static Pot Create()
            => new();

        private bool ShouldPlayerGiveChips(Money playerMoney)
        {
            return playerMoney < _highestBid;
        }

        private void RaiseValidation(int playerBidAmount, Player player)
        {
            if (playerBidAmount < _highestBid.Value)
                throw new BetTooLowException();

            if (IsPlayerFoldedBefore(player))
                throw new PlayerFoldedBeforeException();
        }

        private void SetHighestBidIfNeccessary(int playerBidAmount)
        {
            if (playerBidAmount > _highestBid.Value)
                _highestBid = Money.Create(playerBidAmount);
        }

        private int GetPlayerBetAmount(List<Bet> playerBids)
            => playerBids.Sum(b => b.Money.Value);

        private List<Bet> TakePlayerBids(Bet currentBid)
        {
            var bids = _bets.Where(b => b.Player == currentBid.Player).ToList();
            bids.Add(currentBid);
            return bids;
        }

        private bool IsFoldBet(Bet bet)
            => bet.BetType == BetType.Fold && bet.Money == Money.None;

        private bool IsPlayerFoldedBefore(Player player)
            => _bets.Any(b => BetType.Fold == b.BetType && b.Player == player);
    }
}
