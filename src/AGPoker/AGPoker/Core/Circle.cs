namespace AGPoker.Core
{
    public static class Circle
    {
        public static T GetNextInCircle<T>(T currentElement, List<T> list)
        {
            var index = list.IndexOf(currentElement);
            if (index == list.Count - 1)
            {
                return list[0];
            }
            else
            {
                return list[index + 1];
            }
        }
    }
}
