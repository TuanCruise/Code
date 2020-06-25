using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using System.Xml;
using System.Data;
using System.IO;

namespace WB.SYSTEM
{
    public class XMLUtils
    {
        public XMLUtils()
		{

		}
        
        public static ArrayList XML2Arr(string sXmlString)
        {
            try
            {
                ArrayList arrOutMsg = new ArrayList();
                string[] arrXML = sXmlString.Split(char.Parse("#"));
                for (int i = 0; i < arrXML.GetLength(0); i++)
                {
                    arrOutMsg.Add(arrXML[i].ToString());
                }

                return arrOutMsg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ReadXML(string sXmlString, string strNoteName)
        {
            try
            {
                string strOutMsg = string.Empty;                
                sXmlString = @"<?xml version='1.0' encoding= 'utf-8' ?><root xmlns='http://tempuri.org/Errors.xsd>'" + sXmlString;
                XmlTextReader reader = new XmlTextReader(sXmlString);                

                while (reader.Read())
                {
                    if (reader.NodeType.Equals(XmlNodeType.Element))
                    {
                        string sNode = reader.Name;
                        string sValue = reader.Value;

                        switch (strNoteName)
                        {
                            case "Content":

                                strOutMsg = sValue;

                                break;
                            case "Acount":    
                                                   
                                break;              
                        };
                    }
                }

                return strOutMsg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ReadXMLValue(string xmlString, string strNode)
        {
            string sContent = string.Empty;

            try
            {
                string strOutMsg = string.Empty;
                XmlDocument xmlDoc  = new XmlDocument();                
                xmlDoc.LoadXml(xmlString);
                                     
                sContent = xmlDoc.SelectSingleNode(strNode).InnerText;

                return sContent;
            }
            catch
            {
                return sContent;
            }
        }

        public static ArrayList ReadXMLValues(string xmlString, string strNode, string strProName)
        {
            try
            {
                ArrayList arrData = new ArrayList();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);
                XmlNodeList pNodes = xmlDoc.SelectNodes(strNode);

                ArrayList arrTemp = new ArrayList();
                arrTemp.Add(strProName);
                arrData.Add(arrTemp);

                foreach (XmlNode node in pNodes)
                {
                    arrTemp = new ArrayList();
                    arrTemp.Add(node.InnerText.ToString());
                    arrData.Add(arrTemp);
                }                               

                return arrData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region GetNodeValArr

        public static object GetNodeValArr(string xmlString, string strPNode, string strFIDMap)
        {
            try
            {
                object objNodeVal = string.Empty;

                ArrayList arrHeader = XML2Arr(strFIDMap);
                ArrayList arrData = new ArrayList();
                arrData.Add(arrHeader);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);                                               
              
                for (int i = 0; i < 100; i++)
                {
                    ArrayList arrTemp = new ArrayList();

                    string strData = ReadXMLValue(xmlString, strPNode + (i + 1).ToString());

                    if (string.IsNullOrEmpty(strData))
                        break;

                    arrTemp = XML2Arr(strData);

                    arrData.Add(arrTemp);
                }

               

                return arrData;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        #endregion 

    }
}
