using System;
using System.Collections.Generic;
using System.Linq;
using WebCore.Base;
using System.Runtime.Serialization;

namespace WebCore.Entities
{
    [DataContract]
    public class OracleParam : EntityBase, ICloneable
    {
        private string m_Name;
        private string m_StoreName;
        [DataMember]
        public string StoreName
        {
            get
            {
                return m_StoreName;
            }
            set
            {
                m_StoreName = value.ToUpper();
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value.ToUpper();
            }
        }
        public object Value { get; set; }

        public object Clone()
        {
            return new OracleParam
                       {
                           Name = Name,
                           StoreName = StoreName,
                           Value = null
                       };
        }
    }

    public static class OracleParamCollecion
    {
        public static List<string> ToListString(this List<OracleParam> @params)
        {
            return (from param in @params
                    select param.Value == null ? null : param.Value.ToString()).ToList();
        }
    }
}
