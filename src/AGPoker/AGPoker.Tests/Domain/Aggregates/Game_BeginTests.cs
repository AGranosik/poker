using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class Game_BeginTests
    {
        private Game _game;
        private List<Player> _players;
        [SetUp]
        public void Setup()
        {
            var owner = Player.Create("fiu", "fiu");
            _game = Game.Create(owner, new GameLimit(5));
            _players = new List<Player>
            {
                Player.Create("hehe2", "hehe2"),
                Player.Create("hehe3", "hehe2"),
                Player.Create("hehe4", "hehe2"),
                Player.Create("hehe5", "hehe2"),

            };
        }

        [Test]
        public void Bagin_NotEnoughPlayers_ThrowsException()
        {
            var func = () => _game.Begin();
            func.Should().Throw<Exception>();
        }

        [Test]
        public void Begin_DealerIsSet_Success()
        {
            AddPlayersToGame();
            _game.Begin();
            _game.Turn.Dealer.Should().NotBeNull();
        }

        [Test]
        public void Begin_TakeBids_Success()
        {
            AddPlayersToGame();
            _game.Begin();

            _game.Stack.Worth.Value.Should().Be(30);
        }

        [Test]
        public void Begin_GiveHandToThePlayers_Success()
        {
            var expectedNumberOfCards = _players.Count * 2;
            AddPlayersToGame();
            _game.Begin();
            var numberOfCardsInGame = _game.Players.Sum(p => p.Cards.Count);
            numberOfCardsInGame
                .Should().Be(expectedNumberOfCards);
        }

        private void AddPlayersToGame()
        {
            foreach (var player in _players)
            {
                _game.Join(player);
            }
        }
    }
}
