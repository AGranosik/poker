﻿namespace AGPoker.Entites.Game.ValueObjects
{
    internal class GameLimit
    {
        public int Limit { get; init; }
        public GameLimit(int limit)
        {
            Validation(limit);
            Limit = limit;
        }

        private void Validation(int limit)
        {
            if (limit <= 1)
                throw new Exception("Number of players cannot be lower than 2");
        }
    }
}
