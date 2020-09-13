using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class CLIMAGE : EntityBase
    {
        public string CLIMAGEKEY { get; set; }

        public string ACCOUNTNO { get; set; }

        public string IMAGEREGKEY { get; set; }

        public string IMAGEVALUE { get; set; }

    }
    public class CLIMAGEModel : CLIMAGE
    {

    }
}
