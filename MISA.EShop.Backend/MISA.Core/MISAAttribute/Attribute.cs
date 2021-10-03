﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.MISAAttribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISANotMap : Attribute
    {
        public string Name = string.Empty;
        public MISANotMap(string name = "")
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute 
    {
        public string Name = string.Empty;
        
        public MISARequired(string name = "")
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAIsNumber : Attribute
    {
        public string Name = string.Empty;

        public MISAIsNumber(string name = "")
        {
            Name = name;
        }
    }
}
