using AGPoker.Core.Exceptions;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Stacks.ValueObjects
{
    [TestFixture]
    internal class Pot_FoldTests
    {
        private Player _player;
        private Pot _pot;

        [SetUp]
        public void SetUp()
        {
            _player = Player.Create("hehe", "eheh");
            _pot = Pot.Create();
        }


        [Test]
        public void Fold_IsAFoldBet_ThrowsException()
        {
            var bet = _player.Call();
            var func = () => _pot.Fold(bet);
            func.Should().Throw<NotProperKindOfBetException>();
        }

        [Test]
        public void Fold_PlayerFoldedBefore_ThrowsException()
        {
            var bet = _player.Fold();
            _pot.Fold(bet);
            var func = () => _pot.Fold(bet);
            func.Should().Throw<PlayerFoldedBeforeException>();
        }

        [Test]
        public void Fold_CanFoldDespiteArlierBets_Success()
        {
            _pot.Raise(Bet.Create(Money.Create(20), _player, BetType.Raise));
            var bet = _player.Fold();
            var func = () => _pot.Fold(bet);
            func.Should().NotThrow();
        }
    }
}
