using System;
using System.Collections.Generic;
using System.Text;
using Host.BusinessBase;
using Host.DataBaseAccessService;
using WB.SYSTEM;
using System.Data;
using System.Collections;


namespace Host.BusinessObject
{

    public class SESSIONLOG : BusinessEntity
    {
        public SESSIONLOG()
        {
            this.entityName = "SESSIONLOG";         

        }

        public ArrayList GetTop1Login(string streBnkNO)
        {
            try
            {
                string sqlStr = string.Empty;
                
                //sqlStr = "SET DATEFORMAT DMY SELECT TOP 1 * FROM SESSIONLOG WITH (NOLOCK) WHERE UserID='" + streBnkNO + "'  AND LOGIN_DATE='" + DateTime.Now.ToString(Constants.DD_MM_YYYY) + "' ORDER BY [LOGIN_TIME] DESC"; //AND INC_PASS='Y'
                sqlStr = "SET DATEFORMAT DMY SELECT TOP 1 * FROM SESSIONLOG WITH (NOLOCK) WHERE UserID='" + streBnkNO + "'  ORDER BY LOGIN_DATE DESC,[LOGIN_TIME] DESC"; //AND INC_PASS='Y'

                ArrayList arrData = new ArrayList();
                ArrayList arrHeader = new ArrayList();

                IDataReader rd;
                rd = dbManager.ExecuteReader(sqlStr, CommandType.Text);

                int j = 0;
                while (j < rd.FieldCount)
                {
                    arrHeader.Add(rd.GetName(j));
                    j++;
                }
                arrData.Add(arrHeader);

                while (rd.Read())
                {
                    ArrayList arrDetail = new ArrayList();
                    for (int i = 0; i < rd.FieldCount; i++)
                    {
                        object obj = (rd.IsDBNull(i)) ? "" : rd.GetValue(i);
                        arrDetail.Add(obj);
                    }
                    arrData.Add(arrDetail);
                }

                rd.Close();
                rd.Dispose();

                return arrData;

            }
            catch (ErrorMessage er)
            {
                ErrorHandler.ThrowError(er);
                return null;
            }
            catch (Exception ex)
            {                
                ErrorMessage objErr = new ErrorMessage();

                objErr.ErrorCode = ErrorHandler.SRV_MAINTENANCE_PROCESS_ERR;
                objErr.ErrorDesc = ex.Message;
                objErr.ErrorSource = ex.Source;
                ErrorHandler.ThrowError(objErr);
                return null;
            }
        }
      
    }
}
