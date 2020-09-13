using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLACCOUNT : EntityBase
    {
        public string ACCOUNTNO { get; set; }

        public string CUSTID { get; set; }

        public string CUSTNAME { get; set; }

        public string PRODUCTCODE { get; set; }

        public string CLPRODDTLKEY { get; set; }

        public string CLPRODDTLNAME { get; set; }

        public string BRANNAME { get; set; }

        public string MODEL { get; set; }

        public string PRODUCTYEAR { get; set; }

        public string CURRENCYCODE { get; set; }

        public string CLCERTIFICATE { get; set; }

        public string OWNER { get; set; }

        public string ORIGINACCOUNTNO { get; set; }

        public string ACCOUNTDESC { get; set; }

        public string ORIGINVALUE { get; set; }

        public string APPRAISALVALUE { get; set; }

        public string CLSTATE { get; set; }

        public string CLHOLDER { get; set; }

        public string CLCHIPNO { get; set; }

        public string ACCOUNTSTATUS { get; set; }

        public string MODIFYDATE { get; set; }

        public string MODIFIEDBY { get; set; }

        public string SUBBRANCHID { get; set; }

        public string CLIMAGEINFORKEY { get; set; }

        public string IMAGEINFORREGKEY { get; set; }

        public string IMAGENAME { get; set; }

        public string IMAGEVALUE { get; set; }

        public string CLINFORKEY { get; set; }

        public string INFORKEY { get; set; }

        public string INFORNAME { get; set; }

        public string INFORVALUE { get; set; }

        public string CLIMAGEREGKEY { get; set; }

        public string IMAGEORDER { get; set; }
    }
    public class CLACCOUNTModel : CLACCOUNT
    {

    }
}
