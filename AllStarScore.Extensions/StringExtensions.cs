namespace AllStarScore.Extensions
{
    public static class StringExtensions
    {
        public static string TrimSafely(this string target)
        {
            return (target ?? string.Empty).Trim();
        }
    }
}