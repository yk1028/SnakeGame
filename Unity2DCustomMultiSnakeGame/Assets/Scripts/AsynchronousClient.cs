using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

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

        public static void SendStartRequest()
        {
            Send(RequestMessageGenerator
                .GenerateStartRequest()
                .ToSendData());
        }

        public static void Send(SnakeInfo info)
        {
            Send(RequestMessageGenerator
                .GenerateSnakeRequest(info)
                .ToSendData());
        }

        public static void Send(AppleInfo info)
        {
            Send(RequestMessageGenerator
                .GenerateAppleRequest(info)
                .ToSendData());
        }
        public static void SendGameEnd(bool win)
        {
            Send(RequestMessageGenerator
                .GenerateGameEndRequest(win)
                .ToSendData());
        }

        public static void SendFindUser(string username)
        {
            Send(RequestMessageGenerator
                .GenerateFindUserRequest(username)
                .ToSendData());
        }

        public static void SendCreateUser(string username)
        {
            Send(RequestMessageGenerator
                .GenerateCreateUserRequest(username)
                .ToSendData());
        }

        private static void Send(byte[] byteData)
        {
            if (client != null)
            {
                client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
            }

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
                    ResponseMessage rm = state.GetResponseMessage();

                    int type = (int)rm.message.GetValue("type");

                    if (type == 0)
                    {
                        int clientId = (int)rm.message.GetValue("clientId");
                        bool canStart = (bool)rm.message.GetValue("start");

                        if (canStart)
                        {
                            JObject apple = (JObject)rm.message.GetValue("apple");
                            float positionX = (float)apple.GetValue("posX");
                            float positionY = (float)apple.GetValue("posY");

                            GameManager.Instance.Init(clientId, positionX, positionY);
                        }
                    }
                    else if (type == 1)
                    {
                        JObject snake = (JObject)rm.message.GetValue("snake");
                        float positionX = (float)snake.GetValue("posX");
                        float positionY = (float)snake.GetValue("posY");
                        float directionX = (float)snake.GetValue("dirX");
                        float directionY = (float)snake.GetValue("dirY");

                        GameManager.Instance.ReceiveEnemySnakeInfo(positionX, positionY, directionX, directionY);
                    }
                    else if (type == 2)
                    {
                        JObject apple = (JObject)rm.message.GetValue("apple");
                        float positionX = (float)apple.GetValue("posX");
                        float positionY = (float)apple.GetValue("posY");
                        bool isMine = (bool)rm.message.GetValue("isMine");

                        GameManager.Instance.ReceiveAppleInfo(positionX, positionY, isMine);
                    }
                    else if (type == 3)
                    {
                        bool win = (bool)rm.message.GetValue("win");

                        GameManager.Instance.GameEnd(win);
                    }
                    else if (type == 4)
                    {
                        bool isExist = (bool)rm.message.GetValue("isExist");
                        
                        if (isExist)
                        {
                            LoginManager.Instance.LoginSucess();
                        } else
                        {
                            LoginManager.Instance.LoginFail();
                        }
                    }
                    else if (type == 5)
                    {
                        bool isSuccess = (bool)rm.message.GetValue("isSuccess");

                        LoginManager.Instance.LoginSucess();
                    }
                }

                state.ClearBuffer();

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
}