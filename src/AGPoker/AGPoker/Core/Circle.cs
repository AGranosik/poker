namespace AGPoker.Core
{
    public static class Circle
    {
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

        private static int GetCurrentElementIndex<T>(T currentElement, List<T> list)
        {
            var index = list.IndexOf(currentElement);
            if (index == -1)
                throw new ArgumentNullException();

            return index;
        }

        private static void InputDataValidation<T>(T currentElement, List<T> list)
        {
            if (currentElement is null)
                throw new ArgumentNullException();

            if (list is null)
                throw new ArgumentNullException();
        }
    }
}
