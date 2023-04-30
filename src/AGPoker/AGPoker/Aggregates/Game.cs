using AGPoker.Common;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Tables;
using AGPoker.Entites.Game.Turns;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Aggregates
{
    public class Game : IAggregateRoot // just to mark as aggregate root
    {
        private Game(Player owner, GameLimit limit)
        {
            CreateValidation(owner, limit);
            Owner = owner;
            Limit = limit;
            Stack = Stack.Create();
        }

        public static Game Create(Player owner, GameLimit limit)
            => new(owner, limit);

        private List<Player> _players = new();
        public Table Table { get; private set; }
        public Player Owner { get; init; }
        public GameLimit Limit { get; init; }
        public Stack Stack { get; init; }
        public IReadOnlyCollection<Player> Players
            => _players.AsReadOnly();

        public Turn Turn { get; private set; }
        public int NumberOfPlayer => _players.Count;

        public void Begin()
        {
            CanBegin();
            StartTurn();
            TakeBetFromBlinds();
            Table = Table.PreFlop(_players);
        }
        // leave game but its optional
        // next round 
        public void Fold(Player player)
        {
            var turnStatus = Turn.Bet(player, BetType.Fold);
            Table.Fold(player);
            Stack.Fold(Bet.Fold(player));
            MovePerformed(turnStatus);
        }

        public void Call(Player player)
        {
            var turnStatus = Turn.Bet(player, BetType.Call);
            Stack.Call(player);
            MovePerformed(turnStatus);
        }

        public void Raise(Bet bet)
        {
            var turnStatus = Turn.Bet(bet.Player, bet.BetType);
            Stack.Raise(bet);
            MovePerformed(turnStatus);
        }

        
        private void MovePerformed(TurnResult turnResult)
        {
            if(turnResult.Status == TurnStatus.Winners)
            {

            }
            StartNewTurnOrRoundIfNeccessary();
        }

        private void StartNewTurnOrRoundIfNeccessary()
        {
            if (Turn.CanStartNextTurn())
            {
                Turn.NextTurn();
                Table.NextTurn(_players.Where(p => p.Money.Any).ToList());
            }
            else if (Turn.CanStartNextRound())
            {
                Turn.NextRound();
                Table.NextStage();
            }
        }

        public void Join(Player player)
        {
            if(CanPlayerJoin(player))
                _players.Add(player);
        }

        private bool CanPlayerJoin(Player player)
        {
            if (NumberOfPlayer + 1 > Limit.Limit)
                throw new Exception("No more players can be added.");

            if (_players.Contains(player))
                throw new Exception("Player already in the game.");

            return true;
        }

        private void CreateValidation(Player owner, GameLimit limit)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            if (limit is null)
                throw new ArgumentNullException(nameof(limit));
        }

        private void StartTurn()
            => Turn = Turn.Start(_players);
        private void CanBegin()
        {
            if (_players.Count <= 1)
                throw new Exception("Not enough players.");
        }

        private void TakeBetFromBlinds()
        {
            var smallBlindMoney = Money.Create(10);
            var bigBlindMoney = Money.Create(20);
            Stack.Raise(Turn.SmallBlindPlayer.Raise(smallBlindMoney));
            Stack.Raise(Turn.BigBlindPlayer.Raise(bigBlindMoney));
        }
    }
}
