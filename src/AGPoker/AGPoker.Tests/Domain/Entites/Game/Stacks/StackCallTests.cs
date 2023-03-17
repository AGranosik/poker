using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks
{
    [TestFixture]
    internal class StackCallTests
    {
        private Player _player;
        private Player _player2;
        private Player _player3;
        private Player _player4;
        private Stack _stack;

        [SetUp]
        public void SetUp()
        {
            _player = Player.Create("hehe", "eheh", 100);
            _player2 = Player.Create("hehe222", "eheh", 200);
            _player3 = Player.Create("hehe3333", "eheh", 300);
            _player4 = Player.Create("hehe3333", "eheh", 100);

            _stack = Stack.Create();
        }

        [Test]
        public void Call_SinglePotSingleBet_Success()
        {
            var func = () => _stack.Call(_player);
            func.Should().NotThrow();
        }

        [Test]
        public void Call_SinglePoMultiplePlayers_Success()
        {
            var func = () => _stack.Call(_player2);
            func.Should().NotThrow();

            func = () => _stack.Call(_player3);
            func.Should().NotThrow();

            func = () => _stack.Call(_player);
            func.Should().NotThrow();
        }

        [Test]
        public void MultiplePotsSinglePlayer_NotEnoughMney_ThrowsException()
        {
            _stack.AllIn(_player.AllIn());
            _stack.AllIn(_player2.AllIn());

            var func = () => _stack.Call(_player4);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void MultiplePots_EnoughMoney_Success()
        {
            _stack.AllIn(_player.AllIn());
            _stack.AllIn(_player2.AllIn());

            var func = () => _stack.Call(_player3);
            func.Should().NotThrow();

            _player3.Money.Value.Should().Be(100);
            _stack.Pots.Count.Should().Be(2);
        }
    }
}
