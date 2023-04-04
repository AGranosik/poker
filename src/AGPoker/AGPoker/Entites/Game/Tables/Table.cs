using AGPoker.Entites.Game.Decks;
using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Tables.ValueObjects;

namespace AGPoker.Entites.Game.Tables
{
    public class Table
    {
        private readonly int _handCards = 2;
        private readonly List<Player> _players;
        private readonly Deck _deck;

        public Flop Flop { get; private set; }
        public TableTurn Turn { get; private set; }
        public River River { get; private set; }
        public Table(List<Player> players)
        {
            _deck = Deck.Create();
            _players = players;
            GiveHandToThePlayers();
        }

        public IReadOnlyCollection<Player> GetWinners(List<Player> playersToDecide)
        {
            if (!AreAllPlayersHaveCards())
                throw new InvalidOperationException();

            if (!IsLastStage())
                throw new InvalidOperationException();

            PotencialPlayersWinnerValidation(playersToDecide);
            var playersCombination = playersToDecide.Select(p => 
            {
                var allCards = new List<Card>();
                allCards.AddRange(p.Cards);
                allCards.AddRange(Flop.Cards);
                allCards.AddRange(Turn.Cards);
                allCards.AddRange(River.Cards); //gett all cards method
                return new
                {
                    Player = p,
                    Combination = CardsCombination.GetCombination(allCards)
                };
            })
            .GroupBy(pc => pc.Combination)
            .OrderBy(pc => pc)
            .First();

            return playersCombination.Select(pc => pc.Player).ToList().AsReadOnly();
        }

        private bool IsLastStage()
            => River is not null || Flop is not null || Turn is not null;

        private void PotencialPlayersWinnerValidation(List<Player> playersToDecide)
        {
            if(playersToDecide is null || playersToDecide.Count == 0)
                throw new InvalidOperationException();

            if(!playersToDecide.All(p => _players.Contains(p)))
                throw new ArgumentException();
        }

        private bool AreAllPlayersHaveCards()
            => _players.All(p => p.Cards.Count == 2);

        public void Fold(Player player)
            => _players.Remove(player);

        public void NextStage()
        {
            if(WasRiver())
                throw new InvalidOperationException();

            if (WasTurn())
                StartRiver();
            else if (WasFlop())
                StartTurn();
            else if (WasPreFlop())
                StartFlop();
        }

        private void StartFlop()
        {
            if (!WasPreFlop())
                throw new ArgumentException();

            Flop = Flop.Create(_deck);
        }

        private void StartTurn()
        {
            if(!WasFlop())
                throw new ArgumentException(nameof(Flop));

            Turn = TableTurn.Create(_deck);
        }

        private void StartRiver()
        {
            if (!WasTurn())
                throw new ArgumentException(nameof(Turn));

            River = River.Create(_deck);
        }

        public static Table PreFlop(List<Player> players)
            => new(players);

        private bool WasRiver()
            => River != null;

        private bool WasTurn()
            => WasFlop() && Turn != null;

        private bool WasFlop()
            => Flop != null && WasPreFlop();

        private bool WasPreFlop()
            => _players.All(p => p.Cards.Distinct().Count() == _handCards);

        public void GiveHandToThePlayers()
        {
            var cardsToTake = _players.Count * _handCards;
            var cards = _deck.TakeNextCards(cardsToTake);
            int skip = 0;
            foreach (var player in _players)
            {
                var cardsToGive = cards.Skip(skip).Take(_handCards).ToList();
                skip += 2;
                player.TakeCards(cardsToGive);
            }
        }
    }
}
