using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLINFORMATION : EntityBase
    {
        public string CLINFORKEY { get; set; }

        public string ACCOUNTNO { get; set; }

        public string INFORREGKEY { get; set; }

        public string INFORVALUE { get; set; }

        public string INFORSELECTED { get; set; }
    }
    public class CLINFORMATIONModel : CLINFORMATION
    {

    }
}
