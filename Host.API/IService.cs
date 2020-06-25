using System.Threading.Tasks;

namespace Core.API
{
    public interface IService
    {
        Task ServiceTheThing(string value);
    }
}