using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Tables;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class Table_GiveAwayAllTests
    {
        private List<Player> _players;
        private Table _table;

        [SetUp]
        public void SetUp()
        {
            _players = new List<Player>()
            {
                Player.Create("hehe", "hehe"),
                Player.Create("hehe2", "hehe"),
                Player.Create("hehe3", "hehe"),
                Player.Create("hehe4", "hehe"),
            };
            _table = Table.PreFlop(_players);
        }
        [Test]
        public void GiveAway_PlayersCannotBeNUll_ThrwosException()
        {
            var exceptionFunc = () => _table.GiveAwayAllneccessaryCards(null);
            exceptionFunc.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GiveAway_PlayersCannotBeEmpty_ThrowsException()
        {
            var exceptionFunc = () => _table.GiveAwayAllneccessaryCards(new List<Player>());
            exceptionFunc.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GiveAway_PlayersHaveToBeInGame_ThrowsException()
        {
            var morePlayers = new List<Player>(_players)
            {
                Player.Create("giu", "asdasd")
            };

            var exceptionFunc = () => _table.GiveAwayAllneccessaryCards(morePlayers);
            exceptionFunc.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GiveAway_RightAfterStart_Success()
        {
            var result = _table.GiveAwayAllneccessaryCards(_players);
            result.Should().NotBeNullOrEmpty();

            TableCheck();
        }

        [Test]
        public void GiveAway_AfterPreFlop_Success()
        {
            _table.NextStage();

            var result = _table.GiveAwayAllneccessaryCards(_players);
            result.Should().NotBeNullOrEmpty();

            TableCheck();
        }

        [Test]
        public void GiveAway_AfterFlop_Success()
        {
            _table.NextStage();
            _table.NextStage();

            var result = _table.GiveAwayAllneccessaryCards(_players);
            result.Should().NotBeNullOrEmpty();

            TableCheck();
        }

        [Test]
        public void GiveAway_AfterTurn_Success()
        {
            _table.NextStage();
            _table.NextStage();
            _table.NextStage();

            var result = _table.GiveAwayAllneccessaryCards(_players);
            result.Should().NotBeNullOrEmpty();

            TableCheck();
        }

        private void TableCheck()
        {
            _players.All(p => p.Cards.Count == 2)
                .Should().BeTrue();

            _table.Flop.Should().NotBeNull();
            _table.Turn.Should().NotBeNull();
            _table.River.Should().NotBeNull();

        }
    }
}
