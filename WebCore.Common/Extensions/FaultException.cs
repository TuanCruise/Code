using System.Linq;
using System.Reflection;
using System.ServiceModel;
using WebCore.Common;
using WebCore.Utils;

namespace WebCore.Extensions
{
    public static class FaultExceptionExtensions
    {
        /// <summary>
        /// Lấy thông tin mã lỗi
        /// </summary>
        /// <param name="ex">Lỗi trả lại từ Server</param>
        /// <returns></returns>
        public static string ToMessage(this System.Exception ex)
        {
            return ex.Message;
        }

        /// <summary>
        /// Lấy thông tin mã lỗi
        /// </summary>
        /// <param name="ex">Lỗi trả lại từ Server</param>
        /// <returns></returns>
        public static string ToMessage(this System.Exception ex, object[] @objects)
        {
            try
            {
                var wex = new FaultExceptionWrapper(ex);
                var formatObjects = new[] {wex}.Union(@objects).ToArray();

                var errorCode = int.Parse(ex.Message);
                if (AllCaches.ErrorsInfo != null && AllCaches.ErrorsInfo.ContainsKey(errorCode))
                    return string.Format(AllCaches.ErrorsInfo[errorCode], formatObjects);

                var type = typeof(ERR_SYSTEM);
                var assembly = Assembly.GetAssembly(type);
                foreach (var errType in assembly.GetTypes())
                {
                    if (errType.Namespace == type.Namespace && errType.Name.StartsWith("ERR_"))
                    {
                        foreach (var constant in errType.GetFields())
                        {
                            var value = constant.GetValue(null);
                            if (value is int && (int)value == errorCode)
                            {
                                return string.Format("ERR{0:CODE}: {1}", wex, constant.Name);
                            }
                        }
                    }
                }

                return string.Format("ERR{0:CODE}: ERR_SYSTEM_KNOWN_BUT_UNDEFINE", wex);
            }
            catch
            {
                return ex.Message;
            }
        }
    }
}
