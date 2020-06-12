namespace WebCore.Base
{
    public interface IModule
    {
        object this[string fieldID] { get; set; }
    }
}
