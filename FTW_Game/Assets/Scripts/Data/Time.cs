using System;

namespace FTW.Data
{
    [Serializable]
    public class Time
    {
        public int Year;
        public int Month;
        public int Day;
        public int Hours;
        public int Minutes;
        public int Seconds;

        public void SetTime(DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            Hours = dateTime.Hour;
            Minutes = dateTime.Minute;
            Seconds = dateTime.Second;
        }

        private DateTime GetDateTime => new DateTime(Year, Month, Day, Hours, Minutes, Seconds);

        public int DifferenceInSeconds => (int)DateTime.Now.Subtract(GetDateTime).TotalSeconds;
    }
}
