using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks
{
    [TestFixture]
    internal class FoldTests
    {
        private Player _player;
        private Player _player2;
        private Player _player3;
        private Stack _stack;

        [SetUp]
        public void SetUp()
        {
            _player = Player.Create("hehe", "eheh", 100);
            _player2 = Player.Create("hehe222", "eheh", 200);
            _player3 = Player.Create("hehe3333", "eheh", 300);

            _stack = Stack.Create();
        }


        [Test]
        public void Fold_PlayerNotInPot_NoException()
        {
            _stack.AllIn(_player);
            var func = () => _stack.Fold(_player2.Fold());
            func.Should().NotThrow();

            foreach(var pot in _stack.Pots)
            {
                var winners = pot.GetWinners();
                winners.Winners.Any(w => w == _player2)
                    .Should().BeFalse();
            }
        }

        [Test]
        public void Fold_Fold_NoException()
        {
            _stack.Fold(_player.Fold());
            var func = () => _stack.Fold(_player2.Fold());
            func.Should().NotThrow();

            foreach (var pot in _stack.Pots)
            {
                var getWinnersFunc = () => pot.GetWinners();
                getWinnersFunc.Should().Throw<ArgumentException>();
            }
        }

        [Test]
        public void Fold_PlayerNotInAnyPot_NoException()
        {
            _stack.AllIn(_player);
            _stack.AllIn(_player3);
            var func = () => _stack.Fold(_player2.Fold());
            func.Should().NotThrow();

            foreach (var pot in _stack.Pots)
            {
                var winners = pot.GetWinners();
                winners.Winners.Any(w => w == _player2)
                    .Should().BeFalse();
            }
        }
    }
}
