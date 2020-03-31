using System;
using System.Globalization;


namespace SWI.Libraries.Common
{
 public   class DateFunctions
    {
        
        public DateTime MonthStart { get; set; }
        public DateTime MonthEnd { get; set; }
        public bool IsThisMonth { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public int WeekOfYear { get; set; }
        public bool IsThisWeek { get; set; }
        
        public bool IsToday { get; set; }
        public bool IsThisYear { get; set; }
        public bool Yesterday { get; set; }
        public bool Tomorrow { get; set; }
        public DateTime Date { get; set; }

        public DateFunctions() {

        }

        public DateTime FromUnixTime(string unixTime)
        {
            try
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddMilliseconds(Convert.ToDouble(unixTime));
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public  DateFunctions(DateTime date, DayOfWeek startOfWeek=DayOfWeek.Sunday) {
            Date = date;
            WeekStart = fnWeekStart(date,startOfWeek);
            WeekEnd= fnWeekEnd(date, startOfWeek);
            WeekOfYear = fnWeekOfYear(date);
            IsThisWeek = fnIsThisWeek(date);

            MonthStart = fnMonthStart(date);
            MonthEnd = fnMonthEnd(date);
            IsThisMonth = fnIsThisMonth(date);

            IsToday = fnIsToday(date);
            IsThisYear = fnIsThisYear(date);
        }

        private DateTime fnWeekStart(DateTime date, DayOfWeek startOfWeek)
        {
            int diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-1 * diff).Date;
        }

        private DateTime fnWeekEnd(DateTime date, DayOfWeek startOfWeek)
        {
            return fnWeekStart(date, startOfWeek).AddDays(6);
        }

        private int fnWeekOfYear(DateTime date)
        {

            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private bool fnIsThisWeek(DateTime date)
        {
            int week = fnWeekOfYear(date);
            int Current = fnWeekOfYear(DateTime.Now);
            return (week == Current) ? true : false;
        }

        private bool fnIsToday(DateTime date)
        {
            return (date.Date==DateTime.Now.Date) ? true : false;
        }

        private DateTime fnMonthStart(DateTime date) {
            return  new DateTime(date.Year, date.Month, 1);
        }

        private DateTime fnMonthEnd(DateTime date)
        {
            return fnMonthStart(date).AddMonths(1).AddDays(-1);
        }
        private bool fnIsThisMonth(DateTime date)
        {
            return (date.Month == DateTime.Now.Month) ? true : false;
        }

        private bool fnIsThisYear(DateTime date)
        {
            return (date.Year == DateTime.Now.Year) ? true : false;
        }

    }
}
