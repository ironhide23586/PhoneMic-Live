using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConnector
{
    /// <summary>
    /// Class to handle data sending operations over the TCP Protocol.
    /// </summary>
    public class TCPDataSender : TCPDataExchange
    {
        /// <summary>
        /// IP Adress of target machine to which dat is sent.
        /// </summary>
        public string SENDING_IP_ADDRESS 
        { 
            get { return privateLocalIPAddress.ToString(); }

            set 
            { 
                privateLocalIPAddress = IPAddress.Parse(value);
                clientGlobal = new TcpClient(value, privatePORT_NO); 
            }
        }

        /// <summary>
        /// Port over which the data is to be sent.
        /// </summary>
        public int SENDING_PORT_NO
        {
            get { return privatePORT_NO; }

            set
            {
                privatePORT_NO = value;
                clientGlobal = new TcpClient(privateLocalIPAddress.ToString(), value);
            }
        }

        /// <summary>
        /// Constructor accepting target server's IP address and port number as arguments.
        /// </summary>
        /// <param name="TARGET_SERVER_IP">IP Address of server to which the data is to be sent.</param>
        /// <param name="PORT_NO">Target port to which the dat is to be sent.</param>
        public TCPDataSender(string TARGET_SERVER_IP, int PORT_NO)
        {
            clientGlobal = new TcpClient(TARGET_SERVER_IP, PORT_NO);
            privateLocalIPAddress = IPAddress.Parse(TARGET_SERVER_IP);
            privatePORT_NO = PORT_NO;
        }
    }
}
