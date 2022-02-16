using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    [SerializableAttribute]
    public class AppleInfo
    {
        public JObject appleInfo = new JObject();

        public AppleInfo(float positionX, float positionY)
        {
            appleInfo.Add("posX", positionX);
            appleInfo.Add("posY", positionY);
        }
    }
}
