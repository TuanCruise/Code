using System;
using System.Collections.Generic;
using System.Text;

namespace WebModelCore.ModelCheckBox
{
    public class CheckBoxModel
    {
        private bool _Value { get; set; }
        public bool Value {
            get { return _Value; }

            set
            {
                _Value = value;
            }
        }
        public dynamic KeyValue { get; set; }
    }
}
