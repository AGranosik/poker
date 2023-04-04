using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Tables;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class Table_GetWinnersTests
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
        public void GetWinners_PlayersCannotbeNull_ThrowsException()
        {
            var func = () => _table.GetWinners(null);
            func.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GetWinners_PlayersCannotBeEmpty_ThrowsException()
        {
            var func = () => _table.GetWinners(new List<Player>());
            func.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GetWinners_NotSamePlayers_ThrowsException()
        {
            var player = Player.Create("sjdfghjdfshjgk", "asdasd");
            var func = () => _table.GetWinners(new List<Player> { player });
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetWinners_SamePlayersExceptOne_ThrowsException()
        {
            var players = new List<Player>()
            {
                Player.Create("hehe", "hehe"),
                Player.Create("hehe7", "hehe"),
                Player.Create("hehe3", "hehe"),
                Player.Create("hehe4", "hehe"),
            };
            var func = () => _table.GetWinners(players);
            func.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetWinners_NotLastStage_ThrowsException()
        {
            var players = new List<Player>()
            {
                Player.Create("hehe", "hehe"),
                Player.Create("hehe7", "hehe"),
                Player.Create("hehe3", "hehe"),
                Player.Create("hehe4", "hehe"),
            };
            var func = () => _table.GetWinners(players);
            func.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GetWinners_SinglePlayer_Success()
        {
            var players = new List<Player>()
            {
                Player.Create("hehe", "hehe"),
                Player.Create("hehe7", "hehe"),
                Player.Create("hehe3", "hehe"),
                Player.Create("hehe4", "hehe"),
            };
        }

        private void TableIntoLastStage()
        {
            _table.NextStage();
            _table.NextStage();
            _table.NextStage();
        }
    }
}
