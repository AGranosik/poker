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
        private Player _dealer;
        private Player _smallBlind;
        private Player _bigBlind;

        [SetUp]
        public void Setup()
        {
            var owner = Player.Create("fiu", "fiu");
            _game = Game.Create(owner, new GameLimit(5));
            _dealer = Player.Create("hehe3", "hehe2");
            _smallBlind = Player.Create("hehe4", "hehe2");
            _bigBlind = Player.Create("hehe5", "hehe2");
            _players = new List<Player>
            {
                _dealer,
                _smallBlind,
                _bigBlind,
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
        public void Takebid_CannotCheckIfNotEqualsToHighestBid_ThrowsException()
        {

        }

        [Test]
        public void TakeBid_EasiestFlow_Success()
        {
            var firstPlayer = _players.Last();
            var chips = Chips.Create(20);

            _game.TakeBid(Bid.Create(chips, firstPlayer)); // 20 - 0 - 10 -20
            _game.TakeBid(Bid.Create(chips, _dealer)); // 20 - 20 - 10 - 20
            _game.TakeBid(Bid.Create(Chips.Create(10), _smallBlind));
            _game.Check(_bigBlind);
        }

        [Test]
        public void TakeBid_BidingShouldBeClosed_ThrowsException()
        {
            var firstPlayer = _players.Last();
            var chips = Chips.Create(20);

            _game.TakeBid(Bid.Create(chips, firstPlayer)); // 20 - 0 - 10 -20
            _game.TakeBid(Bid.Create(chips, _dealer)); // 20 - 20 - 10 - 20
            _game.TakeBid(Bid.Create(Chips.Create(10), _smallBlind));
            _game.Check(_bigBlind);

            var func = () => _game.TakeBid(Bid.Create(chips, firstPlayer));
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
