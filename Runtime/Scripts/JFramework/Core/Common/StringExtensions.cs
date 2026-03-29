using System;
using System.Text.RegularExpressions;

namespace JFramework
{
    public static class StringExtensions
    {
        /// <summary>
        /// 支持百分号自动乘100并格式化小数位的字符串扩展
        /// 用法: "A:{0:F1}% B:{1:0}% C:{2}".FormatPercent(0.9123, 0.876, "test")
        /// </summary>
        public static string FormatPercent(this string format, params object[] args)
        {
            var regex = new Regex(@"\{(\d+)(:[^}]*)?\}(%?)");
            int lastIndex = 0;
            var result = "";
            foreach (Match match in regex.Matches(format))
            {
                result += format.Substring(lastIndex, match.Index - lastIndex);

                int argIndex = int.Parse(match.Groups[1].Value);
                string customFormat = match.Groups[2].Success ? match.Groups[2].Value : "";
                bool hasPercent = match.Groups[3].Value == "%";

                object value = args[argIndex];
                if (hasPercent && value is IConvertible)
                {
                    value = Convert.ToDouble(value) * 100;
                }

                string formatted = customFormat != ""
                    ? string.Format("{0" + customFormat + "}", value is IConvertible ? Convert.ToDouble(value) : value)
                    : value.ToString();

                result += formatted;
                if (hasPercent) result += "%";

                lastIndex = match.Index + match.Length;
            }
            result += format.Substring(lastIndex);
            return result;
        }

        /// <summary>
        /// 将 long 型秒数格式化为倒计时时间字符串（xx小时xx分钟xx秒）
        /// 用法: 3661L.ToCountdownString() // "1小时1分钟1秒"
        /// </summary>
        public static string ToCountdownString(this long seconds, string hourUnit = "小时", string minuteUnit = "分钟", string secondUnit = "秒")
        {
            var ts = TimeSpan.FromSeconds(seconds);
            string result = "";

            if ((int)ts.TotalHours > 0)
                result += $"{(int)ts.TotalHours}{hourUnit}";
            if (ts.Minutes > 0)
                result += $"{ts.Minutes}{minuteUnit}";
            if (ts.Seconds > 0 || result == "") // 保证至少显示秒
                result += $"{ts.Seconds}{secondUnit}";

            return result;
        }

        /// <summary>
        /// 倒计时，格式为 xx:xx:xx 或 xx:xx 或 xx
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string ToCountdownString(this long seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            int hours = (int)ts.TotalHours;
            int minutes = ts.Minutes;
            int secs = ts.Seconds;

            if (hours > 0)
                return $"{hours:D2}:{minutes:D2}:{secs:D2}";
            if (minutes > 0)
                return $"{minutes:D2}:{secs:D2}";
            // 只显示秒
            return $"{secs:D2}";
        }
    }
}