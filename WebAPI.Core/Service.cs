using System.Threading.Tasks;
using GreenPipes.Util;

namespace Core.API
{
    public class Service :
        IService
    {
        public Task ServiceTheThing(string value)
        {
            return TaskUtil.Completed;
        }
    }
}