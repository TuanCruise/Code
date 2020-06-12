using System.Collections.Generic;
using System.Linq;
using WebCore.Common;
using WebCore.Entities;

namespace Core.Utils
{
    public static class CodeUtils
    {
        public static List<CodeInfo> GetCodes(string cdType, string cdName)
        {
            return (from code in AllCaches.CodesInfo
             where code.CodeType == cdType && code.CodeName == cdName
             select code).ToList();
        }

        public static string GetCodeName(string cdType, string cdName, string cdValue)
        {
            return (from code in AllCaches.CodesInfo
                    where code.CodeType == cdType && code.CodeName == cdName && code.CodeValue == cdValue
                    select code).First().CodeValueName;
        }
    }
}
