using System;

namespace Hotfix.framework
{
    public class TcpService : Service
    {
        protected override Connection Apply(int connectionID, Connection.Handle handle)
        {
            return new TcpConnect(connectionID, handle);
        }
    }
}