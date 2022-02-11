using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class RequestMessage
    {
        public JObject message = new JObject();

        public byte[] ToSendData()
        {
            string json = JsonConvert.SerializeObject(message);
            byte[] sendData = Encoding.UTF8.GetBytes(json);
            return sendData;
        }
    }
}