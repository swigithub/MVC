using System;
using System.Globalization;

namespace SWI.Libraries.Common
{
    public class Conversion
    {
        /*----MoB!----*/
        public int Int32(string value) {
            try
            {
                return (!string.IsNullOrEmpty(value)) ? Convert.ToInt32(double.Parse(value)) : 0;
            }
            catch
            {

                return 0;
            }
        }

        public Int64 Int64(string value)
        {
            try
            {
                return (!string.IsNullOrEmpty(value)) ? Convert.ToInt64(value) : 0;
            }
            catch
            {

                return 0;
            }
        }

        public double Double(string value)
        {
            try
            {
                return (!string.IsNullOrEmpty(value)) ? Convert.ToDouble(value) : 0;
            }
            catch
            {

                return 0;
            }
        }

        public bool Bool(string value)
        {
            try
            {
                
                return Convert.ToBoolean(value);
                
            }
            catch 
            {

                return false;
            }
        }

       
             
    }

    public class WeekDays
    {
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
     
    }
}