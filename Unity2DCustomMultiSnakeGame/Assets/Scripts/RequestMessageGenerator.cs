using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class RequestMessageGenerator : MonoBehaviour
    {
        public static RequestMessage GenerateStartRequest()
        {
            var message = new RequestMessage();
            message.message.Add("type", 0);
            return message;
        }

        public static RequestMessage GenerateSnakeRequest(SnakeInfo snakeInfo)
        {
            var message = new RequestMessage();
            message.message.Add("type", 1);
            message.message.Add("snake", snakeInfo.snakeInfo);
            return message;
        }

        public static RequestMessage GenerateAppleRequest(AppleInfo appleInfo)
        {
            var message = new RequestMessage();
            message.message.Add("type", 2);
            message.message.Add("apple", appleInfo.appleInfo);
            return message;
        }

        public static RequestMessage GenerateGameOverRequest()
        {
            var message = new RequestMessage();
            message.message.Add("type", 3);
            return message;
        }
    }
}
