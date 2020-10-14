namespace ParallelsByThreads
{
    public static class Extensions
    {
        public static void Print(this string @string)
        {
            ResultsContainer.Attach(@string);
        }
    }
}