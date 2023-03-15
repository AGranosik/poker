using AGPoker.Common.ValueObjects;
using AGPoker.Core.Exceptions;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Stacks.ValueObjects
{
    public class Pot : ValueObject
    {
        private List<Bet> _bets = new();
        private Money _highestBet = Money.Create(0);

        private Pot() { }

        private Pot(List<Bet> bets)
        {
            CreationValition(bets);
            _bets = bets;
            var maxBet = _bets.GroupBy(b => b.Player)
                .Max(p => p.Sum(x => x.Money.Value));

            SetHighestBetIfNeccessary(Money.Create(maxBet));
        }

        public Money HighestBet
            => Money.Create(_highestBet.Value);
        public Money Value
            => Money.Create(_bets.Sum(b => b.Money.Value));

        public int GetNumberOfPlayers()
            => _bets.Select(b => b.Player).Distinct().Count();

        public bool IsAllIn()
            => _bets.Any(b => b.IsAllIn());

        public PotWinner GetWinners()
        {
            var groppedBets = _bets.GroupBy(b => b.Player)
                .Where(gp => !gp.Any(b => b.IsFolded()))
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
        public void Raise(Bet bet)
        {
            _bets.Add(bet);
            var playerBetsAmount = GetPlayerBetAmount(bet.Player);
            RaiseValidation(playerBetsAmount, bet.Player);
            SetHighestBetIfNeccessary(playerBetsAmount);
        }

        public void Call(Player player)
        {
            var playerMoney = GetPlayerBetAmount(player);

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

        public List<Bet> AllIn(Bet bet)// add tests where there are bets to split
        {
            if (!bet.IsAllIn())
                throw new ArgumentException();

            _bets.Add(bet);
            var playerBetsAmount = GetPlayerBetAmount(bet.Player);
            var betsAboveHighest = SplitIntoSmaller(playerBetsAmount);
            SetHighestBetIfNeccessary(playerBetsAmount, betsAboveHighest.Count > 0);
            return betsAboveHighest;
        }

        public bool CanTakeBetPart(Bet bet)
        {
            if(!IsAllIn())
                return false;

            var betsAmount = GetPlayerBetsWithActual(bet);

            return betsAmount >= _highestBet.Value;
        }

        public void TakePartOfAllInBet(Bet bet) //tests
        {
            if (!CanTakeBetPart(bet))
                throw new ArgumentException();

            var betsAmount = GetPlayerBetsWithActual(bet);

            var howMuchToTake = Money.Create(betsAmount - HighestBet.Value);
            var result = bet.Split(bet.Money - howMuchToTake);
            _bets.Add(result);
        }

        private List<Bet> SplitIntoSmaller(Money newHigestBet)
        {
            SplitIntoSmallerValidation();

            var betsAboveHighest = new List<Bet>();
            var allPlayers = GetAllPlayers();

            foreach (var player in allPlayers)
                betsAboveHighest.AddRange(SplitPlayerBetsUntilEqualNewHighestBet(newHigestBet, player));

            return betsAboveHighest;
        }

        public static Pot Create()
            => new();

        public static Pot Create(List<Bet> bets)
            => new(bets);

        public int GetPlayerBetsWithActual(Bet actualBet)
        {
            var playerBets = GetPlayerBets(actualBet.Player);
            playerBets.Add(actualBet);
            return playerBets.Sum(pb => pb.Money.Value);
        }

        public static void CreationValition(List<Bet> bets)
        {
            if (bets is null)
                throw new ArgumentNullException();

            if (bets.Count == 0)
                throw new ArgumentException(nameof(bets));

            var anyWithValue = bets.Any(b => !b.IsFolded() && b.Money.Any);

            if(!anyWithValue)
                throw new ArgumentException(nameof(bets));
        }

        private List<Player> GetAllPlayers()
            => _bets.Select(b => b.Player).Distinct().ToList();

        private List<Bet> SplitPlayerBetsUntilEqualNewHighestBet(Money newHighestBet, Player player)
        {
            var playerBets = GetPlayerBets(player);
            var betsAboveHighestBet = new List<Bet>();
            Money previousBetsSum = Money.None;

            foreach(var bet in playerBets)
            {
                var actualBetsSum = previousBetsSum + bet.Money;
                var isAlreadyAboveHigestBet = previousBetsSum >= newHighestBet;
                var isNowAboveHighestBet = actualBetsSum > newHighestBet;

                if (isAlreadyAboveHigestBet)
                    betsAboveHighestBet.Add(bet);
                else if (isNowAboveHighestBet)
                {
                    var howMuchToTakeFromBet = actualBetsSum - newHighestBet;
                    var splitedBet = bet.Split(howMuchToTakeFromBet);
                    betsAboveHighestBet.Add(splitedBet);
                }

                previousBetsSum = actualBetsSum;
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

            return Money.Create(moneyInPot / numberOfWinners);
        }


        private bool ShouldPlayerGiveChips(Money playerMoney)
            => playerMoney < _highestBet;

        private void RaiseValidation(Money playerBetsValue, Player player)
        {
            if (playerBetsValue < _highestBet)
                throw new BetTooLowException();

            if (IsPlayerFoldedBefore(player))
                throw new PlayerFoldedBeforeException();
        }

        private void SetHighestBetIfNeccessary(Money playerBetAmount, bool wasSplited = false)
        {
            if (wasSplited || playerBetAmount > _highestBet)
                _highestBet = playerBetAmount;
        }

        private Money GetPlayerBetAmount(Player player)
            => Money.Create(GetPlayerBets(player).Sum(b => b.Money.Value));

        private List<Bet> GetPlayerBets(Player player)
        {
            var bets = _bets.Where(b => b.Player == player).ToList();
            return bets;
        }

        private bool IsFoldBet(Bet bet)
            => bet.BetType == BetType.Fold && bet.Money == Money.None;

        private bool IsPlayerFoldedBefore(Player player)
            => _bets.Any(b => BetType.Fold == b.BetType && b.Player == player);
    }
}
