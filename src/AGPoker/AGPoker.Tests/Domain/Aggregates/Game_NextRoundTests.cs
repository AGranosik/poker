using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class Game_NextRoundTests
    {
        private Game _game;
        private List<Player> _players;
        private Player _dealer;
        private Player _smallBlind;
        private Player _bigBlind;

        [SetUp]
        public void Setup()
        {
            var owner = Player.Create("fiu", "fiu");
            _game = Game.Create(owner, new GameLimit(5));
            _dealer = Player.Create("hehe3", "hehe2");
            _smallBlind = Player.Create("hehe4", "hehe2");
            _bigBlind = Player.Create("hehe5", "hehe2");
            _players = new List<Player>
            {
                Player.Create("hehe6", "hehe2"),
                Player.Create("hehe7", "hehe2"),
                _dealer,
                _smallBlind,
                _bigBlind
            };
            AddPlayersToGame();
            _game.Begin();
        }

        [Test]
        public void RoundFinished_AllCalls_Success()
        {
            AllPlayersCalled();
            AllPlayersCalled();
            AllPlayersCalled();
            _game.Call(_players[1]);
            _game.Call(_players[2]);
            _game.Call(_players[3]);
            _game.Call(_players[4]);

            var tableWinners = _game.Table.GetWinners(_players);
            _game.Call(_players[0]);

            var currentDealer = _game.Turn.Dealer;
            currentDealer.Should().Be(_smallBlind);

            var table = _game.Table;
            table.Flop.Should().Be(null);
            table.Turn.Should().Be(null);
            table.River.Should().Be(null);

            CheckWinners(tableWinners, Money.Create(100), Money.Create(500));
        }

        private void CheckWinners(IReadOnlyCollection<Player> tableWinners, Money winningPrize, Money startingMoney)
        {
            var winningPrizePerWinner = Money.Create(winningPrize.Value / tableWinners.Count);
            tableWinners.All(t => t.Money - startingMoney == winningPrize)
                .Should().BeTrue();
        }
        private void AddPlayersToGame()
        {
            foreach (var player in _players)
            {
                _game.Join(player);
            }
        }

        private void AllPlayersCalled()
        {
            _game.Call(_players[1]);
            _game.Call(_players[2]);
            _game.Call(_players[3]);
            _game.Call(_players[4]);
            _game.Call(_players[0]);
        }
    }
}
