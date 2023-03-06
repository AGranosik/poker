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
        private Player _player;
        private Player _player2;
        private Player _player3;
        private Player _player4;

        [SetUp]
        public void SetUp()
        {
            _stack = Stack.Create();
            _player = Player.Create("_player", "hehe", 90);
            _player2 = Player.Create("_player2", "hehe", 200);
            _player3 = Player.Create("_player3", "hehe", 300);
            _player4 = Player.Create("_player4", "hehe", 100);
    }

        //cases:
        // current pot can be all in pot
        // create enxt one because the first is already all in and player has highest value in it
        // create enxt one because the first is already all in and player has no highest value in it
        // create enxt one because the first is already all in but player has not enought to even highest bet, so create the third pot 
        //  all when the same player bets in pot

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

            var oldPot = _stack.Pots.First(p => p.HighestBet.Value == 200);

            oldPot.Should().NotBeNull();
            var oldPotWinners = oldPot.GetWinners();
            oldPotWinners.Should().NotBeNull();
            oldPotWinners.Winners.Count.Should().Be(2);
            oldPotWinners.WinningPrize.Value.Should().Be(200);

            var newPot = _stack.Pots.First(p => p.HighestBet.Value == 50);
            newPot.Should().NotBeNull();
            var newPotWinners = newPot.GetWinners();

            newPotWinners.Winners.Count.Should().Be(1);
            newPotWinners.WinningPrize.Value.Should().Be(50);
        }

        [Test]
        public void AllIn_SmallerAllInAfterAnother_EnoughPotsShouldBeCreated()
        {
            _stack.Raise(Bet.Raise(Money.Create(250), _player3)); //90 x3 
            _stack.AllIn(_player2);
            _stack.AllIn(_player);


            _stack.Pots.Count.Should().Be(3);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();

            var firstPot = _stack.Pots.First(p => p.HighestBet.Value == 90);
            firstPot.Should().NotBeNull();
            var firstPotWinners = firstPot.GetWinners();
            firstPotWinners.Winners.Count.Should().Be(3);
            firstPotWinners.WinningPrize.Value.Should().Be(90);

            var secondPot = _stack.Pots.First(p => p.HighestBet.Value == 110);
            secondPot.Should().NotBeNull();
            var secondPotWinners = secondPot.GetWinners();
            secondPotWinners.Winners.Count.Should().Be(2);
            secondPotWinners.WinningPrize.Value.Should().Be(110);

            var thirdPot = _stack.Pots.First(p => p.HighestBet.Value == 50);
            thirdPot.Should().NotBeNull();
            var thirdPotWinners = thirdPot.GetWinners();
            thirdPotWinners.Winners.Count.Should().Be(1);
            thirdPotWinners.WinningPrize.Value.Should().Be(50);
        }

        [Test]
        public void AllIn_SmallerAllInAfterAnother_EnoughPotsShouldBeCreated2()
        {
            _stack.Raise(Bet.Raise(Money.Create(250), _player3));
            _stack.AllIn(_player2);
            _stack.AllIn(_player4);


            _stack.Pots.Count.Should().Be(3);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();

            var firstPot = _stack.Pots.First(p => p.HighestBet.Value == 100);
            firstPot.Should().NotBeNull();
            var firstPotWinners = firstPot.GetWinners();
            firstPotWinners.Winners.Count.Should().Be(3);
            firstPotWinners.WinningPrize.Value.Should().Be(100);

            var secondPot = _stack.Pots.Last(p => p.HighestBet.Value == 100);
            secondPot.Should().NotBeNull();
            var secondPotWinners = secondPot.GetWinners();
            secondPotWinners.Winners.Count.Should().Be(2);
            secondPotWinners.WinningPrize.Value.Should().Be(100);

            var thirdPot = _stack.Pots.Last(p => p.HighestBet.Value == 50);
            thirdPot.Should().NotBeNull();
            var thirdPotWinners = thirdPot.GetWinners();
            thirdPotWinners.Winners.Count.Should().Be(1);
            thirdPotWinners.WinningPrize.Value.Should().Be(50);
        }

        [Test]
        public void AllIn_SmallerAllInAfterAnother_EnoughPotsShouldBeCreated3()
        {
            _stack.Raise(Bet.Raise(Money.Create(250), _player3)); //250 -> 50
            _stack.AllIn(_player2); //200 x2 -> 110 x2 -> 100 x3 
            _stack.AllIn(_player); // 90 x4
            _stack.AllIn(_player4); // 100 -> 10 x3


            _stack.Pots.Count.Should().Be(4);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();

            var firstPot = _stack.Pots.First(p => p.HighestBet.Value == 90);
            firstPot.Should().NotBeNull();
            var firstPotWinners = firstPot.GetWinners();
            firstPotWinners.Winners.Count.Should().Be(4);
            firstPotWinners.WinningPrize.Value.Should().Be(90);

            var secondPot = _stack.Pots.Last(p => p.HighestBet.Value == 100);
            secondPot.Should().NotBeNull();
            var secondPotWinners = secondPot.GetWinners();
            secondPotWinners.Winners.Count.Should().Be(2);
            secondPotWinners.WinningPrize.Value.Should().Be(100);

            var thirdPot = _stack.Pots.Last(p => p.HighestBet.Value == 50);
            thirdPot.Should().NotBeNull();
            var thirdPotWinners = thirdPot.GetWinners();
            thirdPotWinners.Winners.Count.Should().Be(1);
            thirdPotWinners.WinningPrize.Value.Should().Be(50);
        }

        [Test]
        public void AllIn_SmallerAllInAfterAnother_EnoughPotsShouldBeCreated4()
        {
            _stack.Raise(Bet.Raise(Money.Create(250), _player3)); //250 -> 50
            _stack.AllIn(_player2); //200 x2 -> 110 x2 -> 100 x3 
            _stack.AllIn(_player4); // 100 -> 10 x3
            _stack.AllIn(_player); // 90 x4


            _stack.Pots.Count.Should().Be(4);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();

            var firstPot = _stack.Pots.First(p => p.HighestBet.Value == 90);
            firstPot.Should().NotBeNull();
            var firstPotWinners = firstPot.GetWinners();
            firstPotWinners.Winners.Count.Should().Be(4);
            firstPotWinners.WinningPrize.Value.Should().Be(90);

            var secondPot = _stack.Pots.Last(p => p.HighestBet.Value == 100);
            secondPot.Should().NotBeNull();
            var secondPotWinners = secondPot.GetWinners();
            secondPotWinners.Winners.Count.Should().Be(2);
            secondPotWinners.WinningPrize.Value.Should().Be(100);

            var thirdPot = _stack.Pots.Last(p => p.HighestBet.Value == 50);
            thirdPot.Should().NotBeNull();
            var thirdPotWinners = thirdPot.GetWinners();
            thirdPotWinners.Winners.Count.Should().Be(1);
            thirdPotWinners.WinningPrize.Value.Should().Be(50);
        }

        [Test]
        public void AllIn_SmallerAllInAfterAnotherNumberOfPlayerAsPriority_EnoughPotsShouldBeCreated5()
        {
            _stack.AllIn(_player); // 90 -> x4
            _stack.AllIn(_player3); // 300 -> 210 -> 100
            _stack.AllIn(_player2); // 200 -> 110 x2
            _stack.AllIn(_player4); // 100 -> 10 x3


            _stack.Pots.Count.Should().Be(4);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();

            var firstPot = _stack.Pots.FirstOrDefault(p => p.HighestBet.Value == 90);
            firstPot.Should().NotBeNull();
            var firstPotWinners = firstPot.GetWinners();
            firstPotWinners.Winners.Count.Should().Be(4);
            firstPotWinners.WinningPrize.Value.Should().Be(90);

            var secondPot = _stack.Pots.LastOrDefault(p => p.HighestBet.Value == 90);
            secondPot.Should().NotBeNull();
            var secondPotWinners = secondPot.GetWinners();
            secondPotWinners.Winners.Count.Should().Be(1);
            secondPotWinners.WinningPrize.Value.Should().Be(90);

            var fourthPot = _stack.Pots.LastOrDefault(p => p.HighestBet.Value == 110);
            fourthPot.Should().NotBeNull();
            var fourthPottWinners = fourthPot.GetWinners();
            fourthPottWinners.Winners.Count.Should().Be(2);
            fourthPottWinners.WinningPrize.Value.Should().Be(110);

            var thirdPot = _stack.Pots.LastOrDefault(p => p.HighestBet.Value == 10);
            thirdPot.Should().NotBeNull();
            var thirdPotWinners = thirdPot.GetWinners();
            thirdPotWinners.Winners.Count.Should().Be(2);
            thirdPotWinners.WinningPrize.Value.Should().Be(10);
        }

        [Test]
        public void AllIn_SmallerAllInAfterAnotherNumberOfPlayerAsPriority_EnoughPotsShouldBeCreated6()
        {
            _stack.AllIn(_player2); //  200 -> 110 -> 100 x2
            _stack.AllIn(_player); // 90 x4
            _stack.AllIn(_player4); // 10 x3
            _stack.AllIn(_player3); // 100


            _stack.Pots.Count.Should().Be(4);
            _stack.Pots.Any(p => p.IsAllIn)
                .Should().BeTrue();

            var firstPot = _stack.Pots.FirstOrDefault(p => p.HighestBet.Value == 90);
            firstPot.Should().NotBeNull();
            var firstPotWinners = firstPot.GetWinners();
            firstPotWinners.Winners.Count.Should().Be(4);
            firstPotWinners.WinningPrize.Value.Should().Be(90);

            var secondPot = _stack.Pots.FirstOrDefault(p => p.HighestBet.Value == 100);
            secondPot.Should().NotBeNull();
            var secondPotWinners = secondPot.GetWinners();
            secondPotWinners.Winners.Count.Should().Be(2);
            secondPotWinners.WinningPrize.Value.Should().Be(100);

            var fourthPot = _stack.Pots.LastOrDefault(p => p.HighestBet.Value == 100);
            fourthPot.Should().NotBeNull();
            var fourthPottWinners = fourthPot.GetWinners();
            fourthPottWinners.Winners.Count.Should().Be(1);
            fourthPottWinners.WinningPrize.Value.Should().Be(100);

            var thirdPot = _stack.Pots.LastOrDefault(p => p.HighestBet.Value == 10);
            thirdPot.Should().NotBeNull();
            var thirdPotWinners = thirdPot.GetWinners();
            thirdPotWinners.Winners.Count.Should().Be(3);
            thirdPotWinners.WinningPrize.Value.Should().Be(10);
        }
    }
}
