using System.Net;
using System.Net.Sockets;

namespace TCPConnector
{
    /// <summary>
    /// Base class inherited by classes performing data exchange operations over TCP.
    /// </summary>
    public abstract class TCPDataExchange
    {
        protected TcpClient clientGlobal;
        
        protected NetworkStream nwStreamGlobal;

        protected int privatePORT_NO;
        protected IPAddress privateLocalIPAddress = IPAddress.Any;
        protected int privateBufferSize = 8192;

        protected byte[] bufferGlobal;
        protected string stringDataExchanged;
    }
}
