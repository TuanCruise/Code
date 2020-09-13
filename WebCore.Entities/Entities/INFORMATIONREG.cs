using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class INFORMATIONREG : EntityBase
    {
        public string INFORREGKEY { get; set; }

        public string INFORMANDATORY { get; set; }

        public string INFORCODE { get; set; }

        public string INFORNAME { get; set; }

    }
    public class INFORMATIONREGModel : INFORMATIONREG
    {

    }
}
