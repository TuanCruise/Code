using System.Collections.Generic;
using System.Linq;
using WebCore.Common;
using WebCore.Entities;

namespace Core.Utils
{
    public static class SysvarUtils
    {
        public static List<SysvarInfo> GetValues(string grName)
        {
            return (from value in AllCaches.SysvarsInfo
                    where value.GrName == grName 
                    select value).ToList();
        }

        public static string GetVarValue(string grName, string varName)
        {
            return (from value in AllCaches.SysvarsInfo
                    where value.GrName == grName && value.VarName == varName
                    select value).First().VarValue;
        }
    }
}
