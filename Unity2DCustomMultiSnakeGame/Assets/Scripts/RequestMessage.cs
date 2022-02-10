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

        public void SetStartRequest()
        {
            message.Add("type", 0);
        }

        public void Set(SnakeInfo snakeInfo)
        {
            message.Add("type", 1);
            message.Add("snake", snakeInfo.snakeInfo);
        }

        public void Set(AppleInfo appleInfo)
        {
            message.Add("type", 2);
            message.Add("apple", appleInfo.appleInfo);
        }

        public byte[] ToSendData()
        {
            byte[] sendData = new byte[23500];
            string json = JsonConvert.SerializeObject(message);
            sendData = Encoding.UTF8.GetBytes(json);
            return sendData;
        }
    }
}