using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using AGPoker.Exceptions;
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
                Player.Create("hehe6", "hehe2"),
                Player.Create("hehe7", "hehe2"),
                _dealer,
                _smallBlind,
                _bigBlind
            };
            AddPlayersToGame();
            _game.Begin();
        }

        [Test]
        public void TakeBid_PlayerNotInGame_ThrowsException()
        {
            var playerNotInGame = Player.Create("sadasd", "adasd");
            var chips = Chips.Create(30);
            var bid = playerNotInGame.Raise(chips.Amount);
            var func = () => _game.Raise(bid);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void TakeBid_NotPlayerTurn_ThrowsException()
        {
            var notPlayerTurn = _players[1];
            var chips = Chips.Create(30);
            var bid = notPlayerTurn.Raise(chips.Amount);
            var func = () => _game.Raise(bid);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void TakeBid_RoundClosedCannotTakeBet_ThrowsException()
        {
            AllPlayersCalled();

            var func = () => _game.Call(_players[0]);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void TakeBid_SimpleBid_Success()
        {
            var playerTurn = _players[0];
            var chips = Chips.Create(30);
            var bid = playerTurn.Raise(chips.Amount);
            _game.Raise(bid);
            _game.Stack.Value.Value.Should().Be(60);
        }

        [Test]
        public void TakeBid_EasiestFlow_Success()
        {
            var firstPlayer = _players[0];
            var secondPlayer = _players[1];

            _game.Call(firstPlayer);
            _game.Call(secondPlayer);
            _game.Call(_dealer); 
            _game.Call(_smallBlind);
            _game.Call(_bigBlind);

            _game.Stack.Value.Value.Should().Be(100);
        }

        [Test]
        public void TakeBid_BidingShouldBeClosed_ThrowsException()
        {
            var firstPlayer = _players[0];
            var secondPlayer = _players[1];
            var chips = Chips.Create(20);


            _game.Call(firstPlayer); // 20 - 0 - 10 -20
            _game.Call(secondPlayer); // 20 - 0 - 10 -20
            _game.Call(_dealer); // 20 - 20 - 10 - 20
            _game.Call(_smallBlind);
            _game.Call(_bigBlind);

            var func = () => _game.Raise(firstPlayer.Raise(chips.Amount));
            func.Should().Throw<CannotBetException>();
        }

        private void AddPlayersToGame()
        {
            foreach(var player in _players)
            {
                _game.Join(player);
            }
        }

        private void AllPlayersCalled()
        {
            _game.Call(_players[0]);
            _game.Call(_players[1]);
            _game.Call(_players[2]);
            _game.Call(_players[3]);
            _game.Call(_players[4]);
        }
    }
}
