using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using WebCore.Entities;
using WebModelCore;
using WebModelCore.CodeInfo;
using WebModelCore.Common;

namespace WebAppCoreBlazorServer.Common
{
    public static class CommonView
    {
        public static string GetLanguage(this ModuleFieldInfo field, List<LanguageInfo> languages, string modName)
        {
            if (field != null && !string.IsNullOrEmpty(field.FieldName))
            {
                field.FieldName = field.FieldName.Trim();
            }
            var languageText = field.FieldName;
            var nameCheck = string.Format("{0}${1}.Label", modName, field.FieldName);
            var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck.Any())
            {
                languageText = langCheck.First().LanguageValue;
                return languageText;
            }
            return field.FieldName;
        }

        public static string GetLanguageMenu(this MenuItemInfo field, List<LanguageInfo> languages)
        {
            if (field != null && !string.IsNullOrEmpty(field.MenuName))
            {
                field.MenuName = field.MenuName.Trim();
            }
            var languageText = field.MenuName;
            var nameCheck = string.Format("MENU.{0}.TEXT", field.MenuName);
            var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck.Any())
            {
                languageText = langCheck.First().LanguageValue;
                return languageText;
            }
            return languageText;
        }
        public static string GetIconMenu(this MenuItemInfo field, List<LanguageInfo> languages)
        {
            if (field != null && !string.IsNullOrEmpty(field.MenuName))
            {
                field.MenuName = field.MenuName.Trim();
            }
            var languageText = "";
            var nameCheck = string.Format("MENU.{0}.ICON", field.MenuName);
            var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck != null)
            {
                if (langCheck.Any())
                    languageText = langCheck.First().LanguageValue;
                return languageText;
            }
            return languageText;
        }
        public static string GetLanguage(this string fieldName, List<LanguageInfo> languages, string modName)
        {
            var languageText = fieldName;
            var nameCheck = string.Format("{0}${1}.Label", modName, fieldName);
            var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck != null)
            {
                if (langCheck.Any())
                    languageText = langCheck.First().LanguageValue;
            }
            return languageText;
        }

        public static string GetLanguageTitlePage(this string modName, List<LanguageInfo> languages)
        {
            var languageText = modName;
            var nameCheck = string.Format("{0}.Title", modName);
            var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck != null)
            {
                if (langCheck.Any())
                    languageText = langCheck.First().LanguageValue;
            }
            return languageText;
        }
        public static string GetLanguageBtn(this string fieldName, List<LanguageInfo> languages)
        {
            if (!string.IsNullOrEmpty(fieldName))
            {
                fieldName = fieldName.Trim();
            }
            var languageText = fieldName;
            var nameCheck = string.Format("SEARCHMASTER${0}.Caption", fieldName);
            var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck != null)
            {
                if (langCheck.Any())
                    languageText = langCheck.First().LanguageValue.Replace("&", "");
            }
            return languageText;
        }
        public static string GetGroupFieldTextBtn(this ModuleFieldInfo field, List<LanguageInfo> languages)
        {
            return "";
        }
        public static string GetHtmlControl(this ModuleFieldInfo field, List<CodeInfoModel> dataComboBoxs = null, List<LanguageInfo> languageInfos = null, string subMod = "", List<dynamic> dataEdit = null, string modName = "", string keyEdit = "", int pageSlide = 0, List<ModuleFieldInfo> fieldSubmited = null)
        {
            if (languageInfos == null)
            {
                languageInfos = new List<LanguageInfo>();
            }
            AppConfiguration app = new AppConfiguration();
            var hostUrl = app.HostAddress;
            var valueControl = "";

            if (dataEdit != null)
            {
                foreach (var item in dataEdit)
                {
                    var dataRows = ((Newtonsoft.Json.Linq.JContainer)item);
                    if (field.ControlType == "IMG")
                    {
                        var valueControlTemp = "";
                        var parramTemp = "";
                        foreach (var column in dataRows)
                        {
                            var columnName = ((Newtonsoft.Json.Linq.JProperty)column).Name;
                            if (columnName.ToUpper() == field.FieldName.ToUpper())
                            {
                                if (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value != null)
                                {
                                    valueControlTemp = (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value).ToString().Trim();
                                }
                            }
                            else if (columnName.ToUpper() == field.ParameterName.ToUpper())
                            {
                                if (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value != null)
                                {
                                    parramTemp = (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value).ToString().Trim();
                                }
                            }
                        }
                        valueControl += parramTemp + "$" + valueControlTemp + ",";
                    }
                    else
                    {
                        foreach (var column in dataRows)
                        {
                            var columnName = ((Newtonsoft.Json.Linq.JProperty)column).Name;
                            if (columnName.ToUpper() == field.FieldName.ToUpper())
                            {
                                if (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value != null)
                                {
                                    valueControl += (((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)column).Value).Value).ToString().Trim() + ",";
                                }
                            }
                        }
                    }
                }
            }
            valueControl = valueControl.Trim(',');
            var validate = "";
            var classCustom = "";
            if (field.FieldType == EFieldType.INT.ToString() || field.FieldType == EFieldType.LNG.ToString())
            {
                classCustom = " text-number";
            }
            if (field.Nullable == EYesNo.N.ToString())
            {
                validate += " required='' aria-required='true'";
            }
            if (field.MaxLength > 0)
            {
                validate += string.Format(" maxlength='{0}'", field.MaxLength);
            }
            //validate = "";
            var languageText = field.FieldName;
            var nameCheck = string.Format("{0}${1}.Label", modName, field.FieldName);
            var langCheck = languageInfos.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            if (langCheck.Any())
            {
                languageText = langCheck.First().LanguageValue;
            }
            if (string.IsNullOrEmpty(valueControl) && fieldSubmited != null)
            {
                var checkValue = fieldSubmited.Where(x => x.FieldName.ToLower() == field.FieldName.ToLower());
                if (checkValue.Any())
                {
                    valueControl = checkValue.First().FieldID;
                }
            }
            switch (field.ControlType)
            {
                case "TB":
                    var disable = "";
                    if ((dataEdit != null && dataEdit.Count() > 0) && field.DisableEdit == "Y")
                    {
                        disable = "disabled='disabled'";
                    }
                    if (field.AutoCode == "Y" && (dataEdit == null || dataEdit.Count() == 0))
                    {
                        var currDate = DateTime.Now.ToString("ddMMyyhhmmss");
                        if (field.MaxLength - currDate.Length > 0)
                        {
                            valueControl = CommonFunction.RandomString(field.MaxLength - currDate.Length) + currDate;
                        }
                    }
                    if (!string.IsNullOrEmpty(field.Mod_Enter))
                    {

                        return string.Format("<InputText id='{0}' name='{1}' value='{2}' {3} class='form-control {4} {8}  @bind-Value='moduleFieldInfo[0].FieldId' enter-go-mod' placeholder='{5}' mod-enter='{6}' {7} {9}/>", field.FieldName, field.FieldName, valueControl, validate, classCustom, languageText, field.Mod_Enter, disable, string.IsNullOrEmpty(field.Callback) ? "" : "call-back", "callback='" + field.Callback + "'");
                    }
                    return string.Format("<input type='text' id='{0}' name='{1}' {2} {3} class='form-control {4} {7}'  @bind='abcd' placeholder='{5}' {6} {8}/>", field.FieldName, field.FieldName, valueControl, validate, classCustom, languageText, disable, string.IsNullOrEmpty(field.Callback) ? "" : "call-back", "callback='" + field.Callback + "'");
                case "CHK":
                    var check = "";
                    if ((valueControl.ToUpper() == "Y" || valueControl == "1") && field.DefaultValue == "Y")
                    {
                        check = "checked='checked'";
                    }
                    else if (string.IsNullOrEmpty(valueControl) && field.DefaultValue != "Y")
                    {
                        check = "";
                    }
                    return string.Format("<input type='checkbox' id='{0}' name='{1}' value='{2}' {3} />", field.FieldName, field.FieldName, field.DefaultValue, check);
                case "DT":
                    return string.Format("<input type='text' id='{0}' name='{1}' value='{2}' class='form-control datepicker' data-validation='date' data-validation-format='dd-mm-yyyy' {3}  placeholder='{4}' />", field.FieldName, field.FieldName, valueControl, validate, languageText);
                case "TA":
                    return string.Format("<textarea id='{0}' name='{1}' class='form-control' rows='4' {3} cols='50'  placeholder={4}>{2}</textarea>", field.FieldName, field.FieldName, valueControl, validate, languageText);
                case "CB":
                    var disableCb = "";
                    if ((dataEdit != null && dataEdit.Count() > 0) && field.DisableEdit == "Y")
                    {
                        disableCb = "disabled='disabled'";
                    }
                    var text = string.Format("<select id='{0}' name='{1}' class='form-control select2 {5}' {2} placeholder='{3}' {4} {6}>", field.FieldName, field.FieldName, validate, languageText, disableCb, string.IsNullOrEmpty(field.Callback) ? "" : "call-back", "callback='" + field.Callback + "'");
                    text += string.Format("<option selected='selected' value='{0}'>{1}</option>", "", String.Format("-- Chọn {0}--", languageText));
                    if (dataComboBoxs != null)
                    {
                        var options = dataComboBoxs.Where(x => x.Name.ToUpper() == field.FieldName.ToUpper());
                        if (options != null && options.Any())
                        {
                            foreach (var item in options.First().CodeInfos)
                            {
                                if (valueControl.ToUpper() == "True".ToUpper())
                                {
                                    valueControl = "Y";
                                }
                                else if (valueControl.ToUpper() == "FALSE")
                                {
                                    valueControl = "N";
                                }
                                if (item.CodeValue == valueControl || item.CodeValue == field.DefaultValue)
                                {
                                    text += string.Format("<option selected='selected' value='{0}'>{1}</option>", item.CodeValue, item.CodeValueName);
                                }
                                else
                                {
                                    text += string.Format("<option value='{0}'>{1}</option>", item.CodeValue, item.CodeValueName);
                                }
                            }
                        }
                    }

                    text += "</select>";
                    return text;
                case "MCB":
                    var disableMCb = "";
                    if ((dataEdit != null && dataEdit.Count() > 0) && field.DisableEdit == "Y")
                    {
                        disableMCb = "disabled='disabled'";
                    }
                    var textMCB = string.Format("<select id='{0}' name='{1}' class='form-control select2' {2} placeholder={3} {4} multiple='multiple'>", field.FieldName, field.FieldName, validate, languageText, disableMCb);
                    //textMCB += string.Format("<option selected='selected' value='{0}'>{1}</option>", "", String.Format("-- Chọn {0}--", languageText));
                    if (dataComboBoxs != null)
                    {
                        var options = dataComboBoxs.Where(x => x.Name.ToUpper() == field.FieldName.ToUpper());
                        if (options != null && options.Any())
                        {
                            foreach (var item in options.First().CodeInfos)
                            {
                                if (valueControl.ToUpper() == "True".ToUpper())
                                {
                                    valueControl = "Y";
                                }
                                else if (valueControl.ToUpper() == "FALSE")
                                {
                                    valueControl = "N";
                                }
                                if (item.CodeValue == valueControl || item.CodeValue == field.DefaultValue)
                                {
                                    textMCB += string.Format("<option selected='selected' value='{0}'>{1}</option>", item.CodeValue, item.CodeValueName);
                                }
                                else
                                {
                                    textMCB += string.Format("<option value='{0}'>{1}</option>", item.CodeValue, item.CodeValueName);
                                }
                            }
                        }
                    }

                    textMCB += "</select>";
                    return textMCB;
                case "CC":
                    var textCC = string.Format("<select id='{0}' name='{1}' class='form-control select2 {4}' {2}  placeholder='{3}' {5}>", field.FieldName, field.FieldName, validate, languageText, string.IsNullOrEmpty(field.Callback) ? "" : "call-back", "callback='" + field.Callback + "'");
                    textCC += string.Format("<option selected='selected' value='{0}'>{1}</option>", "", String.Format("-- Chọn {0}--", languageText));
                    if (dataComboBoxs != null)
                    {
                        var options = dataComboBoxs.Where(x => x.Name.ToUpper() == field.FieldName.ToUpper());
                        if (options != null && options.Any())
                        {
                            foreach (var item in options.First().CodeInfos)
                            {
                                if (item.CodeValue == valueControl)
                                {
                                    textCC += string.Format("<option selected='selected' value='{0}'>{1}</option>", item.CodeValue, item.CodeValueName);
                                }
                                else
                                {
                                    textCC += string.Format("<option value='{0}'>{1}</option>", item.CodeValue, item.CodeValueName);
                                }
                            }
                        }
                    }

                    textCC += "</select>";
                    return textCC;
                case "SP":
                    return string.Format("<textarea id='{0}' name='{1}' class='form-control ckeditor' rows='4' cols='50'  placeholder='{2}'></textarea>", field.FieldName, field.FieldName, languageText);
                case "PDF":
                    string urlFile = "";
                    string page = "";
                    if (dataComboBoxs != null)
                    {
                        var options = dataComboBoxs.Where(x => x.Name.ToUpper() == field.FieldName.ToUpper());
                        if (options != null)
                        {
                            foreach (var item in options.First().CodeInfos)
                            {
                                urlFile = item.CodeValueName;
                                page = item.CodeValue;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(urlFile))
                    {
                        return "";
                    }

                    var slideControl = string.Format("<iframe id='iframe' src='{0}/pdfjs-2.2.228-dist/web/viewer.html?file={0}/Uploads/{1}&page={2}' style='width:100%;min-height:40vh;'></iframe>", hostUrl, urlFile, int.Parse(page) + pageSlide); ;
                    return slideControl;
                case "LCB":
                    var textLCB = "";
                    if (dataComboBoxs != null)
                    {
                        var arrValues = valueControl.ToStringArrayUpper(',');
                        var options = dataComboBoxs.Where(x => x.Name.ToUpper() == field.FieldName.ToUpper());
                        if (options != null && options.Any())
                        {
                            int index = 0;
                            foreach (var item in options.First().CodeInfos)
                            {
                                if (valueControl.ToUpper() == "True".ToUpper())
                                {
                                    valueControl = "Y";
                                }
                                else if (valueControl.ToUpper() == "FALSE")
                                {
                                    valueControl = "N";
                                }
                                if (arrValues.Contains(item.CodeValue.ToUpper()))
                                {
                                    //if (string.IsNullOrEmpty(item.CodeValueName.GetLanguageTitle(languageInfos))) {
                                    //    var a = 1;
                                    //}

                                    textLCB += string.Format("<div class='col-lg-6 col-6'><input type='checkbox' checked='checked' name='{0}[{2}]' id='{0}' value='{1}'/> {3}</div>", field.FieldName, item.CodeValue, index, item.CodeValueName.GetLanguageTitle(languageInfos));
                                }
                                else
                                {
                                    textLCB += string.Format("<div class='col-lg-6 col-6'><input type='checkbox' name='{0}[{2}]' id='{0}' value='{1}'/> {3}</div>", field.FieldName, item.CodeValue, index, item.CodeValueName.GetLanguageTitle(languageInfos));
                                }
                                index++;
                            }
                        }
                    }
                    return textLCB;
                    break;
                case "UL"://Upload
                    var ulControl = "";
                    ulControl = String.Format("<div class='input-group'><div class='custom-file'><input type = 'file' class='custom-file-input' multiple id='{0}' name='{0}'><label class='custom-file-label' for='exampleInputFile'/>{1}</label></div></div>", field.FieldName, (String.IsNullOrEmpty(valueControl) ? "Choose file" : valueControl), valueControl);
                    return ulControl;
                case "SCL"://Schedule
                    var schedule = String.Format(@"<div class='wrap'><div id='{0}' class='calendar-schedule'></div><div style = 'clear:both'></div>", field.FieldName);
                    return schedule;
                case "IMG"://Image
                    var imgControl = "";
                    var split = valueControl.Split(',');
                    foreach (var item in split)
                    {
                        var valueParr = item.Split("$");
                        if (valueParr.Length > 1)
                        {
                            //string modId,string fieldName, string parr = ""
                            var actionLink = string.Format("{0}/Home/GoToMod?modId={1}&fieldName={2}&parr={3}&key={4}", hostUrl, field.Mod_Enter, field.ParameterName, valueParr[0], keyEdit);
                            imgControl += string.Format(@"<div class='form-group no-padding col-lg-6 col-6'><div class='card no-padding'><div class='col-12 left text-center no-padding card-body border-screen' style='height:100%'><a href='{3}'><img name ='{0}' id='{0}' class='img img-control' src='{1}/Uploads/{2}'/></div></div></div>", field.FieldName, hostUrl, valueParr[1], actionLink);
                        }
                        else
                            imgControl += string.Format(@"<div class='form-group no-padding col-lg-6 col-6'><div class='card no-padding'><div class='col-12 left text-center no-padding card-body border-screen' style='height:100%'><img name ='{0}' id='{0}' class='img img-control' src='{1}/Uploads/{2}'/></div></div></div>", field.FieldName, hostUrl, item);
                    }

                    return imgControl;
                case "BC"://Barcode
                    disable = "";
                    if ((dataEdit != null && dataEdit.Count() > 0) && field.DisableEdit == "Y")
                    {
                        disable = "disabled='disabled'";
                    }
                    if (!string.IsNullOrEmpty(field.Mod_Enter))
                    {
                        return string.Format("<div class='input-group'><input type='text' id='{0}' name='{1}' value='{2}' {3} class='form-control {4} barcode enter-go-mod' placeholder='{5}'  mod-enter='{6}' {7}/><button id='btnGoToMod' class='btn btn-default on-mobile' type='button'>Kiểm tra</button></div>", field.FieldName, field.FieldName, valueControl, validate, classCustom, languageText, field.Mod_Enter, disable);
                    }
                    return string.Format("<input type='text' id='{0}' name='{1}' value='{2}' {3} class='form-control {4} barcode' placeholder='{5}' {6}/>", field.FieldName, field.FieldName, valueControl, validate, classCustom, languageText, disable);
                default:
                    return string.Format("<input type='text' id='{0}' name='{1}' value='{2}' class='form-control'  placeholder={3} />", field.FieldName, field.FieldName, valueControl, languageText);
            }
        }
        public static string GetValueGridControl(this ModuleFieldInfo field, List<CodeInfoModel> dataComboBoxs)
        {
            //var checkSource = dataComboBoxs.Where(x => x.Name)
            var options = dataComboBoxs.Where(x => x.Name.ToUpper() == field.FieldName.ToUpper());
            if (options != null && options.Any())
            {
                var checkSource = options.First().CodeInfos.Where(x => x.CodeValue == field.FieldID);
                if (checkSource != null && checkSource.Any())
                {
                    return checkSource.First().CodeValueName;
                }
            }
            return field.FieldID;
        }
        public static string GetTextConfirmBtn(this ButtonInfo field, List<LanguageInfo> languages)
        {
            //if (field != null && !string.IsNullOrEmpty(field.MenuName))
            //{
            //    field.MenuName = field.MenuName.Trim();
            //}
            var languageText = "Bạn chắc chắn muốn xóa dữ liệu này";
            //var nameCheck = string.Format("MENU.{0}.ICON", field.MenuName);
            //var langCheck = languages == null ? new List<LanguageInfo>() : languages.Where(x => x.LanguageName.ToUpper() == nameCheck.ToUpper());
            //if (langCheck != null)
            //{
            //    if (langCheck.Any())
            //        languageText = langCheck.First().LanguageValue;
            //    return languageText;
            //}
            return languageText;
        }
        public static bool IsCallModMaintain(this ButtonInfo btn)
        {
            if (btn.CallSubModule!= ECallSubMod.MMN.ToString())
            {
                return true;
            }
            return false;
        }
        public static string IsButtonView(this ButtonInfo btn)
        {
            if (btn.CallSubModule == ECallSubMod.MVW.ToString())
            {
                return "0";
            }
            return "1";
        }
    }

    public class SwInputTextBase : InputBase<string>
    {
        [Parameter] 
        public string Id { get; set; }
        [Parameter] 
        public string Label { get; set; }
        [Parameter] 
        public Expression<Func<string>> ValidationFor { get; set; }
        
        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }
    }

}