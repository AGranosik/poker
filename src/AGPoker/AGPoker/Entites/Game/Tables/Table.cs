﻿using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Decks;
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

        public void StartFlop()
        {
            if (!WasPreFlop())
                throw new ArgumentException();

            Flop = Flop.Create(_deck);
        }

        public void StartTurn()
        {
            if(!WasFlop())
                throw new ArgumentException(nameof(Flop));

            Turn = TableTurn.Create(_deck);
        }

        public void StartRiver()
        {
            if (!WasTurn())
                throw new ArgumentException(nameof(Turn));

            River = River.Create(_deck);
        }

        public static Table PreFlop(List<Player> players)
            => new(players);

        private bool WasTurn()
            => WasFlop() || Turn != null;

        private bool WasFlop()
            => Flop != null || WasPreFlop();

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