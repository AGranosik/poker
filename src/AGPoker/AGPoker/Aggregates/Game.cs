using AGPoker.Common;
using AGPoker.Entites.Game.Decks;
using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Aggregates
{
    // minimal bid
    // minimal diff
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

        public static Game Create(Player owner, GameLimit limit) // domain objects or not....
            => new(owner, limit);

        private List<Player> _players = new();
        private Deck _deck;
        public Player Owner { get; init; }
        public Player Dealer { get; private set; }
        public Player SmallBlindPlayer { get; private set; }
        public Player BigBlindPlayer { get; private set; }
        public GameLimit Limit { get; init; }
        public Stack Stack { get; init; }
        public IReadOnlyCollection<Player> Players
            => _players.AsReadOnly();

        private int _currentPlayerIndex = 0;
        public int NumberOfPlayer => _players.Count;

        public void Begin()
        {
            CanBegin();
            SetDealer();
            SetSmallBlindPlayer();
            SetBigBlindPlayer();
            TakeBidFromBlinds();
            GiveHandToThePlayers();
        }

        // 
        public void TakeBid(Bid bid)
        {
            var bidPlayerIndex = _players.IndexOf(bid.Player);
            if (bidPlayerIndex == -1)
                throw new ArgumentException("No player in the game.");

            CheckIfPlayersTurn(bidPlayerIndex);
            // should create new pot
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

        private List<Card> TakeCards(int n) // probably should be rand
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

        private void CheckIfPlayersTurn(int bidPlayerIndex)
        {
            if (bidPlayerIndex != _currentPlayerIndex)
                throw new ArgumentException("Not this player turn.");
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

        private void SetDealer()
        {
            Dealer = GetNextPlayer();
        }

        private void SetSmallBlindPlayer()
        {
            SmallBlindPlayer= GetNextPlayer();
        }

        private void SetBigBlindPlayer()
        {
            BigBlindPlayer= GetNextPlayer();
        }
        private void CanBegin()
        {
            if (_players.Count <= 1) //use game limit
                throw new Exception("Not enough players.");
        }

        private Player GetNextPlayer()
        {
            if (_currentPlayerIndex == _players.Count - 1)
                return _players[0];

            return _players[++_currentPlayerIndex];
        }

        private void TakeBidFromBlinds()
        {
            var smallBlindMoney = Money.Create(10);
            var bigBlindMoney = Money.Create(20);
            Stack.TakeABid(SmallBlindPlayer.MakeABid(smallBlindMoney));
            Stack.TakeABid(BigBlindPlayer.MakeABid(bigBlindMoney));
        }
    }
}
