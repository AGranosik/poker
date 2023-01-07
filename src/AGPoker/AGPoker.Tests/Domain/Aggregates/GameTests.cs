using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class GameTests
    {
        [Test]
        public void Game_PlayerCannotBeNull_ThrowsException()
        {
            var func = () => Game.Create(null, null);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void Game_LimitCannotBeNull_ThrowsException()
        {
            var func = () => Game.Create(Player.Create("hehe", "fiu fiu"), null);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void Game_Creation_Success()
        {
            var func = () => Game.Create(Player.Create("hehe", "fiu fiu"), new GameLimit(3));
            func.Should().NotThrow();
        }

        [Test]
        public void Game_PotIsNotNull_Success()
        {
            var game = Game.Create(Player.Create("hehe", "hehe"), new GameLimit(2));
            game.Pot.Should().NotBeNull();
        }

        [Test]
        public void JoinPlayer_NoPlayers_Success()
        {
            var owner = Player.Create("hehe", "fiu fiu");
            var player = Player.Create("hehe", "fiu fiu2");
            var game = Game.Create(owner, new GameLimit(3));
            var func = () => game.Join(player);
            func.Should().NotThrow();
            game.NumberOfPlayer.Should().Be(1);
        }

        [Test]
        public void JoinPlayer_OwnerCanJoin_Success()
        {
            var owner = Player.Create("hehe", "fiu fiu");
            var game = Game.Create(owner, new GameLimit(3));
            var func = () => game.Join(owner);
            func.Should().NotThrow();
            game.NumberOfPlayer.Should().Be(1);
        }

        [Test]
        public void JoinPlayer_CannotExceedLimit_Success()
        {
            var owner = Player.Create("hehe", "fiu fiu");
            var player = Player.Create("hehe", "fiu fiu2");
            var player2 = Player.Create("hehe", "fiu fiu3");
            var game = Game.Create(owner, new GameLimit(2));
            game.Join(owner);
            game.Join(player);
            var func = () => game.Join(player2);
            func.Should().Throw<Exception>();
        }

        [Test]
        public void JoinPlayer_SamePlayerCannotJoinTwice()
        {
            var owner = Player.Create("hehe", "fiu fiu");
            var player = Player.Create("hehe", "fiu fiu2");
            var game = Game.Create(owner, new GameLimit(3));
            game.Join(owner);
            game.Join(player);
            var func = () => game.Join(player);
            func.Should().Throw<Exception>();
        }
    }
}
