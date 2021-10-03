using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Enumeration
{
    public class MISAEnum
    {
        public enum MISAStringOperator
        {
            Contain = 0,
            Equal = 1,
            StartWith = 2,
            EndWith = 3,
            NotContain = 4,
        }

        public enum MISASNumberOperator
        {
            Equal = 0,
            Lower = 1,
            LowerOrEqual = 2,
            Greater = 3,
            GreaterOrEqual = 4,
        }
        public enum DataType
        {
            String = 0,
            Integer = 1,
            Double = 2,
        }
    }
}
