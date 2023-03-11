using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_CreationTests
    {
        private Player _player;
        private Player _player2;

        [SetUp]
        public void SetUp()
        {
            _player = Player.Create("hehe", "ssss");
            _player2 = Player.Create("fiu", "sdsffjsdf");
        }

        [Test]
        public void Creation_NoBets_Success()
        {
            var func = () => Pot.Create();
            func.Should().NotThrow();
        }

        [Test]
        public void Bets_CannotBeNull_ThrowsException()
        {
            var func = () => Pot.Create(null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Bets_CannotBeEmpty_ThrowsException()
        {
            var func = () => Pot.Create(new List<Bet>());
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void SingleBet_CannotBeFalded_ThrowsException()
        {
            var bet = _player.Fold();
            var func = () => Pot.Create(new List<Bet> { bet });
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void MultipleBets_AtleastOneNoFoldedWithValue_ThrowsException()
        {
            var bets = new List<Bet>
            {
                _player.Fold(),
                _player2.Call()
            };

            var func = () => Pot.Create(bets);
            func.Should().Throw<ArgumentException>();
        }


        [Test]
        public void MultipleBets_AtleastOneNoFoldedWithValue_HighestBetSet()
        {
            var bets = new List<Bet>
            {
                _player.Fold(),
                _player2.Call(),
                _player2.Call(),
                _player2.Call(Money.Create(200))
            };

            var pot = Pot.Create(bets);
            pot.Should().NotBeNull();
            pot.HighestBet.Value.Should().Be(200);
        }
    }
}
