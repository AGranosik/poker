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
        private Money _highestBet = Money.Create(0);

        private Pot() { }

        public Money Value
            => Money.Create(_bets.Sum(b => b.Money.Value));

        public bool IsAllIn
            => _bets.Any(b => b.IsAllIn());

        public PotWinner GetWinners()
        {
            var groppedBets = _bets.GroupBy(b => b.Player)
                .Where(gp => gp.Any(b => b.BetType != BetType.Fold))
                .Select(gp => new
                {
                    Player = gp.Key,
                    SumOfBets = Money.Create(gp.Sum(m => m.Money.Value))
                })
                .ToList();

            var winners = groppedBets.Where(b => b.SumOfBets == _highestBet).ToList();
            var winPrize = PrizePerWinner(winners.Count);

            return PotWinner.Create(winners.Select(w => w.Player).ToList(), winPrize);
        }

        public void Fold(Bet bet)
        {
            if (!IsFoldBet(bet))
                throw new NotProperKindOfBetException();

            if (IsPlayerFoldedBefore(bet.Player))
                throw new PlayerFoldedBeforeException();

            _bets.Add(bet);
        }

        // raise works as 'add my money to pot not like 'i want the highest be to be like: amount' 
        public void Raise(Bet bet) // player and Money
        {
            var playerBets = TakePlayerBets(bet);
            var playerBidAmount = GetPlayerBetAmount(playerBets);
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
                var missingChips = _highestBet - playerMoney;
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

        private Money PrizePerWinner(int numberOfWinners)
        {
            if (numberOfWinners == 0)
                return Money.None;

            var moneyInPot = _bets.Sum(b => b.Money.Value);

            return Money.Create((moneyInPot / numberOfWinners)); // dont give a f*ck about remainder.
        }

        private bool ShouldPlayerGiveChips(Money playerMoney)
        {
            return playerMoney < _highestBet;
        }

        private void RaiseValidation(int playerBidAmount, Player player)
        {
            if (playerBidAmount < _highestBet.Value)
                throw new BetTooLowException();

            if (IsPlayerFoldedBefore(player))
                throw new PlayerFoldedBeforeException();
        }

        private void SetHighestBidIfNeccessary(int playerBidAmount)
        {
            if (playerBidAmount > _highestBet.Value)
                _highestBet = Money.Create(playerBidAmount);
        }

        private int GetPlayerBetAmount(List<Bet> playerBids)
            => playerBids.Sum(b => b.Money.Value);

        private List<Bet> TakePlayerBets(Bet currentBid)
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
