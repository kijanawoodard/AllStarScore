namespace AllStarScore.Extensions
{
    public static class StringExtensions
    {
        public static string TrimSafely(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

		public static string StripCompanyId(this string target)
		{
			var working = target.TrimSafely();

			if (working.StartsWith("company/"))
			{
				var index = working.IndexOf("/", "company/".Length, System.StringComparison.Ordinal);
				working = working.Substring(index + 1);
			}

			return working;
		}

		public static string ExtractCompetitionId(this string target)
		{
			var working = target.TrimSafely();
			var start = working.IndexOf("competitions/", System.StringComparison.Ordinal);
			if (start < 0) return target;

			var index = working.IndexOf("/", start + "competitions/".Length, System.StringComparison.Ordinal);
			if (index < 0) return target;

			working = working.Substring(0, index);
			return working;
		}

        public static string ForMvc(this string target)
        {
            //strip company id; will put it back in the model binder - reduces security surface dramatically
        	var working = target.StripCompanyId();
        	return working.ForScoringMvc();
        }

		public static string ForScoringMvc(this string target)
		{
			return target
					.Replace("/", "-");
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