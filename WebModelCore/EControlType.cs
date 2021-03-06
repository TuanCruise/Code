﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModelCore {
    public enum EControlType {
        [Description("Combobox")]
        CB,
        [Description("Textbox")]
        TB,
        [Description("TextArea")]
        TA,
        [Description("DatePicker")]
        DT,
        [Description("Upload")]
        UL,
        [Description("Slide")]
        SL,
        [Description("Barcode")]
        BC,
        [Description("Schedule")]
        SCL,
        [Description("TAB")]
        TAB,
        [Description("GRID EDIT")]
        GE,
        //dongpv:upload image
        [Description("Upload file")]
        UF,
        //dongpv:lookup data
        [Description("Looup data")]
        LV,
        
    }
}
