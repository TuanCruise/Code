using System;
using System.Collections.Generic;
using System.Text;
using WB.SYSTEM;
using System.Data;
using System.Collections;
using System.Configuration;
//using WB.HostDAL;
using System.Xml;
using Newtonsoft.Json.Converters;
using WB.SystemLibrary;

namespace XMLBuilder
{

    public class XMLDefinition 
    {

        #region Dispose
        // ====================================================================
        // This member overrides Object.Finalize.
        // ====================================================================
        ~XMLDefinition()
        {
            Dispose();
        }

        // ====================================================================
        // Releases all resources used by this object.
        // ====================================================================
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion Dispose

        #region Variables

        public ArrayList m_ArrTxn = new ArrayList();
        public string m_MCode = string.Empty;
        
        /*
        private DBManager _dbManager;

        public DBManager dbManager
        {
            get { return _dbManager; }
            set { _dbManager = value; }
        }     
        */
        #endregion Variables             

        public XMLDefinition()
        {          
        }

       

        #region BuildXML

        public string ArrayList2XMLMsg(ArrayList arrData, string strMsgType, bool blMultiRow)
        {
            string StringXML = "";
            try
            {
                //StringXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";                
                //StringXML += "<Input>";
                //StringXML += "\n";
                //StringXML += BuildHeaderXML(arrData);

                //StringXML += "\n";
                StringXML += BuildBodyXML(arrData, strMsgType, blMultiRow);               
                //StringXML += "\n";
                //StringXML += "</Input>";

                //Sign:
                //StringXML += "\n";
                //StringXML += BuildSecurityXML(arrData);

                return StringXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BuildXMLField(ArrayList arrData, string strMsgType, string strTable, bool blMultiRow)
        {
            try
            {
                string strSQL = "";
                ArrayList arrRow = new ArrayList();
                if (blMultiRow)
                {
                    arrRow = arrData;
                }
                else
                {
                    arrRow.Add(arrData);
                }

                //Chuyen doi mang du lieu thanh bang
                DataTable dt = new DataTable();
                string strParent = "";
                DataSet ds = new DataSet();
                ds.DataSetName = strTable;

                string debugMode = SysUtils.CString(ConfigurationManager.AppSettings["DEBUGMODE"]);

                if (debugMode == "Y")
                {                                      
                   
                }
                else
                {
                    ArrayList arrMsgField = SysUtils.LoadXMData("XML_MSGFIELD_TAG", "MSGTYPE='" + strMsgType + "'", "TAGORD");
                    int intBLevel = 0;
                    if (arrMsgField.Count > 1)
                    {
                        for (int k = 1; k < arrMsgField.Count; k++)
                        {
                            ArrayList arrHeader = (ArrayList)arrMsgField[0];
                            ArrayList arrDtl = (ArrayList)arrMsgField[k];

                            string strTag = SysUtils.CString(SysUtils.getProperty(arrHeader, arrDtl, "TAG"));
                            string strPDefault = SysUtils.CString(SysUtils.getProperty(arrHeader, arrDtl, "PDEFVALUE"));
                            string strDefault = SysUtils.CString(SysUtils.getProperty(arrHeader, arrDtl, "DEFVALUE"));
                            if (SysUtils.CString(SysUtils.getProperty(arrHeader, arrDtl, "TAGTYPE")) == "N") //rd["TAGTYPE"].ToString()
                            {

                                dt.Dispose();
                                dt = new DataTable();
                                dt.TableName = strTag;// rd["TAG"].ToString();

                                //Parent column
                                //strParent = rd["PARENTTAGCD"].ToString();
                                strParent = SysUtils.CString(SysUtils.getProperty(arrHeader, arrDtl, "PARENTTAGCD"));
                                if (strParent != "")
                                {
                                    dt.Columns.Add(strParent);
                                    if (!string.IsNullOrEmpty(strPDefault)) //rd["PDEFVALUE"]
                                    {
                                        dt.Columns[strParent].DefaultValue = strPDefault;// rd["PDEFVALUE"].ToString();
                                    }
                                    else
                                    {
                                        dt.Columns[strParent].DefaultValue = "";
                                    }
                                }

                                intBLevel++;
                            }
                            else  //Add column
                            {
                                dt.Columns.Add(strTag);//rd["TAG"].ToString()
                                //if (rd["DEFVALUE"] != null && rd["DEFVALUE"].ToString() != "")
                                if (!string.IsNullOrEmpty(strDefault))
                                {
                                    dt.Columns[strTag].DefaultValue = strDefault; //rd["DEFVALUE"].ToString(); rd["TAG"].ToString()
                                }
                                else
                                {
                                    dt.Columns[strTag].DefaultValue = ""; //rd["TAG"].ToString()
                                }
                            }

                            //if (rd["LASTCOL"].ToString() == "Y")
                            if (SysUtils.CString(SysUtils.getProperty(arrHeader, arrDtl, "LASTCOL")) == "Y")
                            {
                                //Add value
                                for (int i = 0; i < arrRow.Count; i++)
                                {
                                    ArrayList arrDetail = (ArrayList)arrRow[i];
                                    dt.Rows.Add();
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        string columnName = SysUtils.CString(dt.Columns[j].ColumnName);
                                        string fromat = SysUtils.CString(dt.Columns[j].ColumnName);

                                        if (arrDetail.IndexOf(columnName) != -1)
                                        {
                                            dt.Rows[i][columnName] = SysUtils.CString(SysUtils.getValue(arrDetail, columnName));
                                        }
                                    }
                                }
                                //distinct data
                                DataTable dtDist = dt.DefaultView.ToTable(true);

                                //save to dataset
                                ds.Tables.Add(dtDist);
                                //create relationship
                                if (strParent != "" && ds.Tables.Count > 1)
                                {
                                    DataRelation drel = new DataRelation("Relation" + intBLevel.ToString(), ds.Tables[ds.Tables.Count - 2].Columns[strParent], ds.Tables[ds.Tables.Count - 1].Columns[strParent]);
                                    drel.Nested = true;
                                    ds.Relations.Add(drel);
                                    ds.Tables[ds.Tables.Count - 1].Columns[strParent].ColumnMapping = MappingType.Hidden;
                                }
                            }
                        }
                    }

                }

                //Add du lieu
                if (ds == null)
                {
                    ErrorMessage objErr = new ErrorMessage();
                    objErr.ErrorCode = ErrorHandler.EBANK_SYSTEM_ERROR;
                    objErr.ErrorSource = "GIPDefinition.BuildXML";
                    ErrorHandler.ThrowError(objErr);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BuildBodyXML(ArrayList arrData, string strMsgType, bool blMultiRow)
        {
            string StringXML = "";
            try
            {
                DataSet ds = BuildXMLField(arrData, strMsgType, "BODY", blMultiRow);

                StringXML = ds.GetXml();
                StringXML = StringXML.Replace("<BODY>", "");
                StringXML = StringXML.Replace("</BODY>", "");

                SysUtils.SaveTextToFile(StringXML, null, null, ".XML");
                return StringXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BuildHeaderXML(ArrayList arrData)
        {
            string StringXML = "";
            try
            {
                DataSet ds = BuildXMLField(arrData, "00000", "HEADER", false);

                StringXML = ds.GetXml();
                StringXML = StringXML.Replace("<Table1>", "");
                StringXML = StringXML.Replace("</Table1>", "");
                return StringXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BuildSecurityXML(ArrayList arrData)
        {
            string StringXML = "";
            try
            {
                DataSet ds = BuildXMLField(arrData, "99999", "SECURITY", false);

                StringXML = ds.GetXml();
                return StringXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion BuildXML

        public ArrayList XMLMsg2ArrayList(string strMessage)
        {
            try
            {
                ArrayList arrReturn = new ArrayList();
                if (strMessage != "")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strMessage);
                    foreach (XmlNode root in doc.ChildNodes)
                    {
                        //if (root.Name == "DATA")
                        if (root.Name == "root" || root.Name == "DATA" || root.Name == "packages") //ROOT NAME
                        {
                            foreach (XmlNode node in root.ChildNodes)
                            {
                                ArrayList arr1 = ConvertXMLtoArr(node);
                                arrReturn.Add(node.Name);
                                arrReturn.Add(arr1);
                            }
                            break;
                        }
                    }
                }
                return arrReturn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ArrayList ConvertXMLtoArr(XmlNode root)
        {
            try
            {
                ArrayList arr = new ArrayList();
                arr.Add(new ArrayList());//name
                arr.Add(new ArrayList());//value

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.HasChildNodes)
                    {
                        ArrayList arr1 = ConvertXMLtoArr(node);
                        if (((ArrayList)arr[0]).Count == 0)
                        {
                            arr = new ArrayList();
                            for (int i = 0; i < arr1.Count; i++)
                            {
                                arr.Add(arr1[i]);
                            }
                        }
                        else
                        {
                            int j = ((ArrayList)arr[0]).IndexOf(((ArrayList)arr1[0])[0]);
                            //Chua co thi them cot
                            if (j == -1)
                            {
                                int iEC = ((ArrayList)arr[0]).Count;
                                int iER = arr.Count - 1;
                                //Add column name
                                for (int i = 0; i < ((ArrayList)arr1[0]).Count; i++)
                                {
                                    ((ArrayList)arr[0]).Add(((ArrayList)arr1[0])[i]);
                                }
                                //Add column for first rows
                                for (int iR = 1; iR <= iER; iR++)
                                {
                                    for (int i = 0; i < ((ArrayList)arr1[0]).Count; i++)
                                    {
                                        ((ArrayList)arr[iR]).Add(((ArrayList)arr1[1])[i]);
                                    }
                                }
                                //Add next new rows
                                for (int iR = 1; iR <= iER; iR++)
                                {
                                    for (int k = 2; k < arr1.Count; k++)
                                    {
                                        ArrayList arr2 = new ArrayList();
                                        for (int i = 0; i < iEC; i++)
                                        {
                                            arr2.Add(((ArrayList)arr[iR])[i]);
                                        }
                                        for (int i = 0; i < ((ArrayList)arr1[0]).Count; i++)
                                        {
                                            arr2.Add(((ArrayList)arr1[k])[i]);
                                        }
                                        arr.Add(arr2);
                                    }
                                }
                            }
                            //Da co field thi add dong 
                            else
                            {
                                //Du lieu moi thieu cac cot dau
                                j = ((ArrayList)arr1[0]).IndexOf(((ArrayList)arr[0])[0]);
                                if (j == -1)
                                {
                                    //Xet den cot trung
                                    int h = ((ArrayList)arr[0]).IndexOf(((ArrayList)arr1[0])[0]);
                                    //Tao mang moi cot 0 den cot thieu
                                    ArrayList arr2 = new ArrayList();
                                    for (int k = 1; k < arr1.Count; k++)
                                    {
                                        arr2.Add(new ArrayList());
                                        for (int i = 0; i < h; i++)
                                        {
                                            ((ArrayList)arr2[k - 1]).Add(((ArrayList)arr[arr.Count - 1])[i]);
                                        }
                                    }
                                    //Add cot cua dl moi
                                    for (int k = 1; k < arr1.Count; k++)
                                    {
                                        for (int l = 0; l < ((ArrayList)arr1[0]).Count; l++)
                                        {
                                            ((ArrayList)arr2[k - 1]).Add(((ArrayList)arr1[k])[l]);
                                        }
                                    }
                                    //Add dong
                                    for (int k = 0; k < arr2.Count; k++)
                                    {
                                        arr.Add((ArrayList)arr2[k]);
                                    }
                                }
                                else
                                {
                                    //Kiem tra co them cot moi o giua
                                    for (int i = 0; i < ((ArrayList)arr1[0]).Count; i++)
                                    {
                                        int h = ((ArrayList)arr[0]).IndexOf(((ArrayList)arr1[0])[i]);
                                        if (h == -1)
                                        {
                                            h = ((ArrayList)arr[0]).IndexOf(((ArrayList)arr1[0])[i - 1]) + 1;
                                            for (int k = 0; k < arr.Count; k++)
                                            {
                                                ((ArrayList)arr[k]).Add("");
                                                for (int l = ((ArrayList)arr[0]).Count - 1; l > h; l--)
                                                {
                                                    ((ArrayList)arr[k])[l] = ((ArrayList)arr[k])[l - 1];
                                                }
                                                if (k == 0)
                                                    ((ArrayList)arr[0])[h] = ((ArrayList)arr1[0])[i];
                                                else
                                                    ((ArrayList)arr[k])[h] = "";
                                            }
                                        }
                                    }
                                    //Kiem tra du lieu moi thieu cot
                                    for (int i = 0; i < ((ArrayList)arr[0]).Count; i++)
                                    {
                                        int h = ((ArrayList)arr1[0]).IndexOf(((ArrayList)arr[0])[i]);
                                        if (h == -1)
                                        {
                                            h = ((ArrayList)arr1[0]).IndexOf(((ArrayList)arr[0])[i - 1]) + 1;
                                            for (int k = 0; k < arr1.Count; k++)
                                            {
                                                ((ArrayList)arr1[k]).Add("");
                                                for (int l = ((ArrayList)arr1[0]).Count - 1; l > h; l--)
                                                {
                                                    ((ArrayList)arr1[k])[l] = ((ArrayList)arr1[k])[l - 1];
                                                }
                                                if (k == 0)
                                                    ((ArrayList)arr1[0])[h] = ((ArrayList)arr[0])[i];
                                                else
                                                    ((ArrayList)arr1[k])[h] = "";
                                            }
                                        }
                                    }
                                    //Add dong du lieu
                                    for (int k = 1; k < arr1.Count; k++)
                                    {
                                        arr.Add((ArrayList)arr1[k]);
                                    }
                                }
                            }
                        }
                    }
                    else if (node.NodeType == XmlNodeType.Element)
                    {
                        ((ArrayList)arr[0]).Add(node.Name);
                        ((ArrayList)arr[1]).Add("");
                    }
                    else
                    {
                        ((ArrayList)arr[0]).Add(node.ParentNode.Name);
                        ((ArrayList)arr[1]).Add(node.Value);
                    }
                }


                return arr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Add received data
        public void AddData2Table(string strTable, string strTraceNo, ArrayList arrData)
        {
            try
            {
                ArrayList arrHeader = (ArrayList)arrData[0];
                for (int i = 1; i < arrData.Count; i++)
                {
                    ArrayList arrDetail = (ArrayList)arrData[i];
                    ArrayList arrProp = new ArrayList();
                    arrProp.Add("TRACENO");
                    arrProp.Add(strTraceNo);
                    for (int j = 0; j < arrHeader.Count; j++)
                    {
                        arrProp.Add(arrHeader[j].ToString());
                        arrProp.Add(arrDetail[j]);
                    }

                    //base.arrProperties = arrProp;
                    //base.entityName = strTable;
                    //base.Add();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Add received data

        #region jSON

        public string XML2JSon(string xml)
        {
            string jsonText = string.Empty;
            XmlNodeConverter JsonConverter = new XmlNodeConverter();
            try
            {
                //string xml = @"<?xml version='1.0' standalone='no'?>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                jsonText = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                return jsonText;
            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ProcessErr(er);
                return jsonText;
            }
            catch (Exception ex)
            {
                ErrorHandler.ProcessErr(ex, Constants.ERROR_TYPE_SMARTBANK, ErrorHandler.CORE_SBANK_ERR);
                return jsonText;
            }
            finally
            {
            }
        }

        public string JSon2XML(string json)
        {
            string strXml = string.Empty;
            try
            {
                //string xml = @"<?xml version='1.0' standalone='no'?>";
                XmlDocument doc = new XmlDocument();
                doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);
                strXml = doc.InnerXml;
                return strXml;
            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ProcessErr(er);
                return strXml;
            }
            catch (Exception ex)
            {
                ErrorHandler.ProcessErr(ex, Constants.ERROR_TYPE_SMARTBANK, ErrorHandler.CORE_SBANK_ERR);
                return strXml;
            }
            finally
            {
            }
        }

        #endregion jSON



    }
}
