using AGPoker.Common;
using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Players.ValueObjects;
using AGPoker.Entites.Game.Pots.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Game.Players
{
    public class Player : Entity
    {
        private Player(PlayerName playerName, PlayerSurname playerSurname)
        {
            PlayerName = playerName;
            PlayerSurname = playerSurname;
            Chips = Chips.Create(500);
        }

        public PlayerName PlayerName { get; init; }
        public PlayerSurname PlayerSurname { get; init; }
        public Chips Chips { get; init; }
        public IReadOnlyCollection<Card> Cards
            => _cards.AsReadOnly();

        private List<Card> _cards = new();
        public Bid MakeABid(Money amount)
        {
            var bidChips = Chips.TakeAwayChips(amount);
            return Bid.Create(bidChips, this);
        }

        public void TakeCards(List<Card> cards)
        {
            CheckIfCardsNotNullOrEmpty(cards);
            _cards.AddRange(cards);
        }

        private static void CheckIfCardsNotNullOrEmpty(List<Card> cards)
        {
            if (cards is null || cards.Count == 0)
                throw new ArgumentNullException(nameof(cards));
        }

        public static Player Create(string playerName, string playerSurname)
            => new(PlayerName.Create(playerName), PlayerSurname.Create(playerSurname));

        public static bool operator ==(Player player1, Player player2)
        {
            return player1.PlayerName.Value == player2.PlayerName.Value
                && player2.PlayerSurname.Value == player1.PlayerSurname.Value;
        }


        public static bool operator !=(Player player1, Player player2)
            => !(player1 == player2);

    }
}
