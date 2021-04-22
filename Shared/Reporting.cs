using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentDateTime;


namespace daedalus.Shared.Model
{
    public enum PeriodType
    {
        [Description("Today")]
        Today = 1,

        [Description("Yesterday")]
        Yesterday = 3,

        [Description("Last 7 Days")]
        LastSeven = 4,

        [Description("Current Month")]
        CurrentMonth = 5,

        [Description("Last Month")]
        PreviousMonth = 6,

        [Description("Current Year")]
        CurrentYear = 7,

        [Description("Last Year")]
        PreviousYear = 8
    }

    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }

    public class Period
    {
        public PeriodType PeriodType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    static public class Reporting
    {
        static public List<Period> GetReportPeriods()
        {

            DateTime todayStart = DateTime.UtcNow.BeginningOfDay();
            DateTime todayEnd = DateTime.UtcNow.EndOfDay();

            DateTime yesterdayStart = DateTime.UtcNow.PreviousDay().BeginningOfDay();
            DateTime yesterdayEnd = DateTime.UtcNow.PreviousDay().EndOfDay();

            DateTime lastSevenStart = DateTime.UtcNow.Subtract(new TimeSpan(7, 0, 0, 0)).BeginningOfDay();
            DateTime lastSevenEnd = DateTime.UtcNow.EndOfDay();

            DateTime currentMonthStart = DateTime.UtcNow.FirstDayOfMonth().BeginningOfDay();
            DateTime currentMonthEnd = DateTime.UtcNow.LastDayOfMonth().EndOfDay();

            DateTime lastMonthStart = DateTime.UtcNow.PreviousMonth().FirstDayOfMonth().BeginningOfDay();
            DateTime lastMonthEnd = DateTime.UtcNow.PreviousMonth().LastDayOfMonth().EndOfDay();

            DateTime currentYearStart = DateTime.UtcNow.FirstDayOfYear().BeginningOfDay();
            DateTime currentYearEnd = DateTime.UtcNow.LastDayOfYear().EndOfDay();

            DateTime previousYearStart = DateTime.UtcNow.PreviousYear().FirstDayOfYear().BeginningOfDay();
            DateTime previousYearEnd = DateTime.UtcNow.PreviousYear().LastDayOfYear().EndOfDay();

            List<Period> periods = new List<Period>()
                {
                    new Period()
                    {
                        PeriodType = PeriodType.Today,
                        Start = todayStart,
                        End = todayEnd,
                    },
                    new Period()
                    {
                        PeriodType = PeriodType.Yesterday,
                        Start = yesterdayStart,
                        End = yesterdayEnd
                    },
                    new Period()
                    {
                        PeriodType = PeriodType.LastSeven,
                        Start = lastSevenStart,
                        End = lastSevenEnd
                    },
                    new Period()
                    {
                        PeriodType = PeriodType.CurrentMonth,
                        Start = currentMonthStart,
                        End = currentMonthEnd
                    },
                    new Period()
                    {
                        PeriodType = PeriodType.PreviousMonth,
                        Start = lastMonthStart,
                        End = lastMonthEnd
                    },
                    new Period()
                    {
                        PeriodType = PeriodType.CurrentYear,
                        Start = currentYearStart,
                        End = currentYearEnd
                    },
                    new Period()
                    {
                        PeriodType = PeriodType.PreviousYear,
                        Start = previousYearStart,
                        End = previousYearEnd
                    }
                };

            return periods;
        }
    }

}
