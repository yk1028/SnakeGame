using Newtonsoft.Json.Linq;
using System;

namespace Com.Yk1028.SnakeGame
{
    [Serializable]
    public class SnakeInfo
    {
        public JObject snakeInfo = new JObject();

        public SnakeInfo(float positionX, float positionY, float directionX, float directionY)
        {
            snakeInfo.Add("posX", positionX);
            snakeInfo.Add("posY", positionY);
            snakeInfo.Add("dirX", directionX);
            snakeInfo.Add("dirY", directionY);
        }
    }
}
