using System;

namespace Insomnia
{
    /// <summary>
    /// The default behaviour of the program is to run indefinetly by instantiating this class and not doing anything else.
    /// The properties of this class can however be overriden to make the proigram behave differently.
    /// </summary>
    public class Interval
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private sealed class Constants
        {
            public static int DateTimeLength { get { return DATELENGTH + TIMELENGTH + 1; } }
            public const int DATELENGTH = 10;
            public const int TIMELENGTH = 8;
            public const char DATESEPARATOR = '-';
            public const char TIMESEPARATOR = ':';
            public const char DATETIMESEPARATOR = ' ';
        }
        /// <summary>
        /// Array which defines if the program should run only on specific days of the week.
        /// Ex. Set to {true,true,true,true,true,false,false} to only run on weekdays.
        /// </summary>
        public bool[] DaysOfWeek { get; set; }
        public int IntervalInMilliseconds { get; set; }
        /// <summary>
        /// This timespan defines how often the program should check if DateTime.Now is outside of range
        /// </summary>
        public TimeSpan PollForStopPoking { get; set; }

        public Interval()
        {
            this.SetStart(DateTime.Now);
            this.SetEnd(DateTime.MaxValue);
            this.SetDaysOfWeek(new bool[] { true, true, true, true, true, true, true });
            this.SetIntervalInMilliseconds(59);
            this.SetPollForStopPoking(new TimeSpan(0, 5, 0));
        }

        #region Set

        public void SetStart(DateTime startDate)
        {
            this.StartDate = startDate;
        }

        public void SetEnd(DateTime endDate)
        {
            this.EndDate = endDate;
        }

        public void SetDaysOfWeek(bool[] daysOfWeek)
        {
            if (daysOfWeek.Length != 7)
                throw new Exception("Days of week length not 7");
            else
                this.DaysOfWeek = daysOfWeek;
        }

        public void SetPollForStopPoking(TimeSpan timeSpan)
        {
            this.PollForStopPoking = timeSpan;
        }

        #region Set interval

        private int IntervalTrySetCounter { get; set; } = 0;
        private int IntervalTrySetMaxCount { get { return 1; } }
        public void SetIntervalInMilliseconds(int intervalInSeconds)
        {
            if (this.IntervalTrySetCounter < this.IntervalTrySetMaxCount && intervalInSeconds >= 59)
                this.IntervalInMilliseconds = (intervalInSeconds * 1000);

            this.IntervalTrySetCounter++;
        }

        #endregion

        #endregion

        #region Set from args

        public void SetDateTime(string dateTime, bool isStartDateNotEndDate)
        {
            if (!string.IsNullOrEmpty(dateTime) && dateTime.Contains(Constants.DATETIMESEPARATOR.ToString()) && dateTime.Length == Constants.DateTimeLength)
                if (isStartDateNotEndDate)
                    this.SetStart(
                        ParseDateTime(
                            dateTime.Split(Constants.DATESEPARATOR)[0], 
                            dateTime.Split(Constants.DATESEPARATOR)[1], 
                            isStartDateNotEndDate)
                        );
                else
                    this.SetEnd(
                        ParseDateTime(
                            dateTime.Split(Constants.DATESEPARATOR)[0], 
                            dateTime.Split(Constants.DATESEPARATOR)[1], 
                            isStartDateNotEndDate)
                        );
        }

        private DateTime ParseDateTime(string date, string time, bool isStartDateNotEndDate)
        {
            DateTime dateToSet = ParseDate(date, isStartDateNotEndDate);
            DateTime timeToSet = ParseTime(time, isStartDateNotEndDate);

            return new DateTime(
                dateToSet.Year, 
                dateToSet.Month, 
                dateToSet.Day, 
                timeToSet.Hour, 
                timeToSet.Minute, 
                timeToSet.Second
            );
        }

        private DateTime ParseDate(string date, bool isStartDateNotEndDate)
        {
            DateTime dateToSet = DateTime.MinValue;
            if (!string.IsNullOrEmpty(date) && date.Length == Constants.DATELENGTH && date.Contains(Constants.DATESEPARATOR.ToString()))
            {
                string[] dateParts = date.Split(Constants.DATESEPARATOR);
                if (dateParts.Length == 3 && IsNumeric(dateParts))
                    dateToSet = new DateTime(
                        Convert.ToInt32(dateParts[0]), 
                        Convert.ToInt32(dateParts[1]), 
                        Convert.ToInt32(dateParts[2])
                    );
            }
            else
                dateToSet = isStartDateNotEndDate ? DateTime.Now : DateTime.MaxValue;

            return dateToSet;
        }

        private DateTime ParseTime(string time, bool isStartDateNotEndDate)
        {
            DateTime timeToSet = DateTime.MinValue;
            if (!string.IsNullOrEmpty(time) && time.Length == Constants.TIMELENGTH && time.Contains(Constants.TIMESEPARATOR.ToString()))
            {
                string[] timeParts = time.Split(Constants.TIMESEPARATOR);
                if (timeParts.Length == 3 && IsNumeric(timeParts))
                    timeToSet = new DateTime(
                        Convert.ToInt32(timeParts[0]), 
                        Convert.ToInt32(timeParts[1]), 
                        Convert.ToInt32(timeParts[2])
                    );
            }
            else
                timeToSet = isStartDateNotEndDate ? DateTime.Now : DateTime.MaxValue;

            return timeToSet;
        }

        internal void SetDaysOfWeek(string daysOfWeek)
        {
            if (!string.IsNullOrEmpty(daysOfWeek) && daysOfWeek.Length == 7 && IsNumeric(daysOfWeek))
            {
                bool[] daysOfWeekArr = new bool[7];

                for (int i = 0; i < daysOfWeek.Length; i++)
                {
                    int internalDayOfWeek = (i == 6) ? 0 : (i + 1);
                    daysOfWeekArr[internalDayOfWeek] = (daysOfWeek[i] != '0');
                }

                this.SetDaysOfWeek(daysOfWeekArr);
            }
        }

        internal void SetIntervalInMilliseconds(string intervalInSeconds)
        {
            if (this.IsNumeric(intervalInSeconds))
            {
                int intervalInSecondsAsInt = Convert.ToInt32(intervalInSeconds);
                this.SetIntervalInMilliseconds(intervalInSecondsAsInt);
            }
        }

        internal void SetPollForStopPoking(string timespanSeparatedByColon)
        {
            if (timespanSeparatedByColon.Contains(":"))
            {
                string[] timespanParts = timespanSeparatedByColon.Split(':');
                
                int hour = -1;
                int minutes = -1;
                int seconds = -1;
                if (timespanParts.Length == 3)
                    for (int i = 0; i < timespanParts.Length; i++)
                        if (this.IsNumeric(timespanParts[i]))
                            if (i == 0)
                                hour = Convert.ToInt32(timespanParts[i]);
                            else if (i == 1)
                                minutes = Convert.ToInt32(timespanParts[i]);
                            else
                                seconds = Convert.ToInt32(timespanParts[i]);
                
                if (hour != -1 && minutes != -1 && seconds != -1)
                    this.SetPollForStopPoking(new TimeSpan(hour, minutes, seconds));
            }
        }

        #endregion

        private bool IsNumeric(string str)
        {
            int temp;
            return int.TryParse(str, out temp);
        }

        private bool IsNumeric(string[] strArr)
        {
            for (int i = 0; i < strArr.Length; i++)
                if (!IsNumeric(strArr[i]))
                    return false;
            return true;
        }
    }
}