using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks
{
    [TestFixture]
    internal class Stack_SinglePot_Tests
    {
        private Stack _stack;
        private List<Player> _players = new List<Player>
        {
            Player.Create("1", "1"),
            Player.Create("2", "2"),
            Player.Create("3", "3")
        };

        [SetUp]
        public void SetUp()
        {
            _stack = Stack.Create();
        }

        [Test]
        public void Winner_NoBetsNoWinners_ReturnsEmptyList()
        {
            var func = () => _stack.GetWinners();
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Winner_SingleBetSingleWinner_ReturnsList()
        {
            _stack.Raise(Bet.Raise(Money.Create(12), _players[0]));
            var winners = _stack.GetWinners();
            winners.Should().NotBeNull();
            winners.Should().NotBeEmpty();

            var singlePotWinner = winners[0];
            singlePotWinner.Winners.Count.Should().Be(1);
            singlePotWinner.WinningPrize.Value.Should().Be(12);
            (singlePotWinner.Winners.First() == _players[0]).Should().BeTrue();
        }

        [Test]
        public void Winner_MultipleBetsOneIsWnable_ReturnsList()
        {
            _stack.Raise(Bet.Raise(Money.Create(12), _players[0]));
            _stack.Raise(Bet.Raise(Money.Create(13), _players[1]));
            _stack.Call(_players[0]);
            _stack.Raise(Bet.Raise(Money.Create(14), _players[2]));


            var winners = _stack.GetWinners();
            winners.Should().NotBeNull();
            winners.Should().NotBeEmpty();

            var singlePotWinner = winners[0];
            singlePotWinner.Winners.Count.Should().Be(1);
            singlePotWinner.WinningPrize.Value.Should().Be(40);
            (singlePotWinner.Winners.First() == _players[2]).Should().BeTrue();
        }

        [Test]
        public void Winner_MultipleBetsOneIsWnable_ReturnsList2()
        {
            _stack.Raise(Bet.Raise(Money.Create(12), _players[0]));
            _stack.Raise(Bet.Raise(Money.Create(33), _players[1]));
            _stack.Call(_players[0]);
            _stack.Raise(Bet.Raise(Money.Create(50), _players[2]));
            _stack.Raise(Bet.Raise(Money.Create(80), _players[1]));
            _stack.Fold(Bet.Fold(_players[2]));


            var winners = _stack.GetWinners();
            winners.Should().NotBeNull();
            winners.Should().NotBeEmpty();

            var singlePotWinner = winners[0];
            singlePotWinner.Winners.Count.Should().Be(1);
            singlePotWinner.WinningPrize.Value.Should().Be(196);
            (singlePotWinner.Winners.First() == _players[1]).Should().BeTrue();
        }
    }
}
