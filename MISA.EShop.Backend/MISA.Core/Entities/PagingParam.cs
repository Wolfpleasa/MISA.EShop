﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.Core.Enumeration.MISAEnum;

namespace MISA.Core.Entities
{
    public class PagingParam
    {
        public DataType DataType { get; set; }

        public int OperatorIndex { get; set; }

        public string Field { get; set; }
        
        public string Value { get; set; }


        // string
        // 0
        // abc

        // integer
        //0
        //"0"
    }
}
