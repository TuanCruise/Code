using System.Threading.Tasks;

namespace WebAppCoreBlazorServer.Service
{
    public interface IDITest
    {
        Task<string> Test();
    }
}