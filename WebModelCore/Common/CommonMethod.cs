using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using WebCore.Entities;

namespace WebModelCore.Common
{
    public static class CommonMethod
    {
        public static CultureInfo CultureApp = CultureInfo.CurrentCulture;
        public static decimal[] ToDecimalArray(this string value)
        {
            char[] charSeparator = { ',', ';' };
            return value.Trim(charSeparator).Split(charSeparator).Select(x => Convert.ToDecimal(x)).ToArray();
        }
        public static long[] ToLongArray(this string value)
        {
            if (value == null)
            {
                return new List<long>().ToArray();
            }
            char[] charSeparator = { ',', ';' };
            return value.Trim(charSeparator).Split(charSeparator).Select(x => Convert.ToInt64("0" + x)).ToArray();
        }

        public static IEnumerable<string> ToStringArray(this string value, char charSeparator)
        {
            if (string.IsNullOrEmpty(value))
                return new List<string>();
            return value.Trim().Split(charSeparator).Select(x => x).ToArray();
        }
        public static IEnumerable<string> ToStringArrayUpper(this string value, char charSeparator)
        {
            if (string.IsNullOrEmpty(value))
                return new List<string>();
            return value.Trim().Split(charSeparator).Select(x => x.ToUpper()).ToArray();
        }
        /// <summary>
        /// convert string to date with formats { "yyyyMMdd", "dd/MM/yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss"}
        /// </summary>
        /// <param name="strDate">string date</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(this string strDate)
        {
            string[] formats = { "yyyyMMdd", "dd/MM/yyyy", "dd-MM-yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss" };
            if (!string.IsNullOrEmpty(strDate))
            {
                try
                {
                    var date = DateTime.ParseExact(strDate, formats, CultureApp, DateTimeStyles.None);
                    return date;
                }
                catch (Exception e)
                {
                    //
                }
            }
            return DateTime.Now;
        }
        /// <summary>
        /// convert string to date with formats { "yyyyMMdd", "dd/MM/yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss"}
        /// </summary>
        /// <param name="strDate">string date</param>
        /// <returns></returns>
        public static DateTime StringToDateTimeByFomat(this string strDate,string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "dd/MM/yyyy";
            }
            string[] formats = { format};
            if (!string.IsNullOrEmpty(strDate))
            {
                try
                {
                    var date = DateTime.ParseExact(strDate, formats, CultureApp, DateTimeStyles.None);
                    return date;
                }
                catch (Exception e)
                {
                    //
                }
            }
            return DateTime.Now;
        }
        /// <summary>
        /// convert string to date with formats { "yyyyMMdd", "dd/MM/yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss"}
        /// </summary>
        /// <param name="strDate">string date</param>
        /// <returns></returns>
        public static DateTime StringToDateTimeMinValue(this string strDate)
        {
            string[] formats = { "yyyyMMdd", "dd/MM/yyyy", "dd-MM-yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss" };
            if (!string.IsNullOrEmpty(strDate))
            {
                try
                {
                    var date = DateTime.ParseExact(strDate, formats, CultureApp, DateTimeStyles.None);
                    return date;
                }
                catch (Exception e)
                {
                    //
                }
            }
            return new DateTime(1900, 1, 1);
        }
        /// <summary>
        /// convert string to date with formats { "yyyyMMdd", "dd/MM/yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss"}
        /// </summary>
        /// <param name="strDate">string date</param>
        /// <returns></returns>
        public static DateTime StringToDateTimeMaxValue(this string strDate)
        {
            string[] formats = { "yyyyMMdd", "dd/MM/yyyy", "dd-MM-yyyy", "yyyyMM", "yyyyMMddHHmmss", "dd/MM/yyyy HH:mm:ss" };
            if (!string.IsNullOrEmpty(strDate))
            {
                try
                {
                    var date = DateTime.ParseExact(strDate, formats, CultureApp, DateTimeStyles.None);
                    return date;
                }
                catch (Exception e)
                {
                    //
                }
            }
            return new DateTime(2900, 1, 1);
        }
        /// <summary>
        /// Trả lại ID Lỗi để tìm lỗi trong bảng DEFERROR
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static int GetError(this string error)
        {
            try
            {
                var err = error.Split("-->");
                //var xml= XmlSerializer()
                XmlDocument x = new XmlDocument();
                x.LoadXml(err[1]);
                return Convert.ToInt32(x.FirstChild.Attributes[0].Value);
            }
            catch (Exception ex)
            {

            }
            return -1;

        }
        public static string GetLanguageByText(this string text, List<LanguageInfo> languageInfos)
        {
            try
            {
                var lang = languageInfos.Where(x => x.LanguageName.ToLower() == text.ToLower() && (x.LanguageID == "VI" || x.LanguageID == "VN"));
                if (lang.Any())
                {
                    return lang.First().LanguageValue;
                }
            }
            catch (Exception ex)
            {

            }
            return "Không tìm thấy language"+text;

        }
        public static string GetLanguageTitle(this string text, List<LanguageInfo> languageInfos)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    text = text.Trim();
                }
                var textTitle = text + ".title";
                var lang = languageInfos.Where(x => x.LanguageName.ToLower() == textTitle.ToLower() && (x.LanguageID == "VI" || x.LanguageID == "VN"));
                if (lang.Any())
                {
                    return lang.First().LanguageValue;
                }
            }
            catch (Exception ex)
            {

            }
            return text;
        }
        public static int PageSize = 20;

    }
    public static class ListExtensions
    {
        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

}
