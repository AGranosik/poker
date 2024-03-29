﻿using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using AGPoker.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class Game_TakeBetTests
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
            var money = Money.Create(30);
            var bid = playerNotInGame.Raise(money);
            var func = () => _game.Raise(bid);
            func.Should().Throw<CannotBetException>();
        }

        [Test]
        public void TakeBid_NotPlayerTurn_ThrowsException()
        {
            var notPlayerTurn = _players[0];
            var money = Money.Create(30);
            var bid = notPlayerTurn.Raise(money);
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
            var playerTurn = _players[1];
            var money = Money.Create(30);
            var bid = playerTurn.Raise(money);
            _game.Raise(bid);
            _game.Stack.Worth.Value.Should().Be(60);
        }

        [Test]
        public void TakeBid_EasiestFlow_Success()
        {
            var firstPlayer = _players[1];
            var lastPlayer = _players[0];

            _game.Call(firstPlayer);
            _game.Call(_dealer); 
            _game.Call(_smallBlind);
            _game.Call(_bigBlind);
            _game.Call(lastPlayer);

            _game.Stack.Worth.Value.Should().Be(100);
        }

        [Test]
        public void TakeBid_BidingShouldBeClosed_ThrowsException()
        {
            var firstPlayer = _players[1];
            var lastPlayer = _players[0];
            var money = Money.Create(20);

            _game.Call(firstPlayer);
            _game.Call(_dealer);
            _game.Call(_smallBlind);
            _game.Call(_bigBlind);
            _game.Call(lastPlayer); 

            var func = () => _game.Raise(firstPlayer.Raise(money));
            func.Should().NotThrow();
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
            _game.Call(_players[1]);
            _game.Call(_players[2]);
            _game.Call(_players[3]);
            _game.Call(_players[4]);
            _game.Call(_players[0]);
        }
    }
}
