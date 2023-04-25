using AGPoker.Entites.Game.Game.Players;

namespace AGPoker.Entites.Game.Turns
{
    public class TurnResult
    {
        private TurnResult (TurnStatus status, List<Player> playersStayedInGame)
        {
            Validation(playersStayedInGame);
            Status = status;
            WinnerPlayers = playersStayedInGame;
        }

        private TurnResult(TurnStatus status)
        {
            Status = status;
        }

        public TurnStatus Status { get; init; }
        public List<Player> WinnerPlayers { get; init; }

        public static TurnResult InProgress()
            => new(TurnStatus.InProgress);

        public static TurnResult Winners(List<Player> playersStayedInGame)
            => new(TurnStatus.Winners, playersStayedInGame);

        private void Validation(List<Player> playersStayedInGame)
        {
            if(playersStayedInGame is null || playersStayedInGame.Count == 0)
                throw new ArgumentException(nameof(playersStayedInGame));
        }
    }

    public enum TurnStatus
    {
        InProgress,
        Winners
    }
}
