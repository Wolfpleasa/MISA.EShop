using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Enumeration
{
    public class MISAEnum
    {   
        /// <summary>
        /// Các toán tử liên quan đến chuỗi
        /// </summary>
        public enum MISAStringOperator
        {
            Contain = 0,
            Equal = 1,
            StartWith = 2,
            EndWith = 3,
            NotContain = 4,
        }

        /// <summary>
        /// Toán tử liên quan đến tiền
        /// </summary>
        public enum MISASNumberOperator
        {
            Equal = 0,
            Lower = 1,
            LowerOrEqual = 2,
            Greater = 3,
            GreaterOrEqual = 4,
        }

        /// <summary>
        /// Trạng thái trả về 
        /// </summary>
        public enum HTTPStatus
        {
            Ok = 200,
            Created = 201,
            ServerError = 500,
        }

        /// <summary>
        /// Trạng thái cờ
        /// </summary>
        public enum FlagMode
        {
            Add = 0,
            Edit = 1,
            Delete = 3,
        }
    }
}
