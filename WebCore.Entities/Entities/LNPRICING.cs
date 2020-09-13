using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class LNPRICING : EntityBase
    {
        public string LNPRICINGKEY { get; set; }

        public string LNPRICINGNAME { get; set; }

        public string CHANELKEY { get; set; }

        public string LNRATETYPE { get; set; }

        public string MULTIRATEKEY { get; set; }

        public string LNTERMRATEKEY { get; set; }

        public string LNTERM { get; set; }

        public string LNTERMUNIT { get; set; }

        public string LNRATE { get; set; }

        public string RATEUNIT { get; set; }

        public string MULTIRATESTEPKEY { get; set; }

        public string TERM { get; set; }

        public string TERMUNIT { get; set; }

        public string STEPAMOUNTMIN { get; set; }

        public string STEPAMOUNTMAX { get; set; }

        public string STEPRATE { get; set; }

        public string STEPRATEUNIT { get; set; }

        public string EFFECTDATE { get; set; }

        public string EXPIRYDATE { get; set; }

        public string CREATEDATE { get; set; }

        public string CREATEDBY { get; set; }

        public string MODIFYDATE { get; set; }

        public string MODIFIEDBY { get; set; }

        public string REMARK { get; set; }
    }
    public class LNPRICINGModel : LNPRICING
    {

    }
}
