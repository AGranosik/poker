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
        private Player _player2 = Player.Create("hehe2", "hehe", 200);
        private Player _player3 = Player.Create("hehe3", "hehe", 300);

        [SetUp]
        public void SetUp()
        {
            _stack = Stack.Create();
        }

        //cases:
        // current pot can be all in pot
        // create enxt one because the first is already all in and player has highest value in it
        // create enxt one because the first is already all in and player has no highest value in it
        // create enxt one because the first is already all in but player has not enought to even highest bet, so create the third pot 
        //  

        [Test]
        public void AllIn_AllInPotCreated_Success()
        {
            _stack.Raise(Bet.Raise(Money.Create(80), _player));
            _stack.AllIn(_player2);

            _stack.Pots.Count.Should().Be(1);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();
        }

        [Test]
        public void AllIn_AllInEqualToHighestBet_NoAdditionalPotCreated()
        {
            _stack.Raise(Bet.Raise(Money.Create(200), _player3));
            _stack.AllIn(_player2);

            _stack.Pots.Count.Should().Be(1);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();
        }

        [Test]
        public void AllIn_AllInSmallerThenActualHighestBet_SecondPotShoulbeCreated()
        {
            _stack.Raise(Bet.Raise(Money.Create(250), _player3));
            _stack.AllIn(_player2);

            _stack.Pots.Count.Should().Be(2);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();


            var thirdPlayer // check if there is a good winner in each pot.

            _stack.Pots.Count(p => p.HighestBet.Value == 200)
                .Should().Be(1);

            _stack.Pots.Count(p => p.HighestBet.Value == 50)
                .Should().Be(1);
        }
    }
}
