using System;
using System.Collections.Generic;

namespace AllStarScore.Extensions
{
    public static class DateTimeExtensions
    {
		public static IEnumerable<DateTime> GetDateRange(this DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                yield return startDate.Date;
                startDate = startDate.AddDays(1);
            }
        }
    }
}