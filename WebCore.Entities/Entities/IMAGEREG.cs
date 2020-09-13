using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using WebCore.Base;

namespace WebCore.Entities.Entities
{
    [DataContract]
    public class IMAGEREG: EntityBase
    {
        public string IMAGEREGKEY { get; set; }

        public string PRODUCTCODE { get; set; }

        public string IMAGEORDER { get; set; }

        public string IMAGENAME { get; set; }

        public string MANDATORY { get; set; }

    }
    public class IMAGEREGModel: IMAGEREG
    {

    }
}
