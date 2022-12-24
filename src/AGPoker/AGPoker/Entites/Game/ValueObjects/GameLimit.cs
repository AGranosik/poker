﻿using AGPoker.Common.ValueObjects;

namespace AGPoker.Entites.Game.ValueObjects
{
    public class GameLimit : ValueObject // should have 
    {
        public int Limit { get; init; }
        public GameLimit(int limit)
        {
            Validation(limit);
            Limit = limit;
        }

        private void Validation(int limit)
        {
            if (limit <= 1 || limit > 14)
                throw new Exception("Number of players cannot be lower than 2");
        }
    }
}
