using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SILDMS.Utillity
{
    public static class DataValidation
    {
        public static string TrimmedOrDefault(string def)
        {
            if (string.IsNullOrEmpty(def))
            {
                return "";
            }
            else
            {
               return def.Trim();
            }
        }

        public static int ReturnMonthCode(string name)
        {
            switch (name)
            {
                case "Jan":
                    return 1;

                case "Feb":
                    return 2;

                case "Mar":
                    return 3;

                case "Apr":
                    return 4;

                case "May":
                    return 5;

                case "Jun":
                    return 6;

                case "Jul":
                    return 7;

                case "Aug":
                    return 8;

                case "Sep":
                    return 9;

                case "Oct":
                    return 10;

                case "Nov":
                    return 11;

                case "Dec":
                    return 12;

                default:
                    return 0;

            }
        }

        public static DateTime DateTimeConversion(string date)
        {
            var newDate = new DateTime();
            if (string.IsNullOrEmpty(date)) return newDate;
            var strDatepart = date.Substring(0, date.Length);
            if (strDatepart.Contains('/'))
            {
                var str = strDatepart.Split('/');
                newDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[1]),
                    Convert.ToInt32(str[0]));
            }
            else if (strDatepart.Contains('-'))
            {
                var str = strDatepart.Split('-');
                newDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[1]),
                    Convert.ToInt32(str[0]));
            }
            else
            {
                var str = Regex.Split(date, @"\s+");
                newDate = new DateTime(Convert.ToInt32(str[2]), ReturnMonthCode(str[1]),
                   Convert.ToInt32(str[0]));

            }
            return newDate;
        }
    }
}
