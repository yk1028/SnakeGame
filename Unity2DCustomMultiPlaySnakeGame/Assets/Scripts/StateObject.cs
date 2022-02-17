using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// State object for reading client data asynchronously
namespace Com.Yk1028.SnakeGame
{
    public class StateObject
    {
        private static readonly UTF8Encoding utf8 = new UTF8Encoding();
        private static readonly string[] seperator = { "<EOJ>" };
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            CheckAdditionalContent = false
        };

        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Client socket.
        public Socket workSocket = null;

        public List<ResponseMessage> GetResponseMessages()
        {
            string[] jsons = utf8.GetString(buffer)
                .Split(seperator, StringSplitOptions.None);
            jsons = jsons.Take(jsons.Count() - 1).ToArray();
            List<ResponseMessage> messages = new List<ResponseMessage>();

            foreach (var json in jsons)
            {
                Debug.Log(json);
                messages.Add(JsonConvert.DeserializeObject<ResponseMessage>(json, settings));
            }
            return messages;
        }

        public void ClearBuffer()
        {
            Array.Clear(buffer, 0, buffer.Length);
        }
    }
}
