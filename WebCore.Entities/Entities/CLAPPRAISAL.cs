using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLAPPRAISAL : EntityBase
    {
        public string CLAPPRAISALKEY { get; set; }

        public string ACCOUNTNO { get; set; }

        public string MODIFYTIME { get; set; }

        public string MODIFIEDBY { get; set; }

        public string APPRAISALVALUE { get; set; }
    }
    public class CLAPPRAISALModel : CLAPPRAISAL
    {

    }
}
