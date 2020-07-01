using System.ComponentModel;

namespace WebModelCore
{
    public enum EDefCode
    {
        SCDTYPE
    }
    public enum EUITYPE
    {
        [Description("POPUP")]
        P,
        [Description("NOWINDOW")]
        N,
        [Description("TABPAGE")]
        T
    }
    public enum ECallSubMod
    {
        [Description("Mod Edit")]
        MED,
        [Description("Mod Add")]
        MAD,
        [Description("Mod View")]
        MVW,
        [Description("Mod Excute")]
        MMN
    }
}
