using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAudioWinFormTest
{
    public class ChangeEventArgs : EventArgs
    {

        public object OldValue { get; set; }
        public object NewValue { get; set; }

        public ChangeEventArgs() { }
        public ChangeEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
