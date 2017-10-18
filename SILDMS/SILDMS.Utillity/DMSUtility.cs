using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SILDMS.Utillity
{
    public static class DMSUtility
    {
        public static DateTime FormatDate(string strDate)
        {
            var newDate = new DateTime();
            if (string.IsNullOrEmpty(strDate)) return newDate;
            var strDatepart = strDate.Substring(0, 10);
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
                var str = Regex.Split(strDate, @"\s+");
                newDate = new DateTime(Convert.ToInt32(str[2]), ReturnMonthCode(str[1]),
                   Convert.ToInt32(str[0]));

            }
            return newDate;
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
    }
}
