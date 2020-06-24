using System;
using System.Globalization;

namespace WB.SystemLibrary
{
    #region DateFormate

    public class DateFormat
	{
		#region DMYToMDY

		public static string DMYToMDY(string dateString)
		{
			IFormatProvider culture = new CultureInfo("fr-FR", true);
			DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
			return date.ToString("MM/dd/yyyy");
		}

        public static string DMYToDMSY(string dateString)
        {
            IFormatProvider culture = new CultureInfo("fr-FR", true);
            DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
            return date.ToString("dd/MM/yy");
        }

        public static string StrDMY2DDMMYYY(string dateString)
        {
            dateString = dateString.Substring(0, 2) + "/" + dateString.Substring(2, 2) + "/" + dateString.Substring(4, 4);
            return dateString;
        }

        public static string StrYMD2DDMMYYY(string dateString)
        {
            dateString = dateString.Substring(6, 2) + "/" + dateString.Substring(4, 2) + "/" + dateString.Substring(0, 4);
            return dateString;
        }

		#endregion

		#region MDYToDMY

		public static string MDYToDMY(string dateString)
		{
			IFormatProvider culture = new CultureInfo("en-US", true);
			DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
			return date.ToString("dd/MM/yyyy");
		}

		#endregion

        #region MDYToDMYHHMMSS

        public static string MDYToDMYHHMMSS(string dateString)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        #endregion

		#region MDYToYMD

		public static string MDYToYMD(string dateString)
		{
			IFormatProvider culture = new CultureInfo("en-US", true);
			DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
			return date.ToString("yyyy/MM/dd");
		}

		#endregion

		#region YMDToDMY

		public static string YMDToDMY(string dateString)
		{
			IFormatProvider culture = new CultureInfo("en-US", true);
			DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
			return date.ToString("dd/MM/yyyy");
		}


		#endregion

		#region YMDToMDY

		public static string YMDToMDY(string dateString)
		{
			IFormatProvider culture = new CultureInfo("en-US", true);
			DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
			return date.ToString("MM/dd/yyyy");
		}

		#endregion

		#region DMYToYMD

		public static string DMYToYMD(string dateString)
		{
			IFormatProvider culture = new CultureInfo("fr-FR", true);
			DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
			return date.ToString("yyyy/MM/dd");
		}

		#endregion

        #region Format Date Time

        public static string FormatDateTime(string strDateTime)
        {
            string[] arr = strDateTime.Split(char.Parse(" "));
            strDateTime = arr[0].ToString();

            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime date = DateTime.Parse(strDateTime, culture, DateTimeStyles.NoCurrentDateDefault);
            return date.ToString("dd/MM/yyyy");
        }

        #endregion

		#region Compare Date

        //date1 > date2 => true
		public static bool CompareDate(string date1, string date2)
		{
			bool b = false;

			string [] dt1 = date1.Split('/');
			string [] dt2 = date2.Split('/');

			if(int.Parse(dt1[2]) > int.Parse(dt2[2]))
				b = true;			
			else if(int.Parse(dt1[2]) < int.Parse(dt2[2]))
				b = false;				
			else if(int.Parse(dt1[1]) > int.Parse(dt2[1]))
				b = true;					
			else if(int.Parse(dt1[1]) < int.Parse(dt2[1]))
				b = false;						
			else if(int.Parse(dt1[0]) > int.Parse(dt2[0]))
				b = true;
			else if(int.Parse(dt1[0]) <= int.Parse(dt2[0]))
			    b = false;								
			else b = true;
			
			return b;
		}

		#endregion

        #region DMY2YYYYMMDD

        public static string DMY2YYYYMMDD(string dateString)
        {
            IFormatProvider culture = new CultureInfo("fr-FR", true);
            DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
            return date.ToString("yyyyMMdd");
        }

        public static string DMY2YYYYMMDDHHMMSS(string dateString)
        {
            IFormatProvider culture = new CultureInfo("fr-FR", true);
            DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
            return date.ToString("yyyyMMddHHmmss");
        }

        #endregion DMY2YYYYMMDD

        #region ParseDMYToDMY

        public static DateTime ParseDMYToDMY(string dateString)
        {
            DateTime date = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        #endregion
    }

    #endregion

    #region MyDateTime

    /// <summary>
    /// Summary description for DateTime.
    /// </summary>
    public class MyDateTime
    {
        public MyDateTime()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// JulianToGregorian
        /// </summary>
        /// <PARAM name="strGregDate">string</PARAM>
        /// <PARAM name="strFormat">string</PARAM>
        public string JulianToGregorian(string strGregDate, string strFormat)
        {
            DateTime dtStart;
            DateTime dtTrms;
            int iDays;
            int iYear;
            string strDate;
            int iPos;
            Array arParams = null;

            strFormat = strFormat.ToUpper();
            arParams = strGregDate.Split('.');
            iYear = Convert.ToInt32((arParams.GetValue(0)).ToString());
            iDays = Convert.ToInt32((arParams.GetValue(1)).ToString());
            dtStart = new DateTime(iYear, 1, 1);
            dtTrms = dtStart.AddDays(iDays - 1);
            strDate = dtTrms.ToString();
            iPos = strDate.IndexOf(' ');
            strDate = strDate.Substring(0, iPos);

            strDate = ChangeDateFormat(strDate, strFormat);
            return strDate;
        }

        /// <summary>
        /// ChangeDateFormat
        /// </summary>
        /// <PARAM name="strDate">string</PARAM>
        /// <PARAM name="strFormat">string</PARAM>
        public string ChangeDateFormat(string strDate, string strFormat)
        {
            DateTime dateToInsert;

            try
            {
                dateToInsert = DateTime.Parse(strDate);
            }
            catch
            {
                dateToInsert = DateTime.MinValue;
            }

            strFormat = strFormat.ToUpper();
            if (strFormat.StartsWith("MM") && strFormat.IndexOf("YYYY") != -1)
                strDate = dateToInsert.ToString("MM/dd/yyyy");
            else
                if (strFormat.StartsWith("MM") && strFormat.IndexOf("YY") != -1)
                    strDate = dateToInsert.ToString("MM/dd/yy");
                else
                    if (strFormat.StartsWith("DD") && strFormat.IndexOf("YYYY") != -1)
                        strDate = dateToInsert.ToString("dd/MM/yyyy");
                    else
                        if (strFormat.StartsWith("DD") && strFormat.IndexOf("YY") != -1)
                            strDate = dateToInsert.ToString("dd/MM/yy");

            return strDate;
        }

        #region DMYToYMD

        public static string DMY2YYYYMMDD(string dateString)
        {
            IFormatProvider culture = new CultureInfo("fr-FR", true);
            DateTime date = DateTime.Parse(dateString, culture, DateTimeStyles.NoCurrentDateDefault);
            return date.ToString("yyyyMMdd");
        }

        #endregion DMYToYMD
    }

    #endregion
}