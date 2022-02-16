using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// State object for reading client data asynchronously
namespace Com.Yk1028.SnakeGame
{
    public class StateObject
    {
        private static UTF8Encoding utf8 = new UTF8Encoding();

        // Size of receive buffer.  
        public const int BufferSize = 4096;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Client socket.
        public Socket workSocket = null;

        public ResponseMessage GetResponseMessage()
        {
            string json = utf8.GetString(buffer);
            return JsonConvert.DeserializeObject<ResponseMessage>(json); ;
        }

        public void ClearBuffer()
        {
            Array.Clear(buffer, 0, buffer.Length);
        }
    }
}
