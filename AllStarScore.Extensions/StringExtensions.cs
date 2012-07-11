namespace AllStarScore.Extensions
{
    public static class StringExtensions
    {
        public static string TrimSafely(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string ForMvc(this string target)
        {
            return target.TrimSafely().Replace("/", "-");
        }

        public static string FromMvc(this string target)
        {
            return target.TrimSafely().Replace("-", "/");
        }

        public static string FromMvc<T>(this T target)
        {
            return (target as string).FromMvc();
        }
    }
}