using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using WebCore.Entities;
using WebCore.Utils;

namespace WebCore.Base
{
    [DataContract]
    public class DataContainer
    {
        public DataTable DataTable
        {
            get
            {
                if (Container != null)
                {
                    var dt = new DataTable();

                    using (var sr = new StringReader(Container))
                    {
                        dt.ReadXmlSchema(sr);
                    }

                    using (var sr = new StringReader(Container))
                    {
                        dt.ReadXml(sr);
                        return dt;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    var sw = new StringWriter();
                    value.TableName = "Result";
                    value.WriteXml(sw, XmlWriteMode.WriteSchema);
                    Container = sw.ToString();
                }
            }
        }

        public DataSet DataSet
        {
            get
            {
                if (Container != null)
                {
                    var ds = new DataSet();

                    using (var sr = new StringReader(Container))
                    {
                        ds.ReadXmlSchema(sr);
                    }

                    using (var sr = new StringReader(Container))
                    {
                        ds.ReadXml(sr);
                        return ds;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    var sw = new StringWriter();
                    value.WriteXml(sw, XmlWriteMode.WriteSchema);
                    Container = sw.ToString();
                }
            }
        }

        [DataMember]
        public string Container { get; set; }

        public void BuildSchema(DataTable resultTable, List<ModuleFieldInfo> fields)
        {
            if (Container != null)
            {
                using (var sr = new StringReader(Container))
                {
                    resultTable.ReadXmlSchema(sr);
                    foreach (var field in fields)
                    {
                        if (resultTable.Columns.Contains(field.FieldName))
                        {
                            resultTable.Columns[field.FieldName].DataType = FieldUtils.GetType(field.FieldType);
                        }
                    }
                }
            }
        }

        public void FillTable(DataTable resultTable, List<ModuleFieldInfo> fields)
        {
            if (Container != null)
            {
                using (var sr = new StringReader(Container))
                {
                    resultTable.ReadXmlSchema(sr);
                    foreach (var field in fields)
                    {
                        if (resultTable.Columns.Contains(field.FieldName))
                        {
                            resultTable.Columns[field.FieldName].DataType = FieldUtils.GetType(field.FieldType);
                        }
                    }
                }

                using (var sr = new StringReader(Container))
                {
                    resultTable.ReadXml(sr);
                    foreach(DataRow row in resultTable.Rows)
                    {
                        row.AcceptChanges();
                    }
                }
            }
        }

        public DataTable GetTable(List<ModuleFieldInfo> fields)
        {
            if (Container != null)
            {
                var dt = new DataTable();
                using (var sr = new StringReader(Container))
                {
                    dt.ReadXmlSchema(sr);
                    foreach (var field in fields)
                    {
                        if (dt.Columns.Contains(field.FieldName))
                        {
                            dt.Columns[field.FieldName].DataType = FieldUtils.GetType(field.FieldType);
                        }
                    }
                }

                using (var sr = new StringReader(Container))
                {
                    dt.ReadXml(sr);
                    foreach (DataRow row in dt.Rows)
                    {
                        row.AcceptChanges();
                    }
                    return dt;
                }
            }

            return null;
        }
    }
}
