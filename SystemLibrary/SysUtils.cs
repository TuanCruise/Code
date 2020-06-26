using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Runtime.InteropServices;
//using System.Management;
using System.Xml;  
using System.Reflection;
using System.IO;
using WB.SystemLibrary;
//using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web;
//using NUnit.Framework;
//using System.Web.UI;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace WB.SYSTEM
{
	public class SysUtils
	{
		public SysUtils()
		{
		}

        public static string RemoveSignVNStr(string accented)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = accented.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }


      
        public static bool IsMatch(string strText)
        {
            try
            {
                //string pattern = "[^A-Z|^a-z|^0-9|-|,|.|/|(|)|;|:|\\| ]"; 
                //Not use @#' due to they are used in Smartbank
                string pattern = "[^a-z|^A-Z|^0-9|^~!$% .,^&*_?()\\\\/{}+-=`\"]";
                if (strText.IndexOf(char.Parse("<")) >= 0)
                    return false;

                Regex rx = new Regex(pattern);
                if (rx.IsMatch(strText))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;                
            }
            finally
            {

            }
        }

        public static bool IsMatch(string strText, string pattern)
        {
            try
            {                
                Regex rx = new Regex(pattern);
                if (rx.IsMatch(strText))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        //public static bool IsNumberOrChar(TextBox textBoxControl)
        //{
        //    string pattern = "[^A-Z|^a-z|^0-9|[-]|^\t]";            
        //    Regex rx = new Regex(pattern);
        //    if (rx.IsMatch(textBoxControl.Text))
        //        return false;
        //    return true;
        //}

        public static ArrayList RestrictSourcAcc(System.Collections.ArrayList Arr, string strPropetiesName, string strValue)
        {
            ArrayList arrHeaderArr = (ArrayList)Arr[0];

            int i;
            for (i = 1; i < Arr.Count; i++)
            {
                ArrayList arrSubArrDetail = (ArrayList)Arr[i];
                ArrayList arrSubArr = new ArrayList();

                arrSubArr.Add(arrHeaderArr);
                arrSubArr.Add(arrSubArrDetail);

                string strEleValue = SysUtils.getProperty(arrSubArr, strPropetiesName).Trim();

                if (strEleValue == strValue)
                {
                    Arr.RemoveAt(i);
                }
            }

            return Arr;
        }

        #region CheckPermission

        public static object getFValue(ArrayList arrProperties, string property)
        {
            try
            {
                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrDetail = (ArrayList)arrProperties[1];
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        return (object)arrDetail[i];
                    }
                }
                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static object getFValue(ArrayList arrProperties, string value, string property1, string property2)
        {
            try
            {
                ArrayList arrHeader = (ArrayList)arrProperties[0];

                for (int i = 1; i < arrProperties.Count; i++)
                {
                    ArrayList arrDetail = (ArrayList)arrProperties[i];

                    ArrayList arrElement = new ArrayList();
                    arrElement.Add(arrHeader);
                    arrElement.Add(arrDetail);

                    string strValue = getProperty(arrElement, property1).Trim();

                    if (strValue.Trim() == value)
                    {
                        return getProperty(arrElement, property2).Trim() ;
                    }
                }

                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static bool CheckPermission(DataTable dtPermission, string module_id, string permission_id)
        {
            foreach (DataRow r in dtPermission.Rows)
            {
                if (r["module_id"].ToString() == module_id && r["permission_id"].ToString() == permission_id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

		public static bool isDateTime(object obj)
		{
			DateTime date;
			try
			{
                if (obj.ToString() == "01/01/1900")
                    return false;
				date=SysUtils.CDateTime(obj);
				return true;
			}
			catch
			{
				return false;
			}
		}

        //public static bool chkDateTime(object obj)
        //{
        //    DateTime date;
        //    try
        //    {
        //        if (obj.ToString() == "01/01/1900")
        //            return false;
        //        date = SysUtils.CDate(obj);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

		public static bool isNumeric(object obj)
		{
			decimal dec;			
			try
			{
				dec=Convert.ToDecimal(obj);
				return true;
			}
			catch
			{
				return false;
			}
		}

        public static double CDouble(string strValue)
        {
            double dblValue=0;
            try
            {
                if (strValue != null)
                {
                    strValue = strValue.Replace("%", "");
                    strValue = strValue.Replace("$", "");
                    if (strValue != "")
                        dblValue = Convert.ToDouble(strValue);
                }
                return dblValue;
            }
            catch
            {
                return dblValue;
            }
        }

        public static double CDouble(object objValue)
        {
            double dblValue = 0;
            try
            {
                if (objValue != null)
                    dblValue = Convert.ToDouble(objValue);
                return dblValue;
            }
            catch
            {
                return dblValue;
            }
        }
        public static string CString(object objValue)
        {
            string strValue = "";
            try
            {
                if (objValue != null)
                {
                    strValue = strValue.Replace("\"","");
                    strValue = Convert.ToString(objValue).Trim();
                }
                return strValue;
            }
            catch
            {
                return strValue;
            }
        }
        public static bool IsNo1GreaterNo2(object objNumber1, object objNumber2)
        {
            try
            {
                double dblNumber1 = CDouble(objNumber1);
                double dblNumber2 = CDouble(objNumber2);

                if (dblNumber1 > dblNumber2)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return false;
            }
        }

        public static long CLong(string strValue)
        {
            long lngValue = 0;
            try
            {
                if (strValue != null)
                {
                    strValue = strValue.Replace("%", "");
                    strValue = strValue.Replace("$", "");
                    if (strValue != "")
                        lngValue = Convert.ToInt64(strValue);
                }
                return lngValue;
            }
            catch
            {
                return lngValue;
            }
        }

        public static long CLong(object objValue)
        {
            long lngValue = 0;
            try
            {
                if (objValue != null)
                    lngValue = Convert.ToInt64(objValue);
                return lngValue;
            }
            catch
            {
                return lngValue;
            }
        }

        public static int CInt(string strValue)
        {
            int intValue = 0;
            try
            {
                if (strValue != null)
                {
                    strValue = strValue.Replace("%", "");
                    strValue = strValue.Replace("$", "");
                    if (strValue != "")
                        intValue = Convert.ToInt32(strValue);
                }
                return intValue;
            }
            catch
            {
                return intValue;
            }
        }

        public static int CInt(object objValue)
        {
            int intValue = 0;
            try
            {
                if (objValue != null)
                    intValue = Convert.ToInt32(objValue);
                return intValue;
            }
            catch
            {
                return intValue;
            }
        }

        public static DateTime CDateTime(string strValue)
        {
            DateTime dtValue = Convert.ToDateTime("01/01/1900");
            try
            {
                if (strValue != null)
                {
                    if (strValue != "")
                        dtValue = Convert.ToDateTime(strValue);
                }
                return dtValue;
            }
            catch
            {
                return dtValue;
            }
        }

        public static DateTime CDateTime(object objValue)
        {
            DateTime dtValue = Convert.ToDateTime("01/01/1900");
            try
            {
                if (objValue != null)
                    dtValue = Convert.ToDateTime(objValue);
                return dtValue;
            }
            catch
            {
                return dtValue;
            }
        }

        public static bool isEnDate(object objValue)
        {
            DateTime dtValue = Convert.ToDateTime("01/01/1900");
            try
            {
                if (objValue != null)
                {
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    dtValue = DateTime.Parse(objValue.ToString(), culture, DateTimeStyles.NoCurrentDateDefault);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ChKDate(object objValue)
        {            
            try
            {
                if (objValue != null)
                {
                    string[] arrDate = objValue.ToString().Split(char.Parse("/"));

                    if (arrDate.GetLength(0) != 3)
                        return false;

                    if (Convert.ToInt16(arrDate[0].ToString()) < 0 || Convert.ToInt16(arrDate[0].ToString()) > 31)
                        return false;

                    if (Convert.ToInt16(arrDate[1].ToString()) < 0 || Convert.ToInt16(arrDate[1].ToString()) > 12)
                        return false;

                    if (arrDate[2].ToString().Length > 4)
                        return false;

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static System.Collections.ArrayList DataTable2ArrayList(DataTable datatable) 
		{
			try
			{
				ArrayList arrData=new System.Collections.ArrayList();
				ArrayList arrHeader=new System.Collections.ArrayList();
				//Dong dau tien chua header
				for (int i=0;i<datatable.Columns.Count;i++)
				{
					arrHeader.Add(datatable.Columns[i].ColumnName);
				}
				arrData.Add(arrHeader);

				//Dong chi tiet
				foreach (System.Data.DataRow aRow in datatable.Rows)
				{
					ArrayList arrDetail=new ArrayList();

					for (int i=0;i<datatable.Columns.Count;i++)
					{
						arrDetail.Add(aRow[i]);
					}
					arrData.Add(arrDetail);
				}
				return arrData;
			}
			catch
			{
				return null;
			}

		}

        //public static ArrayList ExcelFile2ArrList(string strPathFile, string lang)
        //{
        //    try
        //    {
        //        ArrayList arrData = new ArrayList();               
        //        //S1: OPEN FILE:
        //        //strPathFile = @"C:\newdoc.xls";
        //        CompoundDocument doc = CompoundDocument.Open(strPathFile);

        //        //S2: CONVERT DATA
        //        byte[] bookdata = doc.GetStreamData("Workbook");
        //        if (bookdata == null) return arrData;
        //        Workbook book = WorkbookDecoder.Decode(new MemoryStream(bookdata));

        //        try
        //        {
        //            foreach (Worksheet sheet in book.Worksheets)
        //            {
        //                if ((sheet.Name.Trim().ToUpper() == "DANH_SACH_LENH") || (sheet.Name.Trim().ToUpper() == "LIST_MESSAGES")) //&& lang == "VN"  && lang == "EN"
        //                {
        //                    // tranvers rows by Index
        //                    //for (int rowIndex = sheet.Cells.FirstRowIndex+2; rowIndex <= sheet.Cells.LastRowIndex -1; rowIndex++)                        

        //                    for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
        //                    {
        //                        ArrayList arrRow = new ArrayList();
        //                        Row row = sheet.Cells.GetRow(rowIndex);
        //                        for (int colIndex = row.FirstColIndex; colIndex <= row.LastColIndex; colIndex++)
        //                        {
        //                            Cell cell = row.GetCell(colIndex);

        //                            //HEADER:
        //                            //arrRow.Add(row.GetCell(colIndex).Value);
        //                            if (cell.FormatString == "m/d/yy")
        //                            {
        //                                string strValue = row.GetCell(colIndex).DateValue.ToString(Constants.DD_MM_YYYY);
        //                                if (!string.IsNullOrEmpty(strValue))
        //                                    arrRow.Add(strValue.Trim());
        //                                else
        //                                    arrRow.Add("");
        //                            }
        //                            else
        //                            {
        //                                string strValue = row.GetCell(colIndex).StringValue;
        //                                if (!string.IsNullOrEmpty(strValue))
        //                                    arrRow.Add(strValue.Trim());
        //                                else
        //                                    arrRow.Add("");
        //                            }
        //                        }

        //                        arrData.Add(arrRow);
        //                    }
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            //09/08/2012
        //            doc.Close();      
                    
        //        }

        //        return arrData;
        //    }
        //    catch (WB.SYSTEM.ErrorMessage e)
        //    {
        //        WB.SYSTEM.ErrorHandler.Process(e);
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        WB.SYSTEM.ErrorHandler.Process(e);
        //        return null;
        //    }
        //    finally
        //    {
        //    }           
        //}
       	

     
		public static DataTable ArrayList2DataTable(System.Collections.ArrayList myArray)
		{
			try
			{
                DataTable myTable = new DataTable();

                System.Collections.ArrayList subList = (System.Collections.ArrayList)myArray[0];
                int i = 0;
                int j = 0;
                for (i = 0; i < subList.Count; i++)
                {
                    myTable.Columns.Add(subList[i].ToString().Trim());
                }
                for (i = 1; i < myArray.Count; i++)
                {
                    subList = (System.Collections.ArrayList)myArray[i];
                    DataRow aRow = myTable.NewRow();
                    for (j = 0; j < subList.Count; j++)
                    {
                        aRow[j] = subList[j];
                    }
                    myTable.Rows.Add(aRow);
                }
                return myTable;               
			}
			catch(Exception ex)
			{
				return null;
			}
		}

        public static DataTable ArrayList2DataTable(System.Collections.ArrayList myArray, bool rownumber)
        {
            try
            {
                DataTable myTable = new DataTable();

                System.Collections.ArrayList subList = (System.Collections.ArrayList)myArray[0];

                if (((System.Collections.ArrayList)myArray[0])[0].ToString()== "ROWNUMBER")
                    rownumber = false;
 
                if (rownumber)
                {
                    subList.Insert(0, "ROWNUMBER");
                }

                int i = 0;
                int j = 0;
                for (i = 0; i < subList.Count; i++)
                {
                    myTable.Columns.Add(subList[i].ToString().Trim());                      
                }

                for (i = 1; i < myArray.Count; i++)
                {
                    subList = (System.Collections.ArrayList)myArray[i];
                    if (rownumber)
                    {
                        subList.Insert(0, i.ToString());
                    }

                    DataRow aRow = myTable.NewRow();
                    for (j = 0; j < subList.Count; j++)
                    {
                        aRow[j] = subList[j];
                    }
                    myTable.Rows.Add(aRow);
                }
                return myTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static DataTable ArrList2DataTable(ArrayList myArray)
        {
            DataTable myTable = new DataTable();

            try
            {
                if (myArray == null)
                {
                    return myTable;
                }

                if (myArray.Count == 0)
                {
                    return myTable;
                }

                System.Collections.ArrayList subList = (System.Collections.ArrayList)myArray[0];
              
                int i = 0;
                int j = 0;
                for (i = 0; i < subList.Count; i++)
                {
                    myTable.Columns.Add(subList[i].ToString());
                }
                for (i = 1; i < myArray.Count; i++)
                {
                    subList = (System.Collections.ArrayList)myArray[i];
                    DataRow aRow = myTable.NewRow();
                    for (j = 0; j < subList.Count; j++)
                    {
                        aRow[j] = subList[j];
                    }
                    myTable.Rows.Add(aRow);
                }
                return myTable;
            }
            catch
            {
                return myTable;
            }
        }

        public static DataTable ArrList2DataTable1Empty(ArrayList myArray, int EmptyIdx)
        {
            DataTable myTable = new DataTable();

            try
            {
                if (myArray == null)
                {
                    return myTable;
                }

                if (myArray.Count == 0)
                {
                    return myTable;
                }

                System.Collections.ArrayList subList = (System.Collections.ArrayList)myArray[0];

                int i = 0;
                int j = 0;
                for (i = 0; i < subList.Count; i++)
                {
                    myTable.Columns.Add(subList[i].ToString());
                }
                for (i = 1; i < myArray.Count; i++)
                {
                    if (i - 1 == EmptyIdx)
                    {//them dong trang
                        DataRow eRow = myTable.NewRow();
                        myTable.Rows.Add(eRow);
                    }
                    subList = (System.Collections.ArrayList)myArray[i];
                    DataRow aRow = myTable.NewRow();
                    for (j = 0; j < subList.Count; j++)
                    {
                        aRow[j] = subList[j];
                    }
                    myTable.Rows.Add(aRow);
                }
                return myTable;
            }
            catch
            {
                return myTable;
            }
        }

        public static DataTable ArrayList2DataTable(System.Collections.ArrayList myArray, string strCondition)
        {
            try
            {
                DataTable myTable = new DataTable();
                System.Collections.ArrayList subList = (System.Collections.ArrayList)myArray[0];
                int i = 0;
                int j = 0;
                for (i = 0; i < subList.Count; i++)
                {
                    myTable.Columns.Add(subList[i].ToString());
                }
                for (i = 1; i < myArray.Count; i++)
                {
                    subList = (System.Collections.ArrayList)myArray[i];
                    DataRow aRow = myTable.NewRow();
                    for (int index = 0; index < subList.Count; index++)
                    {
                        if (subList[index].ToString().ToUpper() == strCondition.ToUpper())
                        {
                            for (j = 0; j < subList.Count; j++)
                            {
                                aRow[j] = subList[j];
                            }
                            myTable.Rows.Add(aRow);
                        }
                    }

                }
                return myTable;
            }
            catch
            {
                return null;
            }
        }

		public static bool CompareArrayList(System.Collections.ArrayList Arr1,System.Collections.ArrayList Arr2) 
		{
			int i;
			for (i=0;i<Arr1.Count;i++)
			{
				if (Arr1[i].ToString()!=Arr2[i].ToString())
				{
					return false;
				}
			}
			return true;
		}

        //public static ArrayList RestrictSourcAcc(ArrayList Arr1, ArrayList Arr2, string strPropetiesName) 
        //{
        //    ArrayList arrHeaderArr1 = (ArrayList)Arr1[0];
        //    ArrayList arrHeaderArr2 = (ArrayList)Arr2[0];

        //    ArrayList arrResult = new ArrayList();
        //    arrResult.Add(arrHeaderArr1);

        //    if (Arr2.Count == 1)
        //    {
        //        return Arr1;
        //    }

        //    int i;
        //    for (i=1;i<Arr1.Count;i++)
        //    {
        //        ArrayList arrSubArrDetail1 = (ArrayList)Arr1[i];
        //        ArrayList arrSubArr1 = new ArrayList();

        //        arrSubArr1.Add(arrHeaderArr1);
        //        arrSubArr1.Add(arrSubArrDetail1);

        //        for (int j = 1; j < Arr2.Count; j++)
        //        {
        //            ArrayList arrSubArrDetail2 = (ArrayList)Arr2[j];
        //            ArrayList arrSubArr2 = new ArrayList();

        //            arrSubArr2.Add(arrHeaderArr2);
        //            arrSubArr2.Add(arrSubArrDetail2);                  

        //            if (SysUtils.getProperty(arrSubArr1, strPropetiesName).ToString().Trim().Replace(Constants.SYMBOL_HORI_MARSK, "") == SysUtils.getProperty(arrSubArr2, strPropetiesName).ToString().Trim().Replace(Constants.SYMBOL_HORI_MARSK, ""))
        //            {
        //                //Arr1.RemoveAt(i);
        //                arrResult.Add(arrSubArrDetail1);
        //            }
        //        }
        //    }
        //    return arrResult;

        //}        

      

		public static string formatStr(string inpStr,string formatString)
		{
			string inStr=inpStr.Replace("-","");
			string outStr="";
			string forStr=formatString;
			
			while (forStr.Length>0)
			{
				if (forStr.IndexOf("\\d")==0)
				{
					outStr+=inStr.Substring(0,Convert.ToInt16(forStr.Substring(3,1)));
					inStr=inStr.Substring(Convert.ToInt16(forStr.Substring(3,1)));
					forStr=forStr.Substring(5);
				}
				else if (forStr.Substring(0,1)=="n")
				{
					if (inStr.IndexOf(".")>=0) 
					{
						inStr+="00";
						inStr=inStr.Substring(0,inStr.IndexOf(".")+3);
					}
					else
					{
						inStr+=".00";
					}
					while (inStr.Substring(0,4).IndexOf(",")<0 && inStr.Length>6)
					{
						int lastPos=inStr.IndexOf(",");
						if (lastPos==-1) lastPos=inStr.IndexOf(".");
						inStr=inStr.Substring(0,lastPos-3)+","+inStr.Substring(lastPos-3);
					}
					outStr=inStr;
					forStr="";
				}
				else
				{
					outStr+=forStr.Substring(0,1);
					forStr=forStr.Substring(1);
				}
			}
			return outStr;
		}

		public static decimal MyRound(decimal Amt , double dec)
		{
			try
			{
				int n;
				if (Amt==0) return 0;
				if (dec==0) return Math.Round(Amt);
				n = Convert.ToInt16(Math.Pow(10,Math.Abs(dec)));
				if (dec<0) return n*Math.Round(Amt / n);
				if (dec>0) return Math.Round(Amt * n) / n;						
				return Decimal.MinValue;
			}
			catch
			{
				return Decimal.MinValue;
			}
		}
		
		public static string genCheckKey(string vl)
		{
			return vl.Substring(vl.Length-1);
		}

        public static string BrID2Char(string ebank)
        {
            string strChar = "A";
            try
            {
                string strBrID = ebank.Substring(0, 2);
                switch (strBrID)
                {
                    case "01":
                        strChar = "A";
                        break;
                    case "02":
                        strChar = "B";
                        break;
                    case "03":
                        strChar = "C";
                        break;
                    case "04":
                        strChar = "D";
                        break;
                    case "05":
                        strChar = "E";
                        break;
                    case "06":
                        strChar = "F";
                        break;
                    case "07":
                        strChar = "G";
                        break;
                    case "08":
                        strChar = "H";
                        break;                    
                    case "09":
                        strChar = "I";
                        break;
                    case "10":
                        strChar = "J";
                        break;
                    case "11":
                        strChar = "K";
                        break;
                    case "12":
                        strChar = "L";
                        break;
                    case "80":
                        strChar = "M";
                        break;
                    default:
                        strChar = "Z";
                        break;
                }

                return strChar;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string Day2Char(string day)
        {
            string strChar = "A";
            try
            {
                switch (day)
                {
                    case "01":
                        strChar = "A";
                        break;
                    case "02":
                        strChar = "B";
                        break;
                    case "03":
                        strChar = "C";
                        break;
                    case "04":
                        strChar = "D";
                        break;
                    case "05":
                        strChar = "E";
                        break;
                    case "06":
                        strChar = "F";
                        break;
                    case "07":
                        strChar = "G";
                        break;
                    case "08":
                        strChar = "H";
                        break;
                    case "09":
                        strChar = "I";
                        break;
                    case "10":
                        strChar = "J";
                        break;
                    case "11":
                        strChar = "K";
                        break;
                    case "12":
                        strChar = "L";
                        break;
                    case "13":
                        strChar = "M";
                        break;
                    case "14":
                        strChar = "N";
                        break;
                    case "15":
                        strChar = "O";
                        break;
                    case "16":
                        strChar = "P";
                        break;
                    case "17":
                        strChar = "Q";
                        break;
                    case "18":
                        strChar = "R";
                        break;
                    case "19":
                        strChar = "S";
                        break;
                    case "20":
                        strChar = "T";
                        break;
                    case "21":
                        strChar = "U";
                        break;
                    case "22":
                        strChar = "V";
                        break;
                    case "23":
                        strChar = "W";
                        break;
                    case "24":
                        strChar = "X";
                        break;
                    case "25":
                        strChar = "Y";
                        break;
                    case "26":
                        strChar = "Z";
                        break;
                    case "27":
                        strChar = "1";
                        break;
                    case "28":
                        strChar = "2";
                        break;
                    case "29":
                        strChar = "3";
                        break;
                    case "30":
                        strChar = "4";
                        break;
                    case "31":
                        strChar = "5";
                        break;
                    default:
                        strChar = "Z";
                        break;
                }

                return strChar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Year2Char(string year)
        {
            string strChar = "A";
            try
            {
                switch (year)
                {
                    case "13":
                        strChar = "A";
                        break;
                    case "14":
                        strChar = "B";
                        break;
                    case "15":
                        strChar = "C";
                        break;
                    case "16":
                        strChar = "D";
                        break;
                    case "17":
                        strChar = "E";
                        break;
                    case "18":
                        strChar = "F";
                        break;
                    case "19":
                        strChar = "G";
                        break;
                    case "20":
                        strChar = "H";
                        break;
                    case "21":
                        strChar = "I";
                        break;
                    case "22":
                        strChar = "J";
                        break;
                    case "23":
                        strChar = "K";
                        break;
                    case "24":
                        strChar = "L";
                        break;
                    case "25":
                        strChar = "M";
                        break;
                    case "26":
                        strChar = "N";
                        break;
                    case "27":
                        strChar = "O";
                        break;
                    case "28":
                        strChar = "P";
                        break;
                    case "29":
                        strChar = "Q";
                        break;
                    case "30":
                        strChar = "R";
                        break;
                    case "31":
                        strChar = "S";
                        break;
                    case "32":
                        strChar = "T";
                        break;
                    case "33":
                        strChar = "U";
                        break;
                    case "34":
                        strChar = "V";
                        break;
                    case "35":
                        strChar = "W";
                        break;
                    case "36":
                        strChar = "X";
                        break;
                    case "37":
                        strChar = "Y";
                        break;
                    case "38":
                        strChar = "Z";
                        break;                   
                    default:
                        strChar = "Z";
                        break;
                }

                return strChar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }		

		public static object getValue(ArrayList arr,string name)
		{
            try
            {
                for (int i = 0; i < arr.Count - 1; i += 2)
                    if (arr[i].ToString().ToUpper() == name.ToUpper()) return arr[i + 1];
                return null;
            }
            catch
            {
                return null;
            }
		}

        public static object getURLKey(ArrayList arr, string strKey)
        {
            try
            {
                for (int i = 0; i < arr.Count - 1; i += 2)
                    if (arr[i + 1].ToString().ToUpper() == strKey.ToUpper()) 
                        return arr[i];
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static object getValue(string[] arr, string name)
        {
            try
            {
                for (int i = 0; i < arr.GetLongLength(0) - 1; i += 2)
                    if (arr[i].ToString().ToUpper() == name.ToUpper()) return arr[i + 1];
                return null;
            }
            catch
            {
                return null;
            }
        }

		public static object getValues(ArrayList arr)
		{
			try
			{
				for (int i=0;i<arr.Count-1;i+=2)
				{
                    return arr[i + 1];
				}
                return null;
			}
			catch
			{
				return null;
			}
		}

        //HueMT 17.9
        public static int getIndex(ArrayList arr, string property, string fieldvalue)
        {
            ArrayList arrProperty = (ArrayList)arr[0];
            int propidx = -1;

            for (int i = 0; i < arrProperty.Count; i++)
            {
                if (arrProperty[i].ToString().ToUpper() == property.ToUpper())
                {
                    propidx = i;
                    break;
                }
            }

            if (propidx == -1) return -1;

            for (int i = 0; i < arr.Count; i++)
            {
                ArrayList arrElement = new ArrayList();
                arrElement = (ArrayList)arr[i];
                if (arrElement[propidx].ToString().ToUpper() == fieldvalue.ToUpper())
                    return i;
            }

            return -1;
        }

		public static void setValue(ref ArrayList arr,string name,object Value)
		{
			try
			{
			for (int i=0;i<arr.Count-1;i+=2)
				if (arr[i].ToString().ToUpper()==name.ToUpper()) 
				{
					arr[i+1]=Value;
					return;
				}
			arr.Add(name);
			arr.Add(Value);
			}
			catch(WB.SYSTEM.ErrorMessage ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
			}
			catch(Exception ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
			}
		}

        public static void setProperty(ref ArrayList arrProperties, string property, object Value)
        {
            try
            {
                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrDetail = (ArrayList)arrProperties[1];

                if (arrDetail.Count < arrHeader.Count)
                {
                    for (int i = arrDetail.Count; i < arrHeader.Count; i++)
                    {
                        arrDetail.Add(null);
                    }
                }

                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        arrDetail[i] = Value;
                        //arrProperties[1] = arrDetail;
                        return;
                    }
                }

                arrHeader.Add(property);
                arrDetail.Add(Value);
                //arrProperties[0] = arrHeader;
                //arrProperties[1] = arrDetail;

            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
            }
        }

        public static void setProperty(ref ArrayList arrProperties, string property, object Value, int index)
        {
            try
            {
                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrDetail = (ArrayList)arrProperties[index];
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        arrDetail[i] = Value;
                        return;
                    }
                }

                arrHeader.Add(property);
                arrDetail.Add(Value);

            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
            }
        }

        public static void setProperty(ref ArrayList arrHeader, ref  ArrayList arrDetail, string property, object Value)
        {
            try
            {               
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        arrDetail[i] = Value;
                        //arrProperties[1] = arrDetail;
                        return;
                    }
                }

                arrHeader.Add(property);
                arrDetail.Add(Value);
                //arrProperties[0] = arrHeader;
                //arrProperties[1] = arrDetail;

            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
            }
        }

        public static ArrayList setProperties(ArrayList arrProperty, string property, object Value)
        {
            try
            {
                ArrayList arrResult = new ArrayList();
                ArrayList arrHeader = (ArrayList)arrProperty[0];
                ArrayList arrDetail = (ArrayList)arrProperty[1];
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        arrDetail[i] = Value;

                        arrResult.Add(arrHeader);
                        arrResult.Add(arrDetail);
                        return arrResult;
                    }
                }

                arrHeader.Add(property);
                arrDetail.Add(Value);

                arrResult.Add(arrHeader);
                arrResult.Add(arrDetail);

                return arrResult;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static ArrayList Property2Value(ArrayList arrProperty)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader= (ArrayList)arrProperty[0];
                ArrayList arrDetail = (ArrayList)arrProperty[1];

                int intCount = arrHeader.Count;

                for (int i = 0; i < intCount; i++)
                {
                    if (arrDetail[i] != null)
                    {
                        arrData.Add(arrHeader[i]);
                        arrData.Add(arrDetail[i]);
                    }
                }

                return arrData;      
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static ArrayList Value2Property(ArrayList arrValue)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();
                ArrayList arrDetail = new ArrayList();

                int intCount = arrValue.Count;

                for (int i = 0; i < intCount; i += 2)
                {
                    arrHeader.Add(arrValue[i]);
                    arrDetail.Add(arrValue[i + 1]);
                }

                arrData.Add(arrHeader);
                arrData.Add(arrDetail);

                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static string ArrList2String(string[] arrParm, ArrayList arrData, string strSymbol)
        {
            string strResult = "";
            try
            {
                for (int i = 0; i < arrParm.GetLength(0); i += 2)
                {
                    string strValue = SysUtils.CString(SysUtils.getValue(arrData, arrParm[i]));
                    strResult += strSymbol + arrParm[i] + "=" + strValue;
                }

                return strResult;
            }
            catch
            {
                return strResult;
            }
        }

        public static string Array2URLParm(ArrayList arrParm)
        {
            string strResult = "";
            try
            {                
                for (int i = 0; i < arrParm.Count; i += 2)
                {
                    strResult += "&" + arrParm[i] + "=" + arrParm[i+ 1];
                }

                return strResult;
            }
            catch
            {
                return strResult;
            }
        }

        public static string ArrList2String(ArrayList arrData, string strSymbol)
        {
            string strResult = "";
            try
            {
                for (int i = 0; i < arrData.Count; i += 2)
                    strResult += strSymbol + arrData[i] + "=" + arrData[i + 1];
                return strResult;
            }
            catch
            {
                return strResult;
            }
        }

        public static string ArrProperty2String(ArrayList arrData, string strSymbol)
        {
            string strResult = "";
            try
            {
                ArrayList arrHeader = (ArrayList)arrData[0];
                ArrayList arrDetail = (ArrayList)arrData[1];
                for (int i = 0; i < arrHeader.Count; i ++)
                    strResult += arrHeader[i] + strSymbol + arrDetail[i];

                return strResult;
            }
            catch
            {
                return strResult;
            }
        }

        public static ArrayList String2Arrray(string strData, string strSymbol)
        {
            ArrayList arrData = new ArrayList();
            try
            {
                string[] arrDetail = strData.Split(char.Parse(strSymbol));

                for (int i = 0; i < arrDetail.GetLength(0); i++)
                {
                    arrData.Add(arrDetail[i]);
                }

                return arrData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ArrayList Array2ArrValue(string[] arr, ArrayList  arrParm)
        {
            ArrayList arrData = new ArrayList();
            try
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    arrData.Add(arr[i]);
                }

                return arrData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public static  string getProperty(ArrayList arrHeader,ArrayList arrProperties, string property)
		{
			try
			{
				for(int i=0;i<arrHeader.Count;i++)
				{
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper() && arrProperties[i] != null)
                    {
                        return arrProperties[i].ToString();
                    }
				}
				return null;
			}
			catch(WB.SYSTEM.ErrorMessage ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return null;
			}
			catch(Exception ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return null;
			}
		}

		public static  string getProperty(ArrayList arrProperties, string property)
		{
			try
			{
                if (arrProperties.Count < 2) return null;

				ArrayList arrHeader=(ArrayList)arrProperties[0];
				ArrayList arrDetail=(ArrayList)arrProperties[1];
				for(int i=0;i<arrHeader.Count;i++)
				{
					if(arrHeader[i].ToString().ToUpper()==property.ToUpper())
					{
						return CString(arrDetail[i]);
					}
				}
				return null;
			}
			catch(WB.SYSTEM.ErrorMessage ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return null;
			}
			catch(Exception ex)
			{
				WB.SYSTEM.ErrorHandler.Process(ex);
				return null;
			}
		}

        public static object getProperty(ArrayList arrProperties, string property, int rowind)
        {
            try
            {
                if (arrProperties.Count < rowind + 1) return null;

                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrDetail = (ArrayList)arrProperties[rowind];
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        return arrDetail[i];
                    }
                }
                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }


        public static ArrayList getPropertisArr(ArrayList arrProperties,string value, string property)
        {
            try
            {
                if (arrProperties.Count < 2) return null;

                ArrayList arrHeader = (ArrayList)arrProperties[0];

                for (int i = 1; i < arrProperties.Count; i++)
                {                   
                    ArrayList arrDetail = (ArrayList)arrProperties[i];

                    ArrayList arrElement = new ArrayList();
                    arrElement.Add(arrHeader);
                    arrElement.Add(arrDetail);

                    string strValue = getProperty(arrElement, property).Trim();

                    if (strValue.Trim() == value)
                    {
                        return arrElement;
                    }
                }

                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static ArrayList getPropertisSubArr(ArrayList arrProperties, string value, string property)
        {
            try
            {
                if (arrProperties.Count < 2) return null;

                ArrayList arrHeader = (ArrayList)arrProperties[0];

                ArrayList arrSubData = new ArrayList();

                arrSubData.Add(arrHeader);

                for (int i = 1; i < arrProperties.Count; i++)
                {
                    ArrayList arrDetail = (ArrayList)arrProperties[i];

                    ArrayList arrElement = new ArrayList();
                    arrElement.Add(arrHeader);
                    arrElement.Add(arrDetail);

                    string strValue = getProperty(arrElement, property).Trim();

                    if (strValue.Trim().ToUpper() == value.ToUpper())
                    {
                        arrSubData.Add(arrDetail);
                    }
                }

                return arrSubData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static string getPropertisSubVal(ArrayList arrProperties, string value, string property)
        {
            try
            {
                if (arrProperties.Count < 2) return null;

                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrSubData = new ArrayList();

                arrSubData.Add(arrHeader);

                for (int i = 1; i < arrProperties.Count; i++)
                {
                    ArrayList arrDetail = (ArrayList)arrProperties[i];

                    ArrayList arrElement = new ArrayList();
                    arrElement.Add(arrHeader);
                    arrElement.Add(arrDetail);

                    string strValue = getProperty(arrElement, property).Trim();

                    if (strValue.Trim().ToUpper() == value.ToUpper())
                    {
                        return strValue;                 
                    }
                }

                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static ArrayList getValueArr(ArrayList arrProperties, string property)
        {
            try
            {
                if (arrProperties.Count < 2) return null;

                ArrayList arrResult = new ArrayList();

                ArrayList arrHeader = (ArrayList)arrProperties[0];

                for (int i = 1; i < arrProperties.Count; i++)
                {
                    ArrayList arrDetail = (ArrayList)arrProperties[i];

                    ArrayList arrElement = new ArrayList();
                    arrElement.Add(arrHeader);
                    arrElement.Add(arrDetail);

                    string strValue = getProperty(arrElement, property).Trim();

                    arrResult.Add(strValue);
                }

                return arrResult;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static object getPropertis(ArrayList arrProperties, string property)
        {
            try
            {
                if (arrProperties.Count < 2) return null;

                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrDetail = (ArrayList)arrProperties[1];
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        return arrDetail[i];
                    }
                }
                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }


        public static object GetTxnFieldValue(ArrayList arrProperties, string strProperty)
        {
            try
            {
                for (int i = 0; i < arrProperties.Count; i += 2)
                {
                    if (arrProperties[i].ToString().ToUpper() == strProperty.ToUpper())
                        return arrProperties[i + 1];
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }
        }

        public static string getProperty(ArrayList arrProperties,int intIndex, string property)
        {
            try
            {
                ArrayList arrHeader = (ArrayList)arrProperties[0];
                ArrayList arrDetail = (ArrayList)arrProperties[intIndex];
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    if (arrHeader[i].ToString().ToUpper() == property.ToUpper())
                    {
                        return arrDetail[i].ToString();
                    }
                }
                return null;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        


  //      public string GetVolumeSerial(string strDriveLetter)
		//{
		//	try
		//	{
		//	if( strDriveLetter=="" || strDriveLetter==null) strDriveLetter="C";
		//	ManagementObject disk = 
		//		new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter +":\"");
		//	disk.Get();
		//	return disk["VolumeSerialNumber"].ToString();
		//	}
		//	catch(WB.SYSTEM.ErrorMessage ex)
		//	{
		//		WB.SYSTEM.ErrorHandler.Process(ex);
		//		return "";
		//	}
		//	catch(Exception ex)
		//	{
		//		WB.SYSTEM.ErrorHandler.Process(ex);
		//		return "";
		//	}
		//}

        public static ArrayList ConvertObj2ArrList(object[] objResult)
        {
            try
            {
                ArrayList arr0 = new ArrayList();
                ArrayList arr15 = new ArrayList();
                ArrayList arr16 = new ArrayList();
                ArrayList arrData = new ArrayList();

                string strParm0 = string.Empty;
                for (int i = 0; i < ((object[])objResult[0]).Length; i++)
                {
                    strParm0 += ((object[])objResult[0])[i].ToString() + "|";
                }
                strParm0 = strParm0.TrimEnd(char.Parse("|"));

                string strParm15 = string.Empty;
                for (int i = 0; i < ((object[])objResult[15]).Length; i++)
                {
                    strParm15 += ((object[])objResult[15])[i].ToString() + "|";
                }
                strParm15 = strParm15.TrimEnd(char.Parse("|"));

                string strParm16 = string.Empty;
                for (int i = 0; i < ((object[])objResult[16]).Length; i++)
                {
                    strParm16 += ((object[])objResult[16])[i].ToString() + "|";
                }
                strParm16 = strParm16.TrimEnd(char.Parse("|"));

                for (int i = 0; i < objResult.Length; i++)
                {
                    if (i == 0)
                    {
                        arrData.Add("PARM0");
                        arrData.Add(strParm0);
                    }
                    else if (i == 15)
                    {
                        arrData.Add("PARM15");
                        arrData.Add(strParm15);
                    }
                    else if (i == 16)
                    {
                        arrData.Add("PARM16");
                        arrData.Add(strParm16);
                    }
                    else
                    {
                        arrData.Add("PARM" + i.ToString());
                        arrData.Add(objResult[i]);
                    }
                }

                return arrData;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }

        }

        public static ArrayList Obj2ArrProperty(object[,] objResult,bool incHeader)
        {
            try
            {
                ArrayList arrData = new ArrayList();

                int countRows = objResult.GetUpperBound(0);
                int countColumn = objResult.GetUpperBound(1);

                if (countColumn == 0 || countRows < 1)
                {
                    return arrData;
                }
                if (incHeader)
                {
                    //Header
                    ArrayList arrHeader = new ArrayList();
                    for (int j = 0; j <= countColumn; j++)
                    {
                        arrHeader.Add(objResult[0, j].ToString().Trim());
                    }
                    arrData.Add(arrHeader);
                }

                int iStart = (incHeader ? 1 : 0);
                //Detail
                for (int i = iStart; i <= countRows; i++)
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int j = 0; j <= countColumn; j++)
                    {
                        arrDetail.Add(objResult[i, j].ToString().Trim());
                    }
                    arrData.Add(arrDetail);
                }

                return arrData;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }

        }   
        public static object[] ArrList2ArrObject(ArrayList arrList)
        {
            try
            {                  
                int countRow = arrList.Count;

                int countColum = ((ArrayList)arrList[0]).Count;

                object[] objResult = new object[countRow];

                for (int i = 0; i < countRow; i++)
                {
                    ArrayList arrSubList = (ArrayList)arrList[i];   
                 
                    object[] objSubList = new object[countColum];

                    for (int j = 0; j < countColum; j++)
                    {
                        objSubList[j] = (object)arrSubList[j];
                    }
                    objResult[i] = objSubList;                 
                }

                return objResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }

        }
           
        public ArrayList LoadElementDetail(string strCondition, DataSet dsEorrorMsg)
        {            
            try
            {
                DataTable dataTable = dsEorrorMsg.Tables["Table"];

                DataRow[] dtRows = dataTable.Select(strCondition);

                ArrayList arrResult = new ArrayList();
                ArrayList arrDtHeader = new ArrayList();
                System.Data.DataRow rowHeader = dataTable.Rows[0];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    arrDtHeader.Add(dataTable.Columns[i].ColumnName);
                }
                arrResult.Add(arrDtHeader);

                foreach (System.Data.DataRow aRow in dtRows)
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        arrDetail.Add(aRow[i]);
                    }
                    arrResult.Add(arrDetail);
                }
                return arrResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
		
		/// <summary>
		/// Returns MAC Address from first Network Card in Computer
		/// </summary>
		/// <returns>[string] MAC Address</returns>
		//public string GetMACAddress()
		//{
		//	try
		//	{
		//	ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
		//	ManagementObjectCollection moc = mc.GetInstances();
		//	string MACAddress=String.Empty;
		//	foreach(ManagementObject mo in moc)
		//	{
		//		if(MACAddress==String.Empty)  // only return MAC Address from first card
		//		{
		//			if((bool)mo["IPEnabled"] == true) MACAddress= mo["MacAddress"].ToString() ;
		//		}
		//		mo.Dispose();
		//	}
		//	MACAddress=MACAddress.Replace(":","");
		//	return MACAddress;
		//	}
		//	catch(WB.SYSTEM.ErrorMessage ex)
		//	{
		//		WB.SYSTEM.ErrorHandler.Process(ex);
		//		return "";
		//	}
		//	catch(Exception ex)
		//	{
		//		WB.SYSTEM.ErrorHandler.Process(ex);
		//		return "";
		//	}
		//}

		/// <summary>
		/// Return processorId from first CPU in machine
		/// </summary>
		/// <returns>[string] ProcessorId</returns>
		//public static string GetCPUId()
		//{
		//	try
		//	{
		//	string cpuInfo =  String.Empty;
		//	string temp=String.Empty;
		//	ManagementClass mc = new ManagementClass("Win32_Processor");
		//	ManagementObjectCollection moc = mc.GetInstances();
		//	foreach(ManagementObject mo in moc)
		//	{
		//		if(cpuInfo==String.Empty) 
		//		{// only return cpuInfo from first CPU
		//			cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
		//		}			 
		//	}
		//	return cpuInfo;
		//	}
		//	catch(WB.SYSTEM.ErrorMessage ex)
		//	{
		//		WB.SYSTEM.ErrorHandler.Process(ex);
		//		return "";
		//	}
		//	catch(Exception ex)
		//	{
		//		WB.SYSTEM.ErrorHandler.Process(ex);
		//		return "";
		//	}
		//}

        /// <summary>
        /// Check SMS code
        /// </summary>
        /// <returns>[bool]</returns>
        #region CheckKey

        public static bool CheckKey(string code)
        {
            try
            {
                char[] ch = code.ToCharArray();
                if (ch.Length == 8)
                {
                    int s1 = 0;
                    for (int i = 0; i < 7; i += 2)
                    {
                        s1 += int.Parse(ch[i].ToString());
                    }

                    int s2 = 0;
                    for (int i = 1; i < 7; i += 2)
                    {
                        s2 += int.Parse(ch[i].ToString());
                    }

                    int key = (s1 + s2) / 9;
                    if (int.Parse(ch[7].ToString()) == key)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_GEN_KEY;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Generate SMS code
        /// </summary>
        /// <returns>[string]</returns>
        #region Generate SMS Code

        public static string GenerateCode()
        {
            try
            {
                Random r = new Random();
                int[] arr = new int[7];
                for (int i = 0; i < 7; i++)
                {
                    arr[i] = r.Next(0, 9);
                }

                int s1 = 0;
                for (int i = 0; i < 7; i += 2)
                {
                    s1 += arr[i];
                }

                int s2 = 0;
                for (int i = 1; i < 7; i += 2)
                {
                    s2 += arr[i];
                }

                int key = (s2 + s1) / 9;
                string str = string.Empty;
                for (int i = 0; i < 7; i++)
                {
                    str += arr[i].ToString();
                }

                return str + key.ToString();
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_GEN_KEY;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        #endregion

     

        #region TableRowsSort

        public static DataTable TableRowsSort(DataTable dataTable, string strOrdBy)
        {
            try
            {
                DataRow[] dtRows = dataTable.Select("", strOrdBy);

                DataTable dtResult = dataTable.Clone();

                foreach (DataRow thisRow in dtRows)
                {
                    dtResult.Rows.Add(thisRow.ItemArray);
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }

        }

        public static DataTable Table2Table(DataTable dataTable, string strListFields)
        {
            try
            {
                DataView view = new DataView(dataTable);
                string[] arrListField = strListFields.Split(char.Parse("/"));
                strListFields = "";
                for (int i = 0; i < arrListField.GetLength(0); i++)
                {
                    strListFields += arrListField[i] + ",";
                }

                DataTable distinctTable = view.ToTable(true, "serviceName","serviceCode");                            

                return distinctTable;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }
        }

        public static DataTable TableFilter(DataTable dataTable, string strCondition, string strOrdBy)
        {
            try
            {
                DataRow[] dtRows = dataTable.Select(strCondition, strOrdBy);
                DataTable dtResult = dataTable.Clone();

                foreach (DataRow thisRow in dtRows)
                {
                    dtResult.Rows.Add(thisRow.ItemArray);
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }

        }

    
        public static DataTable Table_Add_ROWNUMBER(DataTable dataTable)
        {
            try
            {
                DataRow[] dtRows = dataTable.Select();
                DataTable dtResult = dataTable.Clone();
                int i = 1;

                foreach (DataRow thisRow in dtRows)
                {
                    thisRow["ROWNUMBER"] = i.ToString();
                    i++;
                    dtResult.Rows.Add(thisRow.ItemArray);
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);
                return null;
            }

        }

     
        #endregion TableRowsSort

    
 
        #region GetErrMsg
        
        public static void GetErrMsg(string strErrorCode, ref  string strErrDes_en, ref string strErrDes_vn)
        {

            DataSet ds = new DataSet();
            try
            {                
                //string strPath = ConfigurationManager.AppSettings[Constants.ERRORS_XML].ToString();                
                //ds.ReadXml(strPath);

                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ErrorMsg.xml";
                ds.ReadXml(filePath);


                if (ds == null)
                {
                    strErrDes_en = "System error";
                    strErrDes_vn = "Dịch vụ tạm thời gián đoạn";
                    return;
                }

                DataRow[] dr;
                dr = ds.Tables["Ebank"].Select(" ErrorCode = '" + strErrorCode + "'");

                if (dr.GetLength(0) <= 0)
                {
                    strErrDes_en = "System error";
                    strErrDes_vn = "Dịch vụ tạm thời gián đoạn";
                    return;
                }

                strErrDes_en = dr[0]["ErrorMsg_en"].ToString();
                strErrDes_vn = dr[0]["ErrorMsg_vn"].ToString();                

            }
            catch 
            {

                strErrDes_en = "System error";
                strErrDes_vn = "Dịch vụ tạm thời gián đoạn";
            }
            finally
            {
                ds.Dispose();
            }
        }

        #endregion GetErrMsg

        #region ValidatePhone

    
        #endregion 

        #region GetErrMsg

        public static string GetErrMsg(string strErrorCode, bool isLnNull)
        {
            string strErrDes = "";
            DataSet ds = new DataSet();
            try
            {                
                //string strPath = ConfigurationManager.AppSettings[Constants.ERRORS_XML].ToString();                
                //ds.ReadXml(strPath);

                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ErrorMsg.xml";
                ds.ReadXml(filePath);

                if (ds == null)
                {
                    if (isLnNull)
                    {
                        strErrDes = "System error. You can do transaction again!";
                    }
                    else
                    {
                        strErrDes = "Dịch vụ tạm thời gián đoạn.Quý khách vui lòng thực hiện lại.";
                    }

                    return strErrDes;
                }

                DataRow[] dr;
                dr = ds.Tables["Ebank"].Select(" ErrorCode = '" + strErrorCode + "'");

                if (dr.GetLength(0) <= 0)
                {
                    if (isLnNull)
                    {
                        strErrDes = "System error. You can do transaction again!";
                    }
                    else
                    {
                        strErrDes = "Dịch vụ tạm thời gián đoạn. Quý khách vui lòng thực hiện lại.";
                    }

                    return strErrDes;
                }

                if (isLnNull)
                {
                    strErrDes = dr[0]["ErrorMsg_en"].ToString();
                }
                else
                {
                    strErrDes = dr[0]["ErrorMsg_vn"].ToString();
                }

                return strErrDes;               
            }
            catch
            {
                if (isLnNull)
                {
                    strErrDes = "System error. You can do transaction again!";
                }
                else
                {
                    strErrDes = "Dịch vụ tạm thời gián đoạn. Quý khách vui lòng thực hiện lại.";
                }

                return strErrDes;
            }
            finally
            {
                ds.Dispose();
            }
        }

        public static string GetErrMsg(string strErrorCode, string strLanguage)
        {
            string strErrDes = "";
            DataSet ds = new DataSet();
            try
            {               
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\AlertCode.xml";
                ds.ReadXml(filePath);

                if (ds == null)
                {
                    if (strLanguage == "EN")
                    {
                        strErrDes = "System error. You can do transaction again!";
                    }
                    else
                    {
                        strErrDes = "Dịch vụ tạm thời gián đoạn.Quý khách vui lòng thực hiện lại.";
                    }

                    return strErrDes;
                }

                DataRow[] dr;
                dr = ds.Tables["ALERTCODE"].Select(" ALERTCD = '" + strErrorCode + "'");

                if (dr.GetLength(0) <= 0)
                {
                    if (strLanguage == "EN")
                    {
                        strErrDes = "System error. You can do transaction again!";
                    }
                    else
                    {
                        strErrDes = "Dịch vụ tạm thời gián đoạn. Quý khách vui lòng thực hiện lại.";
                    }

                    return strErrDes;
                }

                strErrDes = dr[0][strLanguage + "CAPTION"].ToString();

                return strErrDes;
            }
            catch
            {
                if (strLanguage == "EN")
                {
                    strErrDes = "System error. You can do transaction again!";
                }
                else
                {
                    strErrDes = "Dịch vụ tạm thời gián đoạn. Quý khách vui lòng thực hiện lại.";
                }

                return strErrDes;
            }
            finally
            {
                ds.Dispose();
            }
        }

       
      

        public static string GetMessage(string strMsgCode, bool isLnNull)
        {
            string strErrDes = "";
            DataSet ds = new DataSet();
            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ErrorMsg.xml";
                ds.ReadXml(filePath);

                //string strPath = ConfigurationManager.AppSettings[Constants.ERRORS_XML].ToString();
                //ds.ReadXml(strPath);

                if (ds == null)
                {
                    if (isLnNull)
                    {
                        strErrDes = "System error. You can do transaction again!";
                    }
                    else
                    {
                        strErrDes = "Dịch vụ tạm thời gián đoạn.Quý khách vui lòng thực hiện lại.";
                    }

                    return strErrDes;
                }

                DataRow[] dr;
                dr = ds.Tables["Ebank"].Select(" ErrorCode = '" + strMsgCode + "'");

                if (dr.GetLength(0) <= 0)
                {
                    if (isLnNull)
                    {
                        strErrDes = "System error. You can do transaction again!";
                    }
                    else
                    {
                        strErrDes = "Dịch vụ tạm thời gián đoạn. Quý khách vui lòng thực hiện lại.";
                    }

                    return strErrDes;
                }

                if (isLnNull)
                {
                    strErrDes = dr[0]["ErrorMsg_en"].ToString();
                }
                else
                {
                    strErrDes = dr[0]["ErrorMsg_vn"].ToString();
                }

                return strErrDes;
            }
            catch
            {
                if (isLnNull)
                {
                    strErrDes = "System error. You can do transaction again!";
                }
                else
                {
                    strErrDes = "Dịch vụ tạm thời gián đoạn. Quý khách vui lòng thực hiện lại.";
                }

                return strErrDes;
            }
            finally
            {
                ds.Dispose();
            }
        }        

        #endregion GetErrMsg

        #region GetCurrency

        //public static string GetMessage(string streKey)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        streKey = streKey.Trim();

        //        ds.ReadXml(HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + @"App_GlobalResources\\AllCode.xml");
        //        DataRow[] dr;
        //        dr = ds.Tables["SBCURRENCY"].Select(" SHORTCD = '" + streKey + "'");

        //        return dr[0]["CCYCD"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);
        //        return "";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        //public static DataTable GetListCurr(string strTblName)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\\AllCode.xml");

        //        return ds.Tables[strTblName];
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);

        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}      

        //public static string GetAllCode(string strCDType, string strCDName, string strCDVal, string Lang)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        string strMsg = string.Empty;

        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\\AllCode.xml");

        //        DataRow[] dr;
        //        dr = ds.Tables["ALLCODE"].Select("CDTYPE = '" + strCDType + "' AND CDNAME='" + strCDName + "' AND CDVAL='" + strCDVal + "'");

        //        if (ds != null)
        //        {
        //            if (Lang == "EN")
        //                strMsg = dr[0]["CONTENT_EN"].ToString();
        //            else
        //                strMsg = dr[0]["CONTENT_VN"].ToString();
        //        }

        //        return strMsg;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);

        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}


        public static DataSet LoadDataTable(string strFileName)
        {
            DataSet ds = new DataSet();
            try
            {
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\" + strFileName  + ".XML";
                ds.ReadXml(filePath);                            

                return ds;
            }
            catch (Exception ex)
            {
                ErrorHandler.ThrowError(ex);

                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }

        /// <summary>
        /// LAY DU LIEU CUA BANG
        /// </summary>
        /// <param name="ent">TEN BANG</param>
        /// <param name="condition">MENH DE DIEU KIEN</param>
        /// <param name="strFileName">DUONG DAN FIEL XML</param>
        /// <returns></returns>
        //public static ArrayList LoadData(string ent, string condition, string stSort)
        //{
        //    ArrayList arrData = new ArrayList();
        //    try
        //    {
        //        //DataSet ds = LoadDataTable(ent);
        //        DataSet ds = new DataSet();
        //        ArrayList arrAllData = (ArrayList)HttpContext.Current.Application["ARRALLDATA"];
        //        ds = (DataSet)getValue(arrAllData, ent);
        //        if (ds == null)
        //        {
        //            ds = LoadDataTable(ent);
        //        }

        //        DataRow[] dr;
        //        if(!string.IsNullOrEmpty(stSort))
        //            dr = ds.Tables[0].Select(condition); //, stSort
        //        else
        //            dr = ds.Tables[0].Select(condition);

        //        //1. DataTable
        //        //DataTable dt = new DataTable();
        //        //dt.TableName = "AD_ALLCODE";                             
        //        //dr = ds.Tables["ALLCODE"].Select("CDTYPE = '" + strCDType + "' AND CDNAME='" + strCDName + "'");

        //        //for (int i = 0; i < ds.Tables["ALLCODE"].Columns.Count; i++)
        //        //{
        //        //    dt.Columns.Add(ds.Tables["ALLCODE"].Columns[i].ColumnName);
        //        //}
        //        //foreach (DataRow orderRows in dr)
        //        //{
        //        //    dt.ImportRow(orderRows);
        //        //}
        //        //return dt;

        //        //2. ARRAYLIST
        //        ArrayList arrHeader = new ArrayList();
        //        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
        //        {
        //            arrHeader.Add(ds.Tables[0].Columns[i].ColumnName);
        //        }
        //        arrData.Add(arrHeader);

        //        foreach (DataRow aRow in dr)
        //        {
        //            ArrayList arrDetail = new ArrayList();

        //            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
        //            {
        //                arrDetail.Add(aRow[i]);
        //            }
        //            arrData.Add(arrDetail);
        //        }
               
        //        return arrData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }          
        //}

        public static ArrayList LoadXMData(string ent, string condition, string stSort)
        {
            ArrayList arrData = new ArrayList();
            try
            {
                //DataSet ds = LoadDataTable(ent);
                DataSet ds = LoadDataTable(ent);
            
                DataRow[] dr;
                if (!string.IsNullOrEmpty(stSort))
                    dr = ds.Tables[0].Select(condition); //, stSort
                else
                    dr = ds.Tables[0].Select(condition);

                //1. DataTable
                //DataTable dt = new DataTable();
                //dt.TableName = "AD_ALLCODE";                             
                //dr = ds.Tables["ALLCODE"].Select("CDTYPE = '" + strCDType + "' AND CDNAME='" + strCDName + "'");

                //for (int i = 0; i < ds.Tables["ALLCODE"].Columns.Count; i++)
                //{
                //    dt.Columns.Add(ds.Tables["ALLCODE"].Columns[i].ColumnName);
                //}
                //foreach (DataRow orderRows in dr)
                //{
                //    dt.ImportRow(orderRows);
                //}
                //return dt;

                //2. ARRAYLIST
                ArrayList arrHeader = new ArrayList();
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    arrHeader.Add(ds.Tables[0].Columns[i].ColumnName);
                }
                arrData.Add(arrHeader);

                foreach (DataRow aRow in dr)
                {
                    ArrayList arrDetail = new ArrayList();

                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        arrDetail.Add(aRow[i]);
                    }
                    arrData.Add(arrDetail);
                }

                return arrData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ArrayList DataSet2ArrayList(DataSet ds)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                DataRow[] dr;
                dr = ds.Tables[0].Select(); //, stSort
           
                //2. ARRAYLIST
                ArrayList arrHeader = new ArrayList();
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    arrHeader.Add(ds.Tables[0].Columns[i].ColumnName);
                }
                arrData.Add(arrHeader);

                foreach (DataRow aRow in dr)
                {
                    ArrayList arrDetail = new ArrayList();

                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        arrDetail.Add(aRow[i]);
                    }
                    arrData.Add(arrDetail);
                }

                return arrData;
            }
            catch
            {
                return null;
            }

        }

        //public static DataTable GetDataTable(string strFileName)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\\" + strFileName);

        //        dt = ds.Tables[0];

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);

        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}


        //public static string Cidtad2BBank(string strBankID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = GetDataTable("SMLCITDCD2BBANK.xml");

        //        DataRow[] dr;
        //        dr = dt.Select("BANK_ID = '" + strBankID + "'");

        //        string strBBank = string.Empty;

        //        if (dr.Length > 0)
        //        {
        //            strBBank = dr[0]["BBANK"].ToString();
        //        }
        //        return strBBank;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);
        //        return null;
        //    }
        //    finally
        //    {
        //    }
        //}

        //public static string Cidtad2BBankName(string strBankID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = GetDataTable("SMLCITDCD2BBANK.xml");

        //        DataRow[] dr;
        //        dr = dt.Select("BANK_ID = '" + strBankID + "'");

        //        string strBBank = string.Empty;

        //        if (dr.Length > 0)
        //        {
        //            strBBank = dr[0]["BBANKNAME"].ToString();
        //        }
        //        return strBBank;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);
        //        return null;
        //    }
        //    finally
        //    {
        //    }
        //}

        //public static DataTable GetAllCode(string strCDType, string strCDName)
        //{
        //    DataSet ds = new DataSet();

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt.TableName = "ALLCODE";

        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\\AllCode.xml");

        //        DataRow[] dr;
        //        dr = ds.Tables["ALLCODE"].Select("CDTYPE = '" + strCDType + "' AND CDNAME='" + strCDName + "'");

        //        for (int i = 0; i < ds.Tables["ALLCODE"].Columns.Count; i++)
        //        {
        //            dt.Columns.Add(ds.Tables["ALLCODE"].Columns[i].ColumnName);
        //        }

        //        foreach (DataRow orderRows in dr)
        //        {
        //            dt.ImportRow(orderRows);
        //        }

        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);

        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        //public static string GetAllCodeContent(string strCDType, string strCDName, string strCDVal, string strLang)
        //{
        //    DataSet ds = new DataSet();
        //    string strContent = strCDVal;

        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt.TableName = "ALLCODE";

        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\\AllCode.xml");

        //        DataRow[] dr;
        //        dr = ds.Tables["ALLCODE"].Select("CDTYPE = '" + strCDType + "' AND CDNAME='" + strCDName + "' AND CDVAL='" + strCDVal + "'");

        //        if (dr != null & dr.Length > 0)
        //            strContent = dr[0]["CONTENT_" + strLang].ToString();

        //        return strContent;

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ThrowError(ex);

        //        return "";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        public static DataTable GetCurrCode(string strCDType, string strCDName, string strCDVal)
        {
            DataSet ds = new DataSet();
            
            try
            {
                //if (string.IsNullOrEmpty(strCDVal))
                //    strCDVal = "00";

                //string strMsg = string.Empty;

                //string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
                //if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
                //{
                //    strFilePath += "\\";
                //}

                //ds.ReadXml(strFilePath + @"App_GlobalResources\AllCode.xml");

                //DataRow[] dr;
                //dr = ds.Tables["ALLCODE"].Select("CDTYPE = '" + strCDType + "' AND CDNAME='" + strCDName + "' AND CDVAL='" + strCDVal + "'");

                ArrayList arrCurrCD = new ArrayList();
                ArrayList arrSubDetail = new ArrayList();
                arrSubDetail.Add("CDVAL");
                arrSubDetail.Add("CAPTION");
                arrCurrCD.Add(arrSubDetail);

                if (ds != null)
                {
                    arrSubDetail = new ArrayList();
                    arrSubDetail.Add(strCDVal);
                    arrSubDetail.Add(strCDVal);                  
                    arrCurrCD.Add(arrSubDetail);
                }                

                return ArrayList2DataTable(arrCurrCD);
            }            
            catch (Exception ex)
            {                
                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }

        //public static string ConvertCCycd2Ccy(string strCcyCD)
        //{
        //    DataSet ds = new DataSet();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(strCcyCD))
        //            return "";

        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\AllCode.xml");

        //        DataRow[] dr;
        //        dr = ds.Tables["ALLCODE"].Select("CDTYPE = 'CCYCD' AND CDNAME='CCYCD' AND CDVAL='" + strCcyCD + "'");

        //        ArrayList arrCurrCD = new ArrayList();
        //        ArrayList arrSubDetail = new ArrayList();
        //        arrSubDetail.Add("CDVAL");
        //        arrSubDetail.Add("CONTENT_VN");
        //        arrCurrCD.Add(arrSubDetail);

        //        if (ds != null)return dr[0]["CONTENT_VN"].ToString();
        //        else return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        public static string  GetValue(DataTable dt, string strColName, string strColVal,string strRrColName)
        {            
            try
            {
                DataRow[] dr = dt.Select("" + strColName + " = '" + strColVal + "'");

                if (dr != null)
                {
                    return CString(dr[0][strRrColName]);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }          
        }

        //public static DataTable GetCurrCode()
        //{
        //    DataSet ds = new DataSet();

        //    try
        //    {
        //        string strMsg = string.Empty;

        //        string strFilePath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
        //        if (strFilePath.Substring(strFilePath.Length - 1, 1) != "\\")
        //        {
        //            strFilePath += "\\";
        //        }

        //        ds.ReadXml(strFilePath + @"App_GlobalResources\\AllCode.xml");

        //        DataRow[] dr;
        //        dr = ds.Tables["SBCURRENCY"].Select("ACTIVE = '1' ", "ORD");

        //        ArrayList arrCurrCD = new ArrayList();
        //        ArrayList arrSubDetail = new ArrayList();
        //        arrSubDetail.Add("CDVAL");
        //        arrSubDetail.Add("CONTENT_VN");
        //        arrCurrCD.Add(arrSubDetail);

        //        if (ds != null)
        //        {
        //            for (int i = 0; i < dr.GetLength(0); i++)
        //            {
        //                arrSubDetail = new ArrayList();
        //                arrSubDetail.Add(dr[i]["SHORTCD"].ToString());
        //                arrSubDetail.Add(dr[i]["CCYCD"].ToString());
        //                arrCurrCD.Add(arrSubDetail);
        //            }
        //        }

        //        return ArrayList2DataTable(arrCurrCD);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        public static string GetRespMsg(string strRespCode, bool isEng)
        {

            DataSet ds = new DataSet();

            try
            {
                string strMsg = string.Empty;

                if(string.IsNullOrEmpty(strRespCode))
                    return strMsg;

                //29-03-212
                //string strPath = ConfigurationManager.AppSettings["RESPONE_CODE"].ToString();
                //ds.ReadXml(strPath);
                string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                filePath = Path.GetDirectoryName(filePath) + "\\Resource\\ResponseCode.xml";
                ds.ReadXml(filePath);

               
                DataRow[] dr;
                dr = ds.Tables["RESP_CODE"].Select("RespCode = '" + strRespCode + "'");
               
                if (ds != null)
                {
                    if (isEng == true)
                        strMsg = dr[0]["RespMsg_en"].ToString();
                    else
                        strMsg = dr[0]["RespMsg_vn"].ToString();                  
                }

                return strMsg;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }

        #endregion GetCurrency

        #region GetRandomText

        public static string GetRandomText()
        {
            try
            {
                string uniqueID = Guid.NewGuid().ToString();
                string randString = "";
                Random ran = new Random();
                int iLengTextVerify = 6; //canhdm

                for (int i = 0, j = 0; i < uniqueID.Length && j < iLengTextVerify; i++)
                {
                    char l_ch = uniqueID.ToCharArray()[i];
                    if (((l_ch >= 'A' && l_ch <= 'Z') || (l_ch >= 'a' && l_ch <= 'z') || (l_ch >= '0' && l_ch <= '9')) && l_ch != '1' && l_ch != 'l')
                    {
                        int r = ran.Next();
                        if (r % 2 == 0)
                            randString += l_ch.ToString();
                        else
                            randString += l_ch.ToString().ToUpper();
                        j++;
                    }
                }
                return randString;
            }
            catch (Exception ex)
            {
                ErrorHandler.ProcessErr(ex, Constants.ERROR_TYPE_eBANKWEBUI, ErrorHandler.EBANK_SYSTEM_ERROR);
                return null;
            }
        }

        #endregion

        //public static Control GetItem(string Tag, Control Container)
        //{
        //    try
        //    {
        //        Control item;                
        //        foreach (Control ctl in Container.Controls)
        //        {
        //            if (ctl.GetType().ToString() == "System.Windows.Forms.Panel"                    
        //                || ctl.GetType().ToString() == "System.Windows.Forms.GroupBox")
        //            {
        //                item = GetItem(Tag, ctl);
        //                if (item != null)
        //                {
        //                    return item;
        //                }
        //            }
        //            else if (ctl.GetType().ToString().IndexOf("Label") == -1)
        //            {
        //                if (ctl.ID != null)
        //                {
        //                    if (ctl.ID.ToString().ToUpper() == Tag.ToUpper())
        //                    {
        //                        return ctl;
        //                    }
        //                }
        //            }
        //        }

        //        return null;
        //    }
        //    catch (WB.SYSTEM.ErrorMessage ex)
        //    {
        //        WB.SYSTEM.ErrorHandler.Process(ex);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        WB.SYSTEM.ErrorHandler.Process(ex);
        //        return null;
        //    }
        //}

        //public static string GetItemValue(string Tag, System.Web.UI.Page page)
        //{
        //    try
        //    {
        //        Control item = GetItem(Tag, (Control)page);
                
        //        if (item != null)
        //        {
        //            string ItemValue = string.Empty;

        //            if (item.GetType().ToString() == "System.Web.UI.WebControls.TextBox" && item.ID.ToString() == Tag)
        //            {                        
        //                ItemValue = ((TextBox)item).Text;
        //            }
        //            if (item.GetType().ToString() == "System.Web.UI.WebControls.DropDownList" && item.ID.ToString() == Tag)
        //            {
        //                DropDownList cbo = (DropDownList)item;
                        
        //                if (cbo.Items.Count > 0 && cbo.SelectedValue != null)
        //                    ItemValue = cbo.SelectedValue.ToString().Trim();
        //            }
                   
        //            return ItemValue.Trim();
        //        }

        //        return "";
        //    }
        //    catch (WB.SYSTEM.ErrorMessage ex)
        //    {
        //        WB.SYSTEM.ErrorHandler.Process(ex);
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        WB.SYSTEM.ErrorHandler.Process(ex);
        //        return "";
        //    }
        //}

   
        public static bool Like2Msg(ArrayList arrData, string strFName, string strFValue)
        {
            DataTable dt = new DataTable();            
            try
            {                
                dt = SysUtils.ArrayList2DataTable(arrData);                            

                DataRow[] dr;
                dr = dt.Select(strFName + " LIKE '%" + strFValue + "%'");

                //CASE1: NOT FOUND OR VALIDATE strDes 
                if (dr.GetLength(0) == 0) //|| dr.GetLength(0) > 1
                {
                    return false;
                }
                else
                {                   
                    return true;
                }             
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return false;
            }
            finally
            {
                dt.Dispose();                
            }
        }


        #region SaveTextToFile

        public static bool SaveTextToFile(string p_sData, string p_FullPath, string p_ErrInfo = "")
        {
            bool bAns = false;
            StreamWriter objReader = default(StreamWriter);
            try
            {
                objReader = new StreamWriter(p_FullPath);
                objReader.Write(p_sData);
                objReader.Close();
                bAns = true;
            }
            catch (Exception Ex)
            {
                p_ErrInfo = Ex.Message;

            }
            return bAns;
        }

        public static bool SaveTextToFile(string p_sData, string fullPath, string fileName, string strExt)
        {
            bool bAns = false;
            StreamWriter objReader = default(StreamWriter);
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    fileName = "LOG";
                if (string.IsNullOrEmpty(fileName))
                    fileName = ".txt";
                if (string.IsNullOrEmpty(fullPath))
                {
                    fullPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                    fullPath = Path.GetDirectoryName(fullPath) + "\\" + fileName + strExt;
                }
                else
                    fullPath = fullPath + "\\" + fileName + strExt;

                objReader = new StreamWriter(fullPath);
                objReader.Write(p_sData);
                objReader.Close();
                bAns = true;
            }
            catch (Exception Ex)
            {

            }
            return bAns;
        }

        #endregion

        public static ArrayList Text2ArrayList(string strFilePath)
        {
            try
            {
                ArrayList arrResult = new ArrayList();

                using (StreamReader reader = new StreamReader(strFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        arrResult.Add(line);
                    }
                }

                return arrResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ArrayList Text2ArrayList2(string fullPath, string fileName)
        {
            try
            {
                ArrayList arrResult = new ArrayList();

                if (string.IsNullOrEmpty(fullPath))
                {
                    fullPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                    fullPath = Path.GetDirectoryName(fullPath) + "\\" + fileName;
                }

                string strResult = "";
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        strResult += line;
                    }
                }

                arrResult.Add(strResult);
                return arrResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strSymBolSp"> CHUOI DUNG DE TACH</param>
        /// <returns></returns>
        public static ArrayList Text2ArrayList(string strFilePath, string strSymBolSp)
        {
            ArrayList arrResult = new ArrayList();

            try
            {              
                using (StreamReader reader = new StreamReader(strFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if(!string.IsNullOrEmpty(line))
                        {
                            string[] strArr = line.Split(char.Parse("="));
                            arrResult.Add(strArr[0]);
                            arrResult.Add(strArr[1]);
                        }
                    }
                }

                return arrResult;
            }
            catch (Exception ex)
            {
                return arrResult;
            }
        }

        #region GetFileContents

        public static string GetFileContents(string p_FullPath)
        {
            string strContents = null;
            StreamReader objReader = default(StreamReader);
            if (!System.IO.File.Exists(p_FullPath))
            {
                throw new Exception("path is null");
            }
            try
            {
                objReader = new StreamReader(p_FullPath);
                strContents = objReader.ReadToEnd();
                objReader.Close();
                return strContents;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion

        //public static ArrayList LoadDataTbl(string RptCode, string strKey)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        string strPath = Assembly.GetExecutingAssembly().Location;
        //        strPath = strPath.Replace("WB.SystemLibrary.dll", "");
        //        strPath = strPath.Replace("wb.systemlibrary.dll", "");
        //        strPath = strPath.Replace("WB.SystemLibrary.DLL", "");
        //        strPath = strPath.Replace("wb.systemlibrary.DLL", "");

        //        //LOAD FROM XML FILE              
        //        try
        //        {
        //            ds.ReadXml(strPath + @"\ListTemplateFile.xml");
        //        }
        //        catch
        //        {
        //            strPath = ConfigurationManager.AppSettings["TemplatePath"].ToString();
        //            ds.ReadXml(strPath + @"\ListTemplateFile.xml");
        //        }

        //        //GET DATATABLE
        //        DataTable dataTable = ds.Tables[RptCode];

        //        //CONVERT 2 ARR:                
        //        return DataTable2ArrayList(dataTable);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}

        public static ArrayList Tag2Caption(ArrayList arrHeader, ArrayList arrLanguage, string strLang)
        {
            try
            {
                for (int i = 0; i < arrHeader.Count; i++)
                {
                    string strCaption = string.Empty;

                    for (int j = 1; j < arrLanguage.Count; j++)
                    {
                        if (SysUtils.getProperty((ArrayList)arrLanguage[0], (ArrayList)arrLanguage[j], "TAG").ToUpper() == arrHeader[i].ToString().ToUpper())
                        {
                            strCaption = SysUtils.getProperty((ArrayList)arrLanguage[0], (ArrayList)arrLanguage[j], strLang + "CAPTION");
                            arrHeader[i] = strCaption;
                            break;
                        }
                    }

                }

                return arrHeader;
            }
            catch (WB.SYSTEM.ErrorMessage e)
            {
                WB.SYSTEM.ErrorHandler.Process(e);
                return null;
            }
            catch (Exception e)
            {
                WB.SYSTEM.ErrorHandler.Process(e);
                return null;
            }
        }

        public static ArrayList GetParmURL(string strURL, bool isEncrypt, string strKey)
        {
            ArrayList arrParm = new ArrayList();

            try
            {
                if (!isEncrypt)
                {
                    URLEncrypt csURLEncrypt = new URLEncrypt(strURL);
                    arrParm = csURLEncrypt.GetParam(false);

                    if (arrParm == null || arrParm.Count == 0 || (strURL.IndexOf("?") >= 0 && arrParm.Count == 2 && SysUtils.CString(arrParm[1]) == ""))
                        arrParm = GetParmURL(strURL, true, strKey);
                }
                else
                {
                    URLEncrypt csURLEncrypt = new URLEncrypt(strURL, strKey);
                    arrParm = csURLEncrypt.GetParam(true);
                }         

                return arrParm;
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
        }

        public static ArrayList GetParam(string strURL)
        {
            try
            {
                //string sUrl = null;
                string[] QArr;
                int i;
                int iCount;
                ArrayList arrResult = new ArrayList();

                int intLenPath = strURL.IndexOf("?");
                if (intLenPath >= 0)
                {
                    QArr = strURL.Substring(intLenPath + 1, strURL.Length - (intLenPath + 1)).Split(new char[] { '&' });

                    iCount = QArr.GetUpperBound(0);
                    for (i = 0; i <= iCount; i++)
                    {
                        string[] pArr;
                        pArr = QArr[i].Split(new char[] { '=' });

                        arrResult.Add(pArr[0].ToUpper());
                        arrResult.Add(pArr[1]);
                    }
                }

                return arrResult;
            }
            catch (Exception ex)
            {
                ErrorHandler.Process(ex);
                return null;
            }
        }

        /// <summary>
        /// User SYSDATADIC to maping header 2 header
        /// </summary>
        /// <param name="arrDataDic"></param>
        /// <param name="arrHeader"></param>
        /// <returns></returns>
        public static ArrayList MapHeader2Header(ArrayList SysDataDic, ArrayList arrData)
        {
            try
            {
                ArrayList arrHeaderOrg = (ArrayList)arrData[0];

                if (SysDataDic.Count > 1)
                {
                    ArrayList arrHeader = (ArrayList)SysDataDic[0];

                    for (int i = 1; i < SysDataDic.Count; i++)
                    {
                        ArrayList arrDetail = (ArrayList)SysDataDic[i];

                        string strDCode = SysUtils.CString(getProperty(arrHeader, arrDetail, "DCODE"));
                        string strMCode = SysUtils.CString(getProperty(arrHeader, arrDetail, "MCODE"));

                        for (int j = 0; j < arrHeaderOrg.Count; j++)
                        {
                            if (arrHeaderOrg[j].ToString().ToUpper() == strDCode.ToUpper())
                            {
                                arrHeaderOrg.RemoveAt(j);
                                arrHeaderOrg.Insert(j, strMCode);
                            }
                        }
                    }
                }

                arrData.RemoveAt(0);
                arrData.Insert(0, arrHeaderOrg);

                return arrData;
            }
            catch (WB.SYSTEM.ErrorMessage ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
            catch (Exception ex)
            {
                WB.SYSTEM.ErrorHandler.Process(ex);
                return null;
            }
        }

        public static string ConfigAppSettings(string Key, string ddlName)
        {
            try
            {
                string Location = typeof(SysUtils).Assembly.Location;
                return SysUtils.ConfigAppSettings(Location, ddlName, Key);
            }
            catch (Exception ex)
            {
                ErrorMessage objErr = new ErrorMessage();
                objErr.ErrorCode = ErrorHandler.EBANK_SYSTEM_ERROR;
                objErr.ErrorDesc = ex.Message + " at config key " + Key;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }

        public static string ConfigAppSettings(string exeConfigPath, string Host, string Key)
        {
            try
            {
                string strResult = null;

                Configuration config = null;
                string strPath = Path.GetDirectoryName(exeConfigPath);
                if (!string.IsNullOrEmpty(Host)) strPath += "\\" + Host;
                config = ConfigurationManager.OpenExeConfiguration(strPath);
                strResult = config.AppSettings.Settings[Key].Value;

                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable Json2Table(string jsonString)
        {
            DataTable myTable = new DataTable();

            try
            {
                
                jsonString = jsonString.TrimStart('['); 
                jsonString = jsonString.TrimEnd(']');
                string[] jsonParts = jsonString.Split("],[");

                int i = 0;
                int j = 0;
                string row = jsonParts[0];
                string[] col = row.Split("\",\"");

                for (i = 0; i < col.GetLength(0); i++)
                {
                    myTable.Columns.Add(col[i].ToString().Replace("\"",""));
                }

                for (i = 1; i < jsonParts.GetLength(0); i++)
                {                    
                    col = jsonParts[i].Split("\",\"");

                    DataRow aRow = myTable.NewRow();

                    for (j = 0; j < col.GetLength(0); j++)
                    {
                        aRow[j] = CString(col[j]);
                    }

                    myTable.Rows.Add(aRow);
                }

                return myTable;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable Json2Table2(string json)
        {
            try
            {
                var jsonLinq = JObject.Parse(json);
                JObject obj = JObject.Parse(json);
                //JArray arrBody = (JArray)obj["Body"];

                // Find the first array using Linq
                var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
                var trgArray = new JArray();
                foreach (JObject row in srcArray.Children<JObject>())
                {
                    var cleanRow = new JObject();
                    foreach (JProperty column in row.Properties())
                    {
                        // Only include JValue types
                        if (column.Value is JValue)
                        {
                            cleanRow.Add(column.Name, column.Value);
                        }
                    }

                    trgArray.Add(cleanRow);
                }

                return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
