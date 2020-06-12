using System;
using System.Data;
using System.Collections;

namespace WebCore.Base
{
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public static class DataTableExtensions
    {
        public static void CopyToObject(this DataRow row, object obj)
        {
            var type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                var attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attrs.Length > 0 && prop.CanWrite)
                {
                    var attr = (ColumnAttribute)attrs[0];
                    if (row[attr.Name] != DBNull.Value)
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[attr.Name], prop.PropertyType), null);
                    }
                }
            }
        }

        public static void CopyToCollection<T>(this DataRowCollection rows, IList lstObject)
            where T : new()
        {
            foreach(DataRow row in rows)
            {
                var obj = new T();
                row.CopyToObject(obj);
                lstObject.Add(obj);
            }
        }
    }
}