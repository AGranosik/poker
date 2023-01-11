using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Pots.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class Game_TakeBidTests
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
                Player.Create("hehe3", "hehe2"), // dealer
                Player.Create("hehe4", "hehe2"), // small blind
                Player.Create("hehe5", "hehe2"), // big blind
            };
            AddPlayersToGame();
            _game.Begin();
        }

        [Test]
        public void TakeBid_PlayerNotInGame_ThrowsException()
        {
            var playerNotInGame = Player.Create("sadasd", "adasd");
            var chips = Chips.Create(30);
            var bid = Bid.Create(chips, playerNotInGame);
            var func = () => _game.TakeBid(bid);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void TakeBid_NotPlayerTurn_ThrowsException()
        {
            var notPlayerTurn = _players.First();
            var chips = Chips.Create(30);
            var bid = Bid.Create(chips, notPlayerTurn);
            var func = () => _game.TakeBid(bid);
            func.Should().Throw<ArgumentException>();
        }

        private void AddPlayersToGame()
        {
            foreach(var player in _players)
            {
                _game.Join(player);
            }
        }
    }
}
