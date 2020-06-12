using System;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    public class SearchModuleInfo : ModuleInfo,ICloneable
    {
        [DataMember, Column(Name = "QUERYFORMAT")]
        public string QueryFormat { get; set; }
        [DataMember, Column(Name = "EDITSTORE")]
        public string EditStore { get; set; }
        [DataMember, Column(Name = "GROUPBOX")]
        public string GroupBox { get; set; }
        [DataMember, Column(Name = "SHOWGRPCOL")]
        public string ShowGroupColumn { get; set; }
        [DataMember, Column(Name = "FULLWIDTH")]
        public string FullWidth { get; set; }
        [DataMember, Column(Name = "SHOWCHECKBOX")]
        public string ShowCheckBox { get; set; }
        [DataMember, Column(Name = "PAGEMODE")]
        public string PageMode { get; set; }
        [DataMember, Column(Name = "WHEREEXTENSION")]
        public string WhereExtension { get; set; }
        [DataMember, Column(Name = "STATISTICQUERY")]
        public string StatisticQuery { get; set; }
        [DataMember, Column(Name = "AUTOREFRESH")]
        public string AutoRefresh { get; set; }
        [DataMember, Column(Name = "AUTOFITWIDTH")]
        public string AutoFitWidthColumns { get; set; }
        [DataMember, Column(Name = "DEFAULTBUTTON")]
        public string DefaultButton { get; set; }
        //ADD BY TRUNGTT - 20.5.2011 - SHOW DETAIL BY RICHTEXTEDIT
        [DataMember, Column(Name = "DETAILCOLUMN")]
        public string DetailColumn { get; set; }
        //26.09.2011 - ALLOW DOUBLE CLICK
        [DataMember, Column(Name = "DOUBLE_CLICK")]
        public string Double_Click { get; set; }
        //END TRUNGTT
        public bool ShowAsLookUpWindow { get; set; }
        public string ColumnValue { get; set; }
        public string ColumnText { get; set; }
        //TRUNGTT - 21.2.2014 - EXPANDALLGROUPS
        [DataMember, Column(Name = "EXPANDGROUP")]
        public string ExpandGroup { get; set; }
        //END TRUNGTT

        public SearchModuleInfo()
        {
            ShowAsLookUpWindow = false;
        }

        public object Clone()
        {
            return new SearchModuleInfo
                       {
                           AutoFitWidthColumns = AutoFitWidthColumns,
                           AutoRefresh = AutoRefresh,
                           PageMode = PageMode,
                           EditStore = EditStore,
                           FullWidth = FullWidth,
                           GroupBox = GroupBox,
                           GroupModule = GroupModule,
                           ModuleID = ModuleID,
                           ModuleName = ModuleName,
                           ModuleType = ModuleType,
                           ModuleTypeName = ModuleTypeName,
                           QueryFormat = QueryFormat,
                           RoleID = RoleID,
                           ShowCheckBox = ShowCheckBox,
                           ShowGroupColumn = ShowGroupColumn,
                           StartMode = StartMode,
                           StatisticQuery = StatisticQuery,
                           SubModule = SubModule,
                           UIType = UIType,
                           UserClose = UserClose,
                           UserHide = UserHide,
                           UserMax = UserMax,
                           WhereExtension = WhereExtension,
                           //add by TrungTT - 20.5.2011
                           DetailColumn = DetailColumn,
                           //end trungtt
                           ExpandGroup = ExpandGroup
                       };
        }
    }
}

