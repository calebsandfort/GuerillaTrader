using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuerillaTrader.Services
{
    /// <summary>
    /// Time helper class collection for working with trading dates
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// Provides a value far enough in the future the current computer hardware will have decayed :)
        /// </summary>
        /// <value>
        /// new DateTime(2050, 12, 31)
        /// </value>
        public static readonly DateTime EndOfTime = new DateTime(2050, 12, 31);

        /// <summary>
        /// Provides a value far enough in the past that can be used as a lower bound on dates
        /// </summary>
        /// <value>
        /// DateTime.FromOADate(0)
        /// </value>
        public static readonly DateTime BeginningOfTime = DateTime.FromOADate(0);

        /// <summary>
        /// Provides a value large enough that we won't hit the limit, while small enough
        /// we can still do math against it without checking everywhere for <see cref="TimeSpan.MaxValue"/>
        /// </summary>
        public static readonly TimeSpan MaxTimeSpan = TimeSpan.FromDays(1000 * 365);

        /// <summary>
        /// One Day TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneDay = TimeSpan.FromDays(1);

        /// <summary>
        /// One Hour TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneHour = TimeSpan.FromHours(1);

        /// <summary>
        /// One Minute TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneMinute = TimeSpan.FromMinutes(1);

        /// <summary>
        /// One Second TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        /// <summary>
        /// One Millisecond TimeSpan Period Constant
        /// </summary>
        public static readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);

        /// <summary>
        /// Live charting is sensitive to timezone so need to convert the local system time to a UTC and display in browser as UTC.
        /// </summary>
        public struct DateTimeWithZone
        {
            private readonly DateTime utcDateTime;
            private readonly TimeZoneInfo timeZone;

            /// <summary>
            /// Initializes a new instance of the <see cref="QuantConnect.Time.DateTimeWithZone"/> struct.
            /// </summary>
            /// <param name="dateTime">Date time.</param>
            /// <param name="timeZone">Time zone.</param>
            public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
            {
                utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
                this.timeZone = timeZone;
            }

            /// <summary>
            /// Gets the universal time.
            /// </summary>
            /// <value>The universal time.</value>
            public DateTime UniversalTime { get { return utcDateTime; } }

            /// <summary>
            /// Gets the time zone.
            /// </summary>
            /// <value>The time zone.</value>
            public TimeZoneInfo TimeZone { get { return timeZone; } }

            /// <summary>
            /// Gets the local time.
            /// </summary>
            /// <value>The local time.</value>
            public DateTime LocalTime
            {
                get
                {
                    return TimeZoneInfo.ConvertTime(utcDateTime, timeZone);
                }
            }
        }

        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// Create a C# DateTime from a UnixTimestamp
        /// </summary>
        /// <param name="unixTimeStamp">Double unix timestamp (Time since Midnight Jan 1 1970)</param>
        /// <returns>C# date timeobject</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime time;
            try
            {
                // Unix timestamp is seconds past epoch
                time = EpochTime.AddSeconds(unixTimeStamp);
            }
            catch (Exception err)
            {
                //Log.Error(err, "UnixTimeStamp: " + unixTimeStamp);
                time = DateTime.Now;
            }
            return time;
        }

        /// <summary>
        /// Create a C# DateTime from a UnixTimestamp
        /// </summary>
        /// <param name="unixTimeStamp">Double unix timestamp (Time since Midnight Jan 1 1970) in milliseconds</param>
        /// <returns>C# date timeobject</returns>
        public static DateTime UnixMillisecondTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime time;
            try
            {
                // Unix timestamp is seconds past epoch
                time = EpochTime.AddMilliseconds(unixTimeStamp);
            }
            catch (Exception err)
            {
                //Log.Error(err, "UnixTimeStamp: " + unixTimeStamp);
                time = DateTime.Now;
            }
            return time;
        }

        /// <summary>
        /// Convert a Datetime to Unix Timestamp
        /// </summary>
        /// <param name="time">C# datetime object</param>
        /// <returns>Double unix timestamp</returns>
        public static double DateTimeToUnixTimeStamp(DateTime time)
        {
            double timestamp = 0;
            try
            {
                timestamp = (time - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            }
            catch (Exception err)
            {
                //Log.Error(err, time.ToString("o"));
            }
            return timestamp;
        }

        /// <summary>
        /// Get the current time as a unix timestamp
        /// </summary>
        /// <returns>Double value of the unix as UTC timestamp</returns>
        public static double TimeStamp()
        {
            return DateTimeToUnixTimeStamp(DateTime.UtcNow);
        }

        /// <summary>
        /// Returns the timespan with the larger value
        /// </summary>
        public static TimeSpan Max(TimeSpan one, TimeSpan two)
        {
            return TimeSpan.FromTicks(Math.Max(one.Ticks, two.Ticks));
        }
        /// <summary>
        /// Returns the timespan with the smaller value
        /// </summary>
        public static TimeSpan Min(TimeSpan one, TimeSpan two)
        {
            return TimeSpan.FromTicks(Math.Min(one.Ticks, two.Ticks));
        }

        /// <summary>
        /// Returns the larger of two date times
        /// </summary>
        public static DateTime Max(DateTime one, DateTime two)
        {
            return one > two ? one : two;
        }

        /// <summary>
        /// Returns the smaller of two date times
        /// </summary>
        public static DateTime Min(DateTime one, DateTime two)
        {
            return one < two ? one : two;
        }
    }
}
