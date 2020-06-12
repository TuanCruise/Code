using System;
using System.IO;

namespace WebModelCore.Common {
    public class ErrorLog {
        public static void WriteLog(string sFunction, string slỗi)
        {
            string AppPath = Path.Combine(Directory.GetCurrentDirectory());
            StreamWriter sLogFile = null;
            try {
                //Ghi lại hành động của người sử dụng vào log file
                string sDay = string.Format("{0:dd}", DateTime.Now);
                string sMonth = string.Format("{0:MM}", DateTime.Now);
                string strLogFileName = sDay + "-" + sMonth + "-" + DateTime.Now.Year + ".log";
                string strFolderName = AppPath + @"\Logs\" + DateTime.Now.Year + "-" + sMonth;
                //Application.StartupPath
                //Tạo thư mục nếu chưa có
                if (!Directory.Exists(strFolderName + @"\")) {
                    Directory.CreateDirectory(strFolderName + @"\");
                }
                strLogFileName = strFolderName + @"\" + strLogFileName;

                if (File.Exists(strLogFileName)) {
                    //Nếu đã tồn tại file thì tiếp tục ghi thêm
                    sLogFile = File.AppendText(strLogFileName);
                    sLogFile.WriteLine(string.Format("Thời điểm lỗi: {0:hh:mm:ss tt}", DateTime.Now));
                    if (sFunction != string.Empty)
                        sLogFile.WriteLine(string.Format("Hàm/Phương thức gọi lỗi: {0}", sFunction));
                    sLogFile.WriteLine(string.Format("Chi tiết lỗi: {0}", slỗi));
                    sLogFile.WriteLine("-------------------------------------------");
                    sLogFile.Flush();
                }
                else {
                    //Nếu file chưa tồn tại thì có thể tạo mới và ghi log
                    sLogFile = new StreamWriter(strLogFileName);
                    sLogFile.WriteLine(string.Format("Thời điểm lỗi: {0:hh:mm:ss tt}", DateTime.Now));
                    if (sFunction != string.Empty)
                        sLogFile.WriteLine(string.Format("Hàm/Phương thức gọi lỗi: {0}", sFunction));
                    sLogFile.WriteLine(string.Format("Chi tiết lỗi: {0}", slỗi));
                    sLogFile.WriteLine("-------------------------------------------");
                }
                sLogFile.Close();
            }
            catch (Exception) {
                if (sLogFile != null) {
                    sLogFile.Close();
                }
            }
        }

        public static void WriteLog( string sAction)
        {
            WriteLog(string.Empty, sAction);
        }
    }
}
