using System;
using System.Collections;
using System.Xml.Serialization;

namespace WB.MESSAGE
{
    public class Message
    {
		public int MsgID;
		public string MsgIP;
		public int MsgType;
		public int MsgAction;
		public string MsgRef;
		public string MsgBatch;
		public string BranchID;
		public string CPUID;
		public string UserID;
		public string CustType;
		public string MsgTime;
		public DateTime MsgDate;
		public DateTime CreatedDate;
		public DateTime VerifiedDate;
		public DateTime ApprovedDate;
		public DateTime SentDate;
		public string CreatedUser;
		public string VerifiedUser;
		public string ApprovedUser;
		public string SentUser;
		public string ObjectName;		
		public string FunctionID;
		public string MsgStatus;
		public string MsgErrCode;
		public string MsgErrSource;
		public string MsgErrDesc;
		public string MsgErrDesc_vn;
		public string MsgErrType;
		public int DataProvider;
		public int effectRows;
		public bool CheckResponse = true;
		public string MsgSessionID;
		public string MsgSessionType;
		public string Language;		
		public string ModId;    //module id   
        public string modSearchId;    

        /// <Body>
        [XmlArrayItemAttribute("Detail")]
		public ArrayList Body;
		
		public Message()
		{
			Body = new ArrayList();
		}

		public Message(string ObjName, int MesTp)
		{
			//Init message header
			ObjectName = ObjName;
			MsgType = MesTp;
			//Init Message body
			Body = new ArrayList();
		}

		public object getValue(string name)
		{
			for (int i = 0; i < Body.Count - 1; i += 2)
				if (Body[i].ToString().ToUpper().Trim() == name.ToUpper()) return Body[i + 1];
			return null;
		}

		public static object getValue(ArrayList arr, string name)
		{
			for (int i = 0; i < arr.Count - 1; i += 2)
				if (arr[i].ToString() == name) return arr[i + 1];
			return null;
		}


	}
}
