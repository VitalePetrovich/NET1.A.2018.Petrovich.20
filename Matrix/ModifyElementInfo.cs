﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class ModifyElementInfo : EventArgs
    {
        public string Message { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
