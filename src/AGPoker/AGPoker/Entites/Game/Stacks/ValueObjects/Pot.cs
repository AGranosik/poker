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

        private Pot(List<Bet> bets) // tests
        {
            _bets = bets;
            var maxBet = _bets.GroupBy(b => b.Player)
                .Max(p => p.Sum(x => x.Money.Value));

            SetHighestBidIfNeccessary(maxBet);
        }
        public Money HighestBet
            => Money.Create(_highestBet.Value);
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
            var playerBets = GetPlayerBets(bet);
            playerBets.Add(bet);
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

        public List<Bet> AllIn(Bet bet) //shouldnt be bet everywhere??
        {
            _bets.Add(bet);
            var playerBetsAmount = GetPlayerBetAmount(GetPlayerBets(bet));
            var result =  SplitIntoSmaller(bet); // set highest bet
            SetHighestBidIfNeccessary(playerBetsAmount, result.Count > 0);
            return result;
        }

        public bool CanTakeAllInBetPart(Bet bet)
        {
            if (!IsAllInBet(bet))
                throw new ArgumentException();

            var playerBets = GetPlayerBets(bet);
            playerBets.Add(bet);
            var betsAmount = GetPlayerBetAmount(playerBets);

            return betsAmount >= _highestBet.Value;
        }

        public void TakePartOfAllInBet(Bet bet)
        {
            if (!CanTakeAllInBetPart(bet))
                throw new ArgumentException();

            var playerBets = GetPlayerBets(bet);
            playerBets.Add(bet);
            var betsAmount = GetPlayerBetAmount(playerBets);

            var howMuchToTake = Money.Create(_highestBet.Value - betsAmount);
            bet.Money.Split(howMuchToTake); // method in bet where we checking if its an all in bet.
        }

        public List<Bet> SplitIntoSmaller(Bet newHigestBet)
        {
            SplitIntoSmallerValidation();

            var betsAboveHighest = new List<Bet>();
            var allPlayers = _bets.Select(b => b.Player)
                .Distinct()
                .ToList();

            foreach (var player in allPlayers)
                betsAboveHighest.AddRange(SplitPlayerBetsUntilEqualNewHighestBet(newHigestBet, player));

            // cut bets into new highest bet.
            return betsAboveHighest;
        }

        public static Pot Create()
            => new();

        public static Pot Create(List<Bet> bets)
            => new(bets);

        private List<Bet> SplitPlayerBetsUntilEqualNewHighestBet(Bet newHighestBet, Player player)
        {
            var playerBets = _bets.Where(b => b.Player == player);
            var betsAboveHighestBet = new List<Bet>();
            Money sum = Money.None;
            var highestBetValue = newHighestBet.Money;

            foreach(var bet in playerBets)
            {
                var newSum = sum + bet.Money;

                if (sum >= highestBetValue)
                    betsAboveHighestBet.Add(bet);
                else if (newSum > highestBetValue)
                {
                    var howMuchToTakeFromBet = newSum - highestBetValue;
                    var splitedBet = bet.Split(howMuchToTakeFromBet);
                    betsAboveHighestBet.Add(splitedBet); // split bet if neccessary
                }

                sum = newSum;
            }

            return betsAboveHighestBet;
        }

        private void SplitIntoSmallerValidation()
        {
            if (_bets is null || _bets.Count() == 0)
                throw new ArgumentException();
        }

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

        private void SetHighestBidIfNeccessary(int playerBidAmount, bool wasSplited = false)
        {
            if (wasSplited || playerBidAmount > _highestBet.Value)
                _highestBet = Money.Create(playerBidAmount);
        }

        private int GetPlayerBetAmount(List<Bet> playerBids)
            => playerBids.Sum(b => b.Money.Value);

        private List<Bet> GetPlayerBets(Bet currentBid)
        {
            var bids = _bets.Where(b => b.Player == currentBid.Player).ToList();
            return bids;
        }

        private bool IsFoldBet(Bet bet)
            => bet.BetType == BetType.Fold && bet.Money == Money.None;

        private bool IsAllInBet(Bet bet)
            => bet.BetType == BetType.AllIn && bet.Player.Money == Money.None;
        private bool IsPlayerFoldedBefore(Player player)
            => _bets.Any(b => BetType.Fold == b.BetType && b.Player == player);
    }
}
