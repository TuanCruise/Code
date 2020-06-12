using System;
using System.Data;
using System.ServiceModel;

namespace WebCore.Extensions
{
    public class RowFormattable : IFormattable
    {
        public DataRow Row { get; private set; }

        public RowFormattable(DataRow row)
        {
            Row = row;
        }

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var astr = format.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            var fieldName = astr[0];
            if (Row.Table.Columns.Contains(fieldName))
            {
                var value = Row[fieldName];
                if (astr.Length == 2)
                {
                    return string.Format("{0:" + astr[1] + "}", value);
                }
                return string.Format("{0}", value);
            }
            return "{" + format + "}";
        }

        #endregion
    }

    public class FaultExceptionWrapper : IFormattable
    {
        private System.Exception m_Exception;
        public FaultExceptionWrapper(System.Exception exception)
        {
            m_Exception = exception;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == "CODE")
                return m_Exception.Message;

            if (format == "REASON")
                return m_Exception.Message;

            return m_Exception.Message;
        }
    }

    public class ObjectWrapper : IFormattable
    {
        private object m_Object;
        private Type m_ObjectType;
        public ObjectWrapper(object @object)
        {
            m_Object = @object;
            m_ObjectType = @object.GetType();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var astr = format.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            var propertyName = astr[0];
            var prop = m_ObjectType.GetProperty(propertyName);

            if (prop != null)
            {
                var value = prop.GetValue(m_Object, new object[] {});

                if (astr.Length == 2)
                {
                    return string.Format("{0:" + astr[1] + "}", value);
                }

                return string.Format("{0}", value);
            }
            return "{" + format + "}";
        }
    }
}
