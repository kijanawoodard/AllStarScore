namespace AllStarScore.Admin.Infrastructure.Utilities
{
    public static class StringExtensions
    {
        public static string TrimSafely(this string target)
        {
            return (target ?? string.Empty).Trim();
        }
    }
}