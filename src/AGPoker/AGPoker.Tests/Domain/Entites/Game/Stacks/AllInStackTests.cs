using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks
{
    [TestFixture]
    internal class AllInStackTests
    {
        private Stack _stack;
        private Player _player = Player.Create("hehe", "hehe", 100);
        private Player _player2 = Player.Create("hehe", "hehe", 200);
        private Player _player3 = Player.Create("hehe", "hehe", 300);

        [SetUp]
        public void SetUp()
        {
            _stack = Stack.Create();
        }

        [Test]
        public void AllIn_AllInPotCreated_Success()
        {
            _stack.Raise(Bet.Raise(Money.Create(80), _player));
            //_stack.al(Bet.AllIn(_player2));

        }
    }
}
