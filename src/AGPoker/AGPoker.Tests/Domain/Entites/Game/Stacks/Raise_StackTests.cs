using AGPoker.Core.Exceptions;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks
{
    [TestFixture]
    internal class Raise_StackTests
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

        [Test] //stack -> get winners??
        public void MultiplePots_RaiseForMultiplePots_Success()
        {
            _stack.Raise(_player.Raise(Money.Create(20)));
            _stack.Raise(_player2.Raise(Money.Create(30)));
            _stack.Raise(_player.Raise(Money.Create(20))); //max: 40
            _stack.Raise(_player3.Raise(Money.Create(50))); //max: 50
            _stack.Raise(_player.Raise(Money.Create(60)));

            _stack.Raise(_player2.Raise(Money.Create(80)));
            var winners = _stack.GetWinners();
            winners.Should().NotBeNullOrEmpty();
            winners.Count.Should().Be(2);

            var firstPotWinners = winners.FirstOrDefault(w => w.WinningPrize == Money.Create(10));
            firstPotWinners.Should().NotBeNull();
            firstPotWinners.Winners.Count.Should().Be(1);
            var firstPotWinner = firstPotWinners.Winners.First();
            firstPotWinner.Should().Be(_player2);

            var secondPotWinners = winners.FirstOrDefault(w => w.WinningPrize == Money.Create(125));
            secondPotWinners.Should().NotBeNull();
            secondPotWinners.Winners.Count.Should().Be(2);
            secondPotWinners.Winners.All(w => w == _player || w == _player2).Should().BeTrue();
        }

        [Test]
        public void MultiplePots_LastPotNotAllIn_Success()
        {
            _stack.Raise(_player.Raise(Money.Create(20)));
            _stack.Raise(_player2.Raise(Money.Create(30)));
            _stack.Raise(_player.Raise(Money.Create(20))); //max: 40
            _stack.Raise(_player3.Raise(Money.Create(50))); //max: 50
            _stack.Raise(_player.Raise(Money.Create(60))); //max:100

            _stack.Raise(_player2.Raise(Money.Create(100))); // 30 next pot
            _stack.Raise(_player3.Raise(Money.Create(100))); //50

            var winners = _stack.GetWinners();
            winners.Should().NotBeNullOrEmpty();

            var firstPotWinners = winners.FirstOrDefault(w => w.WinningPrize == Money.Create(100));
            firstPotWinners.Should().NotBeNull();
            firstPotWinners.Winners.Distinct().Count().Should().Be(3);

            var secondPotWinners = winners.FirstOrDefault(w => w.WinningPrize == Money.Create(80));
            secondPotWinners.Should().NotBeNull();
            secondPotWinners.Winners.Distinct().Count().Should().Be(1);
            secondPotWinners.WinningPrize.Value.Should().Be(80);
        }


        [Test]
        public void MultiplePots_RaiseCannotBeLower_ThrowsException()
        {
            _stack.Raise(_player.Raise(Money.Create(20)));
            _stack.Raise(_player2.Raise(Money.Create(30)));
            _stack.Raise(_player.Raise(Money.Create(20))); //max: 40
            _stack.Raise(_player3.Raise(Money.Create(50))); //max: 50
            _stack.Raise(_player.Raise(Money.Create(60))); //max:100

            _stack.Raise(_player2.Raise(Money.Create(100))); // 30 next pot
            _stack.Raise(_player3.Raise(Money.Create(100))); //50

            _stack.AllIn(_player2.AllIn());
            var func = () => _stack.Raise(_player3.Raise(Money.Create(49)));
            func.Should().Throw<BetTooLowException>();
        }

        [Test]
        public void MultiplePots_RaiseToSecondPotMax_ThrowsException()
        {
            _stack.Raise(_player.Raise(Money.Create(20)));
            _stack.Raise(_player2.Raise(Money.Create(30))); //30
            _stack.Raise(_player.Raise(Money.Create(20))); //40
            _stack.Raise(_player3.Raise(Money.Create(50))); //50
            _stack.Raise(_player.Raise(Money.Create(60))); // 100

            _stack.Raise(_player2.Raise(Money.Create(170)));
            _stack.Call(_player3);

            var winners = _stack.GetWinners();
            winners.Should().NotBeNullOrEmpty();

            var firstPotWinners = winners.FirstOrDefault(w => w.WinningPrize == Money.Create(100));
            firstPotWinners.Should().NotBeNull();
            firstPotWinners.Winners.Distinct().Count().Should().Be(3);

            var secondPotWinners = winners.LastOrDefault(w => w.WinningPrize == Money.Create(100));
            secondPotWinners.Should().NotBeNull();
            secondPotWinners.Winners.Distinct().Count().Should().Be(2);
            secondPotWinners.WinningPrize.Value.Should().Be(100);
        }
    }
}
