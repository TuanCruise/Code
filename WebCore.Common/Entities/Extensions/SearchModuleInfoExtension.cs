namespace WebCore.Entities.Extensions
{
    public static class SearchModuleInfoExtension
    {
        public static void SetAsLookUpWindow(this SearchModuleInfo obj,string columnValue)
        {
            obj.ShowAsLookUpWindow = true;
            obj.ColumnValue = columnValue;
            obj.ColumnText = null;
            obj.UIType = WebCore.CODES.DEFMOD.UITYPE.POPUP;
            obj.ShowCheckBox = WebCore.CODES.MODSEARCH.SHOWCHECKBOX.YES;
        }

        public static void SetAsLookUpWindow(this SearchModuleInfo obj, string columnValue, string columnText)
        {
            obj.ShowAsLookUpWindow = true;
            obj.ColumnValue = columnValue;
            obj.ColumnText = columnText;
            obj.UIType = WebCore.CODES.DEFMOD.UITYPE.POPUP;
            obj.ShowCheckBox = WebCore.CODES.MODSEARCH.SHOWCHECKBOX.YES;
        }

    }
}
