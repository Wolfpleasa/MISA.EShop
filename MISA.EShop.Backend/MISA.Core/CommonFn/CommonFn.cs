using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.CommonFn
{
    public class CommonFn
    {
        /// <summary>
        /// Chuyển tiếng việt có dấu sang không dấu
        /// </summary>
        private static readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        /// <summary>
        /// Hàm chuyển tiếng việt có dấu sang không dấu
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string utf8Convert1(string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }

        /// <summary>
        /// Hàm trả về lỗi server
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static object ObjError(string message)
        {
            var newObj = new
            {
                devMsg = message,
                userMsg = Core.Properties.ResourceVN.Error_Message_UserVN,
                errorCode = "misa-001",
                moreInfo = @"https:/openapi.misa.com.vn/errorcode/misa-001",
                traceId = ""
            };

            return newObj;
        }
    }
}

