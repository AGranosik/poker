using AGPoker.Common;
using AGPoker.Entites.Game.Decks;
using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Aggregates
{
    public class Game : IAggregateRoot // just to mark as aggregate root
    {
        private readonly int _handCards = 2;
        private Game(Player owner, GameLimit limit)
        {
            CreateValidation(owner, limit);
            Owner = owner;
            Limit = limit;
            Stack = Stack.Create();
            _deck = Deck.Create();
        }

        public static Game Create(Player owner, GameLimit limit)
            => new(owner, limit);

        private List<Player> _players = new();
        private Deck _deck;
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
            TakeBidFromBlinds();
            GiveHandToThePlayers();
        }
        // leave game but its optional
        public void Fold(Player player)
        {
            Turn.Bet(player, BetType.Fold);
        }

        public void Call(Player player)
        {
            Stack.Call(player);
            Turn.Bet(player, BetType.Call);
        }

        public void Raise(Bet bet)
        {
            Stack.Raise(bet);
            Turn.Bet(bet.Player, bet.BetType);
        }

        public void GiveHandToThePlayers()
        {
            var cardsToTake = _players.Count * _handCards;
            var cards = TakeCards(cardsToTake);
            int skip = 0;
            foreach(var player in _players)
            {
                var cardsToGive = cards.Skip(skip).Take(_handCards).ToList();
                skip += 2;
                player.TakeCards(cardsToGive);
            }   
        }

        private List<Card> TakeCards(int n)
        {
            var cards = new List<Card>(n);
            for(int i =0; i < n; i++)
                cards.Add(_deck.GetNextCard());

            return cards;
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

        private void TakeBidFromBlinds()
        {
            var smallBlindMoney = Money.Create(10);
            var bigBlindMoney = Money.Create(20);
            Stack.Raise(Turn.SmallBlindPlayer.Raise(smallBlindMoney));
            Stack.Raise(Turn.BigBlindPlayer.Raise(bigBlindMoney));
        }
    }
}
