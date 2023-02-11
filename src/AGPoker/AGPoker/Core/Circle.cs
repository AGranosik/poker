namespace AGPoker.Core
{
    public static class Circle
    {
        public static T GetPrevious<T>(T cuurentElement, List<T> list, int moveBack)
        {
            InputDataValidation(cuurentElement, list);
            NumberOfMovesValidation(moveBack);

            var index = GetCurrentElementIndex(cuurentElement, list);
            var previousIndex = GetPreviousIndex(index, list);

            for(int i =1; i < moveBack; i++)
                previousIndex = GetPreviousIndex(previousIndex, list);

            return list[previousIndex];
        }

        public static T GetNextInCircle<T>(T currentElement, List<T> list)
        {
            InputDataValidation(currentElement, list);
            var index = GetCurrentElementIndex(currentElement, list);
            if (index == list.Count - 1)
            {
                return list[0];
            }
            else
            {
                return list[index + 1];
            }
        }

        private static int GetPreviousIndex<T>(int currentIndex, List<T> list)
        {
            var lastIndex = list.Count - 1;
            if(currentIndex == 0)
                return lastIndex;

            return currentIndex - 1;
        }

        private static int GetCurrentElementIndex<T>(T currentElement, List<T> list)
        {
            var index = list.IndexOf(currentElement);
            if (index == -1)
                throw new ArgumentNullException();

            return index;
        }

        private static void InputDataValidation<T>(T currentElement, List<T> list)
        {
            if (list is null)
                throw new ArgumentNullException();
        }

        private static void NumberOfMovesValidation(int moveBack)
        {
            if (moveBack <= 0)
                throw new ArgumentException();
        }
    }
}
