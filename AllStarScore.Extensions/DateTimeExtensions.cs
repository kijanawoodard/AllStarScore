using System;
using System.Collections.Generic;

namespace AllStarScore.Extensions
{
    public static class DateTimeExtensions
    {
        public static IEnumerable<DateTime> GetDateRange(this DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}