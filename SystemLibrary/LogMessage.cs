using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using WB.SYSTEM;
//using CommonShared;

namespace WB.SystemLibrary
{
    public class LogMessage
    {
        public const int GENERAL_ERROR = -1;
        public const int LOG_MSG_HEAD = 1;
        public const int PCODE_CASH_WITHDRAW = 10;

        public static void Log(string strMessage)
        {
            try
            {
                StreamWriter myStreamWriter = null;

                String strPath = "C:\\logs\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANK_MSG_FIELDS" + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }
                    myStreamWriter.WriteLine(strMessage);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public static void LogParm(string strMessage, object[] obj)
        {
            try
            {
                StreamWriter myStreamWriter = null;

                String strPath = "C:\\logs\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_SMB_PARM" + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }

                    int intCount = obj.GetLength(0);
                    string strValue = string.Empty;
                    for (int i = 1; i < intCount + 1; i++)
                    {
                        strValue += i.ToString() +  ":"+ SysUtils.CString(obj[i - 1]) + "  ";
                    }

                    myStreamWriter.WriteLine(strMessage + strValue);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public static void Trace(string strMessage)
        {
            try
            {
                StreamWriter myStreamWriter = null;

                String strPath = "C:\\logs\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_TRACE" + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }
                    myStreamWriter.WriteLine(strMessage);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public static void Trace(string strMessage,string strFileName)
        {
            try
            {
                StreamWriter myStreamWriter = null;

                String strPath = "C:\\logs\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_" + strFileName + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }
                    myStreamWriter.WriteLine(strMessage);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public static void LogMessages(string strMessage)
        {
            try
            {
                StreamWriter myStreamWriter = null;

                String strPath = "C:\\logs\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_VNP_MSG_" + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }
                    myStreamWriter.WriteLine(strMessage);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        //LOG ENTIRE REQUEST FROM CLIENT:
        public static void LogInfoClient(string strMessage)
        {
            try
            {
                //string strMessage = string.Empty;

                strMessage = System.DateTime.Now + "\r\n" + strMessage;              
                strMessage += "--------------------------------------------------------------------------------------------------------------" + "\r\n";

                StreamWriter myStreamWriter = null;

                String strPath = "C:\\LOGS\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_CLIENT_REQUEST" + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }
                    myStreamWriter.WriteLine(strMessage);
                }
                catch
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public static void LogFieldsMsg(string strMessage)
        {
            try
            {
                StreamWriter myStreamWriter = null;

                String strPath = "C:\\logs\\";
                String strFilename = "";

                try
                {
                    strFilename = String.Format("{0:ddMMyyyy}", DateTime.Now) + "_LOG_EBANK_MSG_FIELDS" + ".LOG";

                    if (Directory.Exists(strPath))
                    {

                    }
                    else
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!File.Exists(strPath + strFilename))
                    {
                        myStreamWriter = File.CreateText(strPath + strFilename);
                    }
                    else
                    {
                        myStreamWriter = File.AppendText(strPath + strFilename);
                    }
                    myStreamWriter.WriteLine(strMessage);
                    myStreamWriter.WriteLine("--------------------------------------------------------------------------------------------------------------------");
                }
                catch
                {
                }
                finally
                {
                    try
                    {
                        myStreamWriter.Flush();
                        myStreamWriter.Close();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        public static void LogTrans(ArrayList arrInfor)
        {
            try
            {
                              
            }
            catch
            {

            }
        }

    }
}
