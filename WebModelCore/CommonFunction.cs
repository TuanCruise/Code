using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCore.Entities;

namespace WebModelCore
{
    public class CommonFunction
    {
        public static List<ModuleFieldInfo> GetModuleFields(List<ModuleFieldInfo> moduleFieldInfos, string moduleID, string fieldGroup)
        {
            return (from field in moduleFieldInfos
                    where field.ModuleID == moduleID && (field.FieldGroup == fieldGroup || fieldGroup == null)
                    select field).ToList();
        }
        /// <summary>
        /// Lấy tên store gọi lấy dữ liệu dùng cho view và edit
        /// </summary>
        /// <param name="maintainModuleInfo"></param>
        /// <param name="subMod"></param>
        /// <returns></returns>
        public static string GetStoreRunModMaintain(MaintainModuleInfo maintainModuleInfo, string subMod)
        {
            var store = "";
            if (string.IsNullOrEmpty(subMod))
            {
                store = maintainModuleInfo.AddSelectStore;
            }
            else if (subMod.ToUpper() == ESubMod.MED.ToString())
            {
                store = maintainModuleInfo.EditSelectStore;
            }
            else if (subMod.ToUpper() == ESubMod.MVW.ToString())
            {
                store = maintainModuleInfo.ViewSelectStore;
            }
            return store;
        }
        public static string RandomString(int size)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }

}
