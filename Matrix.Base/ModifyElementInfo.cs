namespace Matrix.Base
{
    using System;

    public class ModifyElementInfo : EventArgs
    {
        public string Message { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
