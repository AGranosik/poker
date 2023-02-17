using AGPoker.Common;
using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Players.ValueObjects;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;

namespace AGPoker.Entites.Game.Game.Players
{
    public class Player : Entity
    {
        private Player(PlayerName playerName, PlayerSurname playerSurname, Money money)
        {
            PlayerName = playerName;
            PlayerSurname = playerSurname;
            Money = money;
        }

        public PlayerName PlayerName { get; init; }
        public PlayerSurname PlayerSurname { get; init; }
        public Money Money { get; init; }
        public IReadOnlyCollection<Card> Cards
            => _cards.AsReadOnly();

        private List<Card> _cards = new();

        public Bet AllIn()
            => Bet.AllIn(this, Money.TakeAll());
        public Bet Call(Money amount = null)
        {
            if(amount is not null)
                Money.Split(amount);
            return Bet.Call(this, amount ?? Money.None);
        }

        public Bet Raise(Money amount)
        {
            if (LastChipsGonnaBeTaken(amount))
                return AllIn();

            Money.Split(amount);
            return Bet.Raise(amount, this);
        }

        public Bet Fold()
            => Bet.Fold(this);

        public void TakeCards(List<Card> cards)
        {
            CheckIfCardsNotNullOrEmpty(cards);
            _cards.AddRange(cards);
        }

        public bool LastChipsGonnaBeTaken(Money amount)
            => Money == amount;

        private static void CheckIfCardsNotNullOrEmpty(List<Card> cards)
        {
            if (cards is null || cards.Count == 0)
                throw new ArgumentNullException(nameof(cards));
        }

        public static Player Create(string playerName, string playerSurname, int chips = 500)
            => new(PlayerName.Create(playerName), PlayerSurname.Create(playerSurname), Money.Create(chips));

        public static bool operator ==(Player player1, Player player2)
        {
            return player1.PlayerName.Value == player2.PlayerName.Value
                && player2.PlayerSurname.Value == player1.PlayerSurname.Value;
        }


        public static bool operator !=(Player player1, Player player2)
            => !(player1 == player2);

    }
}
