using AGPoker.Core;
using AGPoker.Entites.Game.Game.Players;
using AGPoker.Entites.Game.Stacks.ValueObjects;
using AGPoker.Entites.Game.Turns;

namespace AGPoker.Tests.Domain.Entites.Game.Turns
{
    internal static class TurnHelper
    {
        public static void EveryPlayerCall(Turn turn, int indexBeforeFirstPLayer, List<Player> players, bool startNextRound = true)
        {
            var playerCounts = players.Count;
            var playerIndexes = Enumerable.Range(0, playerCounts).ToList();
            int previousPlayerIndex = indexBeforeFirstPLayer;
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[Circle.GetNextInCircle(previousPlayerIndex, playerIndexes)];
                turn.Bet(player, BetType.Call);
                previousPlayerIndex = players.IndexOf(player);
            }

            if (startNextRound)
                turn.NextRound();
        }
    }
}
