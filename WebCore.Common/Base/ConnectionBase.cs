using System;
using WebCore.Common;

namespace WebCore.Base
{
    public abstract class ConnectionBase : IDisposable
    {
        protected string ConnectionString { get; set; }

        protected ConnectionBase()
        {
            ConnectionString = App.Configs.ConnectionString;
        }

        protected ConnectionBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Dispose()
        {
        }
    }
}
