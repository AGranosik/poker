using AGPoker.Aggregates;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.ValueObjects;
using NUnit.Framework;

namespace AGPoker.Tests.Domain.Aggregates
{
    [TestFixture]
    internal class Game_GivingCardsTests
    {
        private Game _game;
        private List<Player> _players;

        [SetUp]
        public void SetUp()
        {
            var owner = Player.Create("fiu", "fiu");
            _game = Game.Create(owner, new GameLimit(5));
            _players = new List<Player>
            {
                Player.Create("hehe6", "hehe2"),
                Player.Create("hehe7", "hehe2"),
                Player.Create("hehe3", "hehe2"),
                Player.Create("hehe4", "hehe2"),
                Player.Create("hehe5", "hehe2")
            };
            AddPlayersToGame();
            _game.Begin();
        }

        [Test]
        public void NextRound_AllIntoNextRound_Success()
        {

        }

        private void AddPlayersToGame()
        {
            foreach (var player in _players)
            {
                _game.Join(player);
            }
        }
    }
}
