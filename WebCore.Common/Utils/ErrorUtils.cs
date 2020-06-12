using System;
using WebCore.Common;

namespace WebCore.Utils
{
    public static class ErrorUtils
    {
        /// <summary>
        /// Tạo lỗi không xác định
        /// </summary>
        /// <returns></returns>
        public static Exception CreateError(Exception ex)
        {
            return CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.ToString());
        }

        /// <summary>
        /// Tạo lỗi với mã lỗi xác định
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <returns></returns>
        public static Exception CreateError(int errorCode)
        {
            var ex = new Exception("");
            return ex;
        }

        /// <summary>
        /// Tạo lỗi với log dạng Method(Param1, Param2, Param3)
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="methodName">Tên method</param>
        /// <param name="methodParamValues">Các tham số truyền vào method</param>
        /// <returns></returns>
        public static Exception CreateError(int errorCode, string methodName, params object[] methodParamValues)
        {
#if DEBUG
            var message = methodName + "(";

            var sep = "";
            foreach (object value in methodParamValues)
            {
                message += sep + value;
                sep = ",";
            }
            message += ");";
#else
            var message = "";
#endif

            var ex = new Exception("");
            return ex;
        }

        /// <summary>
        /// Tạo lỗi xác định với chi tiết lỗi
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="subMessage">Chi tiết lỗi</param>
        /// <returns></returns>
        public static Exception CreateErrorWithSubMessage(int errorCode, string subMessage)
        {
            var ex = new Exception("");
            return ex;
        }

        /// <summary>
        /// Tạo lỗi xác định với chi tiết lỗi và Method(Param1, Param2, Param3)
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="subMessage">Chi tiết lỗi</param>
        /// <param name="methodName">Tên Method</param>
        /// <param name="methodParamValues">Các tham số truyền vào method</param>
        /// <returns></returns>
        public static Exception CreateErrorWithSubMessage(int errorCode, string subMessage, string methodName, params object[] methodParamValues)
        {
#if DEBUG
            var message = methodName + "(";

            var sep = "";
            foreach (object value in methodParamValues)
            {
                message += sep + value;
                sep = ",";
            }
            message += ");\r\n-->";
            message += subMessage;
#else
            var message = subMessage;
#endif

            var ex = new Exception("");
            return ex;
        }
    }

    public static class ExceptionExtends
    {
        public static int GetErrorCode(this Exception ex)
        {
            return ex.GetErrorCode() ;
        }

        public static string GetErrorMessage(this Exception ex)
        {
            return ex.Message;
        }
    }
}
