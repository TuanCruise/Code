using System;
using WebCore.Entities;
using WebModelCore;

namespace WebAppCoreBlazorServer.Common
{
    public static class CommonValidate
    {
        public static string ValidateFieldInfo(this ModuleFieldInfo fieldInfo)
        {
            string error = "";
            if (fieldInfo.HideWeb == "Y")
            {//Ko hien thi thi ko can validate
                return "";
            }
            if (fieldInfo.Nullable == EYesNo.N.ToString())
            {
                if (string.IsNullOrEmpty(fieldInfo.FieldID))
                {
                    error += "Là trường bắt buộc.";
                }
            }

            if (fieldInfo.MaxLength > 0)
            {
                if (fieldInfo.FieldID.Length > fieldInfo.MaxLength)
                {
                    error += string.Format("Chỉ được nhỏ hơn {0} kí tự.", fieldInfo.MaxLength);
                }
            }
            if (fieldInfo.FieldType == "INT")
            {
                try
                {
                    if (!string.IsNullOrEmpty(fieldInfo.FieldID))
                    {
                        var checknum = Convert.ToInt32(fieldInfo.FieldID.Replace(".", ","));
                    }
                }
                catch (Exception ex)
                {
                    error += "Là trường số.";
                }
            }
            return error;
        }
    }
}
