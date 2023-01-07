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
        private List<Player> _players = new List<Player>
        {
            Player.Create("hehe2", "hehe2"),
            Player.Create("hehe3", "hehe2"),
            Player.Create("hehe4", "hehe2"),
            Player.Create("hehe5", "hehe2"),

        };
        [SetUp]
        public void Setup()
        {
            var owner = Player.Create("fiu", "fiu");
            _game = Game.Create(owner, new GameLimit(5));
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
            _game.Dealer.Should().NotBeNull();
        }

        [Test]
        public void Begin_SmallBlindPlayerSet_Success()
        {
            AddPlayersToGame();
            _game.Begin();
            _game.Dealer.Should().NotBeNull();
            _game.SmallBlindPlayer.Should().NotBeNull();
        }

        [Test]
        public void Begin_SmallBlindDifferentThanDealer_Success()
        {
            AddPlayersToGame();
            _game.Begin();
            _game.Dealer.Should().NotBeNull();
            _game.SmallBlindPlayer.Should().NotBeNull();

            (_game.Dealer == _game.SmallBlindPlayer).Should().BeFalse();
        }

        [Test]
        public void Begin_BigBlindPlayerSet_Success()
        {
            AddPlayersToGame();
            _game.Begin();
            _game.Dealer.Should().NotBeNull();
            _game.SmallBlindPlayer.Should().NotBeNull();
            _game.BigBlindPlayer.Should().NotBeNull();
        }

        [Test]
        public void Begin_StartPlayersDifferFromEachOther()
        {
            AddPlayersToGame();
            _game.Begin();
            _game.Dealer.Should().NotBeNull();
            _game.SmallBlindPlayer.Should().NotBeNull();
            _game.BigBlindPlayer.Should().NotBeNull();

            var dealerDifferentThanSmallBlind = _game.Dealer != _game.SmallBlindPlayer;
            var dealerDifferentThanBigBlind = _game.Dealer != _game.BigBlindPlayer;
            var smallBlindDifferThanBigBlind = _game.SmallBlindPlayer != _game.BigBlindPlayer;

            dealerDifferentThanBigBlind.Should().BeTrue();
            dealerDifferentThanSmallBlind.Should().BeTrue();
            smallBlindDifferThanBigBlind.Should().BeTrue();
        }

        [Test]
        public void Begin_TakeBids_Success()
        {
            AddPlayersToGame();
            _game.Begin();

            _game.Pot.Value.Value.Should().Be(30);
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
