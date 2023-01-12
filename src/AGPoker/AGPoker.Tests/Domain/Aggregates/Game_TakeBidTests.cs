using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
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
                Player.Create("hehe3", "hehe2"), // dealer
                Player.Create("hehe4", "hehe2"), // small blind
                Player.Create("hehe5", "hehe2"), // big blind
                Player.Create("hehe6", "hehe2"),
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

        [Test]
        public void TakeBid_SimpleBid_Success()
        {
            var playerTurn = _players.Last();
            var chips = Chips.Create(30);
            var bid = Bid.Create(chips, playerTurn);
            _game.TakeBid(bid);
            _game.Stack.Value.Value.Should().Be(60);
        }

        [Test]
        public void TakeBid_EasiestFlow_Success()
        {
            var firstPlayer = _players.Last();
            var chips = Chips.Create(20);

            _game.TakeBid(Bid.Create(chips, firstPlayer)); // 20 - 0 - 10 -20
            _game.TakeBid(Bid.Create(chips, _game.Dealer)); // 20 - 20 - 10 - 20
            _game.TakeBid(Bid.Create(Chips.Create(10), _game.SmallBlindPlayer));
            _game.Check(_game.BigBlindPlayer);
        }

        [Test]
        public void TakeBid_BidingShouldBeClosed_ThrowsException()
        {
            var firstPlayer = _players.Last();
            var chips = Chips.Create(20);

            _game.TakeBid(Bid.Create(chips, firstPlayer)); // 20 - 0 - 10 -20
            _game.TakeBid(Bid.Create(chips, _game.Dealer)); // 20 - 20 - 10 - 20
            _game.TakeBid(Bid.Create(Chips.Create(10), _game.SmallBlindPlayer));
            _game.Check(_game.BigBlindPlayer);

            var func = () => _game.TakeBid(Bid.Create(chips, firstPlayer));
            func.Should().Throw<ArgumentException>();
        }

        private List<Player> GetPlayerInBidOrder()
        {
            return new List<Player>
            {
                _players.Last(),
                _players[0],
                _players[1],
                _players[2],
            };
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
