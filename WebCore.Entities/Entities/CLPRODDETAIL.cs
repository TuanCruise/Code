using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLPRODDETAIL : EntityBase
    {
        public string CLPRODDTLKEY { get; set; }

        public string CLPRODDTLNAME { get; set; }

        public string EFFECTDATE { get; set; }

        public string CATEGORY { get; set; }

        public string BRANNAME { get; set; }

        public string MODEL { get; set; }

        public string PRODUCTYEAR { get; set; }

        public string PRODUCTDDESC { get; set; }

        public string CREATEDATE { get; set; }

        public string CREATEDBY { get; set; }

        public string MODIFYDATE { get; set; }

        public string MODIFIEDBY { get; set; }

        public string USESTATUS { get; set; }

        public string REMARK { get; set; }
    }
    public class CLPRODDETAILModel : CLPRODDETAIL
    {

    }
}
