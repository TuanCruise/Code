using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebAppCoreBlazorServer.Common
{
    public static class StringHelper
    {
        public static string ToUrlFriendly(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }
            name = name.ToLower();
            name = RemoveDiacritics(name);
            name = ConvertEdgeCases(name);
            name = name.Replace(" ", "-");
            name = name.Strip(c =>
                c != '-'
                && c != '_'
                && !c.IsLetter()
                && !Char.IsDigit(c)
            );

            while (name.Contains("--"))
                name = name.Replace("--", "-");

            if (name.Length > 200)
                name = name.Substring(0, 200);

            return name;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool IsLetter(this char c)
        {
            return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
        }

        public static bool IsSpace(this char c)
        {
            return (c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == ' ');
        }

        public static string Strip(this string subject, params char[] stripped)
        {
            if (stripped == null || stripped.Length == 0 || String.IsNullOrEmpty(subject))
            {
                return subject;
            }

            var result = new char[subject.Length];

            var cursor = 0;
            for (var i = 0; i < subject.Length; i++)
            {
                char current = subject[i];
                if (Array.IndexOf(stripped, current) < 0)
                {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        public static string Strip(this string subject, Func<char, bool> predicate)
        {

            var result = new char[subject.Length];

            var cursor = 0;
            for (var i = 0; i < subject.Length; i++)
            {
                char current = subject[i];
                if (!predicate(current))
                {
                    result[cursor++] = current;
                }
            }

            return new string(result, 0, cursor);
        }

        private static string ConvertEdgeCases(string text)
        {
            var sb = new StringBuilder();
            foreach (var c in text)
            {
                sb.Append(ConvertEdgeCases(c));
            }

            return sb.ToString();
        }

        private static string ConvertEdgeCases(char c)
        {
            string swap;
            switch (c)
            {
                case 'ı':
                    swap = "i";
                    break;
                case 'ł':
                case 'Ł':
                    swap = "l";
                    break;
                case 'đ':
                    swap = "d";
                    break;
                case 'ß':
                    swap = "ss";
                    break;
                case 'ø':
                    swap = "o";
                    break;
                case 'Þ':
                    swap = "th";
                    break;
                default:
                    swap = c.ToString();
                    break;
            }

            return swap;
        }


        public static string ToFriendlyUrl(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            var strTitle = str.ToLower();
            var rgx = new Regex("[~?!@#$%^&*()_+/|\\/><\\]\\[.,'\";:-]");

            var result = ReplaceCharacterSpecial(strTitle);
            var strRemoveVn = RemoveSign4VietnameseString(result);
            result = rgx.Replace(strRemoveVn, " ").Trim().Replace(' ', '-');

            return result.Contains("--") ? result.Replace("--", "-") : result;
        }
        public static string RemoveSign4VietnameseString(string str)
        {
            //remove 
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return str;

        }
        private static readonly string[] VietnameseSigns = {
            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string ReplaceCharacterSpecial(string input)
        {
            const string strRegex = "”,“,–,&"; // array character need remove
            if (string.IsNullOrEmpty(input))
                return input;

            var arrayRegex = strRegex.Split(',');
            if (arrayRegex.Length == 0)
                return input;

            foreach (var item in arrayRegex.Where(item => input != null && input.Contains(item)))
            {
                input = input.Replace(item, "");
            }
            return input;
        }
    }
}
