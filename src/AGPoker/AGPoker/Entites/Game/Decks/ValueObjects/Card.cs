namespace AGPoker.Entites.Game.Decks.ValueObjects
{
    public class Card
    {
        // D -> Diamond
        // H -> Heart
        // S -> Spade
        // C -> Club

        private static readonly List<char> _symbols = new() { 'D', 'H', 'S', 'C' };
        private static readonly List<string> _values = new()
        {
            "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2"
        };
        public Card(char symbol, string value)
        {
            Validation(symbol, value);
            Symbol = symbol;
            Value = value;
        }
        public char Symbol { get; init; }
        public string Value { get; init; }

        public static bool operator ==(Card card1, Card card2)
            => card1.Symbol == card2.Symbol && card1.Value == card2.Value;

        public static bool operator !=(Card card1, Card card2)
            => !(card1 == card2);
        private void Validation(char symbol, string value)
        {
            if (!_symbols.Contains(symbol))
                throw new ArgumentException(nameof(symbol));

            if(!_values.Contains(value))
                throw new ArgumentException(nameof(value));
        }

        public static IReadOnlyCollection<char> PossibleSymbols
            => _symbols.AsReadOnly();

        public static IReadOnlyCollection<string> PossibleValues
            => _values.AsReadOnly();
    }
}
