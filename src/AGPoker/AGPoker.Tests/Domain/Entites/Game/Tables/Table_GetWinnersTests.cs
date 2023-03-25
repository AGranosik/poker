using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Tables;
using FluentAssertions;
using NuGet.Frameworks;
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
        public void GetWinners_NoCards_ThrowsException()
        {
            var func = () => _table.GetWinners();
            func.Should().Throw<InvalidOperationException>();
        }
    }
}
