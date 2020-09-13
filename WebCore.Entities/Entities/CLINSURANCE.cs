using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLINSURANCE : EntityBase
    {
        public string INSCODE { get; set; }

        public string INSNAME { get; set; }

        public string INSTYPE { get; set; }

        public string INSCOMPANY { get; set; }

        public string INSTERM { get; set; }

        public string INSLOANMIN { get; set; }

        public string INSLOANMAX { get; set; }

        public string INSMETHOD { get; set; }

        public string INSPERCENT { get; set; }

        public string INSVALUE { get; set; }

        public string INSCHARGEMIN { get; set; }

        public string INSCHARGEMAX { get; set; }

        public string INSVATMETHOD { get; set; }

        public string INSVATPERCENT { get; set; }
    }
    public class ICLINSURANCEModel : CLINSURANCE
    {

    }
}
