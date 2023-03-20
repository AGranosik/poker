using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks
{
    [TestFixture]
    internal class Fold_StackTests
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
            _stack.AllIn(_player.AllIn());
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
            _stack.AllIn(_player.AllIn());
            _stack.AllIn(_player3.AllIn());
            var func = () => _stack.Fold(_player2.Fold());
            func.Should().NotThrow();

            foreach (var pot in _stack.Pots)
            {
                var winners = pot.GetWinners();
                winners.Winners.Any(w => w == _player2)
                    .Should().BeFalse();
            }
        }

        [Test]
        public void Fold_PlayerInOneAllInPots_Success()
        {
            _stack.Raise(_player3.Raise(Money.Create(120)));
            _stack.Raise(_player2.Raise(Money.Create(140)));
            _stack.AllIn(_player.AllIn());
            _stack.Fold(_player3.Fold());

            var winners = _stack.GetWinners();
            winners.Should().NotBeNullOrEmpty();

            var firstPotWinners = winners.First(w => w.WinningPrize.Value == 150);
            firstPotWinners.Winners.Count.Should().Be(2);

            var secondPotWinners = winners.First(w => w.WinningPrize.Value == 60);
            secondPotWinners.Winners.Count.Should().Be(1);
        }
    }
}
