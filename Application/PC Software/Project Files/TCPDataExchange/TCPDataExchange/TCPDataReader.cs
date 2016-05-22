using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

namespace TCPConnector
{
    /// <summary>
    /// Class to handle data receiving operations over the TCP Protocol.
    /// </summary>
    public class TCPDataReader : TCPDataExchange, IDisposable
    {

        #region Class Properties and Fields

        /// <summary>
        /// Port over which data will be received.
        /// </summary>
        public int LISTENING_PORT_NO 
        {
            get { return privatePORT_NO; }
            set { initializeWithPORT_NUMBER(value); }
        }

        /// <summary>
        /// IP Address of incoming data source.
        /// </summary>
        public string LISTENING_IP_ADDRESS
        {
            get { return privateLocalIPAddress.ToString(); }
            set { initializeMethod(value, privatePORT_NO); }
        }

        /// <summary>
        /// Size of data receiving buffer size. Default = 8192
        /// </summary>
        public int ReceiveBufferSize
        {
            get { return clientGlobal.ReceiveBufferSize; }
            set { privateBufferSize = value; }
        }

        /// <summary>
        /// String data received.
        /// </summary>
        public string ReceivedStringData { get { return stringDataExchanged; } }

        /// <summary>
        /// Byte stream data received.
        /// </summary>
        public byte[] ReceivedByteData { get { return bufferGlobal; } }

        /// <summary>
        /// Number of bytes read into the buffer.
        /// </summary>
        public int NoOfBytesRead { get { return bytesReadGlobal; } }
        
        //private IPAddress privateLocalIPAddress = IPAddress.Any;
        
        private TcpListener listenerGlobal;
        private int bytesReadGlobal;

        #endregion

        #region Class Constructors

        /// <summary>
        /// Constructor accepting IP Address and Port number over which data will be exchanged.
        /// </summary>
        /// <param name="IP_ADDRESS">IP Address of incoming data source.</param>
        /// <param name="PORT_NUMBER">Port over which data will be received.</param>
        public TCPDataReader(string IP_ADDRESS, int PORT_NUMBER)
        {
            initializeMethod(IP_ADDRESS, PORT_NUMBER);
        }

        /// <summary>
        /// Constructor accepting only Port Number over which data will be exchanged.
        /// Data will be exchanged over ANY IP Address using this port number if IP_ADDRESS property is left blank.
        /// </summary>
        /// <param name="PORT_NUMBER">Port over which data will be received.</param>
        public TCPDataReader(int PORT_NUMBER)
        {
            initializeWithPORT_NUMBER(PORT_NUMBER);
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Used to initialize object taking both IP Address and Port Number as input.
        /// </summary>
        /// <param name="IP_ADDRESS">IP Address from which to accept data.</param>
        /// <param name="PORT_NUMBER">Port over which data will be exchanged.</param>
        private void initializeMethod(string IP_ADDRESS, int PORT_NUMBER)
        {
            privateLocalIPAddress = IPAddress.Parse(IP_ADDRESS);
            initializeWithPORT_NUMBER(PORT_NUMBER);
        }

        /// <summary>
        /// Used to initialize object taking only port number. Data will be accepted from any IP Address.
        /// </summary>
        /// <param name="PORT_NUMBER">Port over which data will be exchanged.</param>
        private void initializeWithPORT_NUMBER(int PORT_NUMBER)
        {
            this.privatePORT_NO = PORT_NUMBER;
            listenerGlobal = new TcpListener(this.privateLocalIPAddress, this.privatePORT_NO);
        }

        #endregion

        #region Data Procuring Methods

        #region Listener Initialization Methods

        /// <summary>
        /// Starts the TCP Listener.
        /// </summary>
        public void StartListener()
        {
            listenerGlobal.Start();
        }

        /// <summary>
        /// Stops the TCP Listener.
        /// </summary>
        public void StopListener()
        {
            listenerGlobal.Stop();
        }

        #endregion

        #region Data Exchange with initialization methods

        /// <summary>
        /// Gets the data received as a string over the specified IP Address and Port number and updates the ReceivedData property.
        /// NOTE : This method requires explicit starting and stopping of the listener through the StartListener()
        /// and StopListener() methods.
        /// </summary>
        /// <returns>Data Received over the given port as string.</returns>
        public string GetReceivedStringData()
        {
            GetReceivedByteData();
            stringDataExchanged = Encoding.ASCII.GetString(bufferGlobal, 0, bytesReadGlobal);
            return stringDataExchanged;
        }

        /// <summary>
        /// Gets data as a byte stream over the specified IP Address and Port number, and stores in the
        /// buffer.
        /// NOTE : Listener must be explicitly started by calling the StartListener() method before calling this 
        /// function.
        /// </summary>
        /// <returns>Byte stream of received data.</returns>
        public byte[] GetReceivedByteData()
        {
            //---Global variables utilised---
            clientGlobal = listenerGlobal.AcceptTcpClient();

            clientGlobal.ReceiveBufferSize = privateBufferSize;

            nwStreamGlobal = clientGlobal.GetStream();
            bufferGlobal = new byte[clientGlobal.ReceiveBufferSize];

            bytesReadGlobal = nwStreamGlobal.Read(bufferGlobal, 0, clientGlobal.ReceiveBufferSize);
            return bufferGlobal;
        }

        #endregion

        #region Data Exchange without intialization methods

        /// <summary>
        /// Gets the data received as a string over the specified IP Address and Port number and updates the ReceivedData property.
        /// NOTE : This method does not require explicit starting and stopping of the listener through the StartListener()
        /// and StopListener() methods.
        /// </summary>
        /// <returns>Data Received over the given port as string.</returns>
        public string GetReceivedStringDataWithoutInitialization()
        {
            GetReceivedByteDataWithoutInitialization();

            //---convert the data received into a string--- 
            stringDataExchanged = Encoding.ASCII.GetString(bufferGlobal, 0, bytesReadGlobal);
            return stringDataExchanged;
        }

        /// <summary>
        /// Gets data as a byte stream over the specified IP Address and Port number, and stores in the
        /// buffer.
        /// This method creates a connection, receives data and then automtically closes the connection.
        /// NOTE : The start listener and stop listener methods need not be called.
        /// </summary>
        /// <returns>Byte stream of received data.</returns>
        public byte[] GetReceivedByteDataWithoutInitialization()
        {
            TcpListener l2 = new TcpListener(this.privateLocalIPAddress, this.privatePORT_NO);////////
            l2.Start();//////
            TcpClient client = l2.AcceptTcpClient();/////////
            

            client.ReceiveBufferSize = privateBufferSize;
            //---get the incoming data through a network stream---
            NetworkStream nwStreamLocal = client.GetStream();
            bufferGlobal = new byte[client.ReceiveBufferSize];

            //---read incoming stream---
            bytesReadGlobal = nwStreamLocal.Read(bufferGlobal, 0, client.ReceiveBufferSize);

            
            nwStreamLocal.Close();/////////////
            client.Close();
            l2.Stop();
            
            return bufferGlobal;
        }

        #endregion

        #endregion

        /// <summary>
        /// Disposes and releases all unmanaged resources utilized by this class.
        /// </summary>
        public void Dispose()
        {
            nwStreamGlobal.Dispose();
            clientGlobal.Close();
            listenerGlobal.Stop();
        }
    }
}
