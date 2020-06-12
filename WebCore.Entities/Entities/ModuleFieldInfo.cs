using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebCore.Base;

namespace WebCore.Entities
{
    [DataContract]
    [Serializable]
    public class ModuleFieldInfo : EntityBase
    {
        private string m_ParameterName;
        [DataMember, Column(Name = "FLDID")]
        public string FieldID { get; set; }
        [DataMember, Column(Name = "MODID")]
        public string ModuleID { get; set; }
        [DataMember, Column(Name = "MODTYPE")]
        public string ModuleType { get; set; }
        [DataMember, Column(Name = "FLDNAME")]
        public string FieldName { get; set; }
        [DataMember, Column(Name = "PARAMNAME")]
        public string ParameterName {
            get
            {
                return m_ParameterName;
            }
            set
            {
                m_ParameterName = value != null ? value.ToUpper() : null;
            }
        }
        [DataMember, Column(Name = "FLDGROUP")]
        public string FieldGroup { get; set; }
        [DataMember, Column(Name = "FLDTYPE")]
        public string FieldType { get; set; }
        [DataMember, Column(Name = "IMPTYPE")]
        public string ImportType { get; set; }
        [DataMember, Column(Name = "SCDTYPE")]
        public string ConditionType { get; set; }
        [DataMember, Column(Name = "TEXTCASE")]
        public string TextCase { get; set; }
        [DataMember, Column(Name = "CTRLTYPE")]
        public string ControlType { get; set; }
        [DataMember, Column(Name = "FLDVALIDATE")]
        public string FieldValidate { get; set; }
        [DataMember, Column(Name = "FLDFORMAT")]
        public string FieldFormat { get; set; }
        [DataMember, Column(Name = "DEFAULTVALUE")]
        public string DefaultValue { get; set; }
        [DataMember, Column(Name = "CUSTOMSCD")]
        public string CustomSearchCondition { get; set; }
        [DataMember, Column(Name = "LISTSOURCE")]
        public string ListSource { get; set; }
        [DataMember, Column(Name = "CALLBACK")]
        public string Callback { get; set; }
        [DataMember, Column(Name = "POPUPMODE")]
        public string PopupMode { get; set; }
        [DataMember, Column(Name = "TABSTOP")]
        public string TabStop { get; set; }
        [DataMember, Column(Name = "ROE")]
        public string ReadOnlyOnEdit { get; set; }
        [DataMember, Column(Name = "ROA")]
        public string ReadOnlyOnAdd { get; set; }
        [DataMember, Column(Name = "ROV")]
        public string ReadOnlyOnView { get; set; }
        [DataMember, Column(Name = "FOS")]
        public string FixedOnSearch { get; set; }
        [DataMember, Column(Name = "GOS")]
        public string GroupOnSearch { get; set; }
        [DataMember, Column(Name = "ICONONLY")]
        public string IconOnly { get; set; }
        [DataMember, Column(Name = "TEXTALIGN")]
        public string TextAlign { get; set; }
        [DataMember, Column(Name = "FIXEDWIDTH")]
        public int FixedWidth { get; set; }
        [DataMember, Column(Name = "VR")]
        public string ValidateRule { get; set; }
        [DataMember, Column(Name = "VROE")]
        public string ValidateRuleOnEdit { get; set; }
        [DataMember, Column(Name = "VROA")]
        public string ValidateRuleOnAdd { get; set; }
        [DataMember, Column(Name = "NULLABLE")]
        public string Nullable { get; set; }
        [DataMember, Column(Name = "WHEREEXTENSION")]
        public string WhereExtension { get; set; }
        [DataMember, Column(Name = "HOLDCOLUMN")]
        public string HoldColumn { get; set; }
        [DataMember, Column(Name = "MERGECELL")]
        public string MegerCell { get; set; }
        [DataMember, Column(Name = "COLUMNSORT")]
        public string ColumnSort { get; set; }
        [DataMember, Column(Name = "GROUP_SUMMARY")]
        public string Group_Summary { get; set; }
        [DataMember, Column(Name = "UNBOUND_EXPRESSION")]
        public string UnboundExpression { get; set; }
        [DataMember, Column(Name = "FSCDVALUE")]
        public string FunctionSCDValue { get; set; }
        [DataMember, Column(Name = "MAXLENGTH")]
        public int MaxLength { get; set; }
        [DataMember, Column(Name = "WRAPTEXT")]
        public string WrapText { get; set; }
        [DataMember, Column(Name = "TXCHECK")]
        public string TxCheck { get; set; }
        [DataMember, Column(Name = "APPTYPE")]
        public string AppType { get; set; }
        [DataMember, Column(Name = "BRTYPE")]
        public string BrType { get; set; }
        [DataMember, Column(Name = "BANDEDGRID")]
        public string BandedGrid { get; set; }
        [DataMember, Column(Name = "BUSSTABNAME")]
        public string BussTabName { get; set; }
        [DataMember, Column(Name = "ISJSON")]
        public string IsJson { get; set; }
        [DataMember, Column(Name = "HIDE_WEB")]
        public string HideWeb { get; set; }
        [DataMember, Column(Name = "ORDER")]
        public int Order { get; set; }
        [DataMember, Column(Name = "MOD_ENTER")]
        public string Mod_Enter { get; set; }
        [DataMember, Column(Name = "DISABLE_EDIT")]
        public string DisableEdit { get; set; }
        [DataMember, Column(Name = "AUTO_CODE")]
        public string AutoCode { get; set; }
        [DataMember, Column(Name = "FIELDSGROUP")]
        public string FieldsGroup { get; set; }
        [DataMember, Column(Name = "PARENTID")]
        public string ParentId { get; set; }
        [DataMember, Column(Name = "CALLMODID")]
        public string CallModId { get; set; }
        /// <summary>
        /// Cột này chỉ để bind dữ liệu khi control nhập. ko có trong DB
        /// </summary>
        [DataMember, Column(Name = "Value")]
        public string Value {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        /// <summary>
        /// Cột này chỉ để bind dữ liệu khi User check vào checkbox. ko có trong DB
        /// </summary>
        [DataMember, Column(Name = "CheckBox")]
        public bool IsCheck { get; set; }
        private string _Value { get; set; }
        [DataMember]
        public List<ModuleFieldInfo> FieldChilds { get; set; }

        public ModuleFieldInfo()
        {
            FieldChilds = new List<ModuleFieldInfo>();
        }
    }
}
