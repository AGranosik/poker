using AGPoker.Entites.Game.Decks.ValueObjects;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Tables;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Entites.Game.Tables
{
    [TestFixture]
    internal class TableTests
    {
        private List<Player> _players;

        [SetUp]
        public void SetUp()
        {
            _players = new List<Player>
            {
                Player.Create("hehe6", "hehe2"),
                Player.Create("hehe7", "hehe2"),
                Player.Create("hehe3", "hehe2"),
                Player.Create("hehe4", "hehe2"),
                Player.Create("hehe5", "hehe2")
            };
        }

        [Test]
        public void PreFlop_PlayersHasUniqueCards_Success()
        {
            var table = Table.PreFlop(_players);
            table.Should().NotBeNull();
            AllPlayersHasUniqueNumberOfCards(2);
            AllCardsAreUnique(_players.SelectMany(p => p.Cards).ToList());
        }

        [Test]
        public void Flop_PlayersHasUniqueCards_Success()
        {
            var table = Table.PreFlop(_players);
            table.Should().NotBeNull();

            table.NextStage();
            table.Flop.Should().NotBeNull();
            AllCardsAreUnique(table.Flop.Cards);
            var usedCards = _players.SelectMany(p => p.Cards).ToList();
            usedCards.AddRange(table.Flop.Cards);
            AllCardsAreUnique(usedCards);
            table.Flop.Cards.Count.Should().Be(3);
        }

        [Test]
        public void Turn_PlayersHasUniqueCards_Success()
        {
            var table = Table.PreFlop(_players);
            table.Should().NotBeNull();

            table.NextStage();
            table.NextStage();
            table.Turn.Should().NotBeNull();

            var usedCards = _players.SelectMany(p => p.Cards).ToList();
            usedCards.AddRange(table.Flop.Cards);
            usedCards.AddRange(table.Turn.Cards);
            AllCardsAreUnique(usedCards);
            table.Turn.Cards.Count.Should().Be(1);
        }

        [Test]
        public void River_PlayersHasUniqueCards_Success()
        {
            var table = Table.PreFlop(_players);
            table.Should().NotBeNull();

            table.NextStage();
            table.NextStage();
            table.NextStage();
            table.River.Should().NotBeNull();

            var usedCards = _players.SelectMany(p => p.Cards).ToList();
            usedCards.AddRange(table.Flop.Cards);
            usedCards.AddRange(table.Turn.Cards);
            usedCards.AddRange(table.River.Cards);
            AllCardsAreUnique(usedCards);

            table.River.Cards.Count.Should().Be(1);
        }

        [Test]
        public void AfterRiver_CannotPastRiver_ThrowsException()
        {
            var table = Table.PreFlop(_players);
            table.Should().NotBeNull();

            table.NextStage();
            table.NextStage();
            table.NextStage();
            var func = () => table.NextStage();
            func.Should().Throw<InvalidOperationException>();
        }

        private void AllPlayersHasUniqueNumberOfCards(int n)
        {
            _players.All(p => p.Cards.Count == n && p.Cards.Distinct().Count() == p.Cards.Count)
                .Should().BeTrue();
        }

        private void AllCardsAreUnique(List<Card> cards)
        {
            cards.Count().Should().Be(cards.Distinct().Count());
        }
    }
}
