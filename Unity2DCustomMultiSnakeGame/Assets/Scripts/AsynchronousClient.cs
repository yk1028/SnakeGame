using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Com.Yk1028.SnakeGame
{
    public class AsynchronousClient
    {
        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent gameDone =
            new ManualResetEvent(false);

        private static Socket client;

        public static void StartClient(String hostIP, int serverPort)
        {
            // Connect to a remote server
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostIP);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, serverPort);

                // Create a TCP/IP socket.  
                client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                Receive(client);

                // Receive the response from the remote device.
                gameDone.WaitOne();

                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public static void EndClient()
        {
            gameDone.Set();
        }

        public static void Send(SnakeInfo info)
        {
            Send(Serialize(info));
        }

        public static void Send(AppleInfo info)
        {
            Send(Serialize(info));
        }

        private static void Send(byte[] byteData)
        {
            if (client != null)
            {
                client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            }
            // Begin sending the data to the remote device.  
            
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 1)
                {
                    object obj = Deserialize(state.buffer);

                    if (obj is SnakeInfo)
                    {
                        GameManager.Instance.ReceiveEnemySnakeInfo((SnakeInfo) obj);
                    } 
                    else if (obj is AppleInfo)
                    {
                        GameManager.Instance.ReceiveAppleInfo((AppleInfo) obj);
                    }
                }

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        public static byte[] Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
                return memoryStream.ToArray();
            }
        }

        public static object Deserialize(byte[] message)
        {
            using (var memoryStream = new MemoryStream(message))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
}