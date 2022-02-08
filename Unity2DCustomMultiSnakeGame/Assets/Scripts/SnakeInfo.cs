using System;

namespace Com.Yk1028.SnakeGame
{
    [SerializableAttribute]
    public class SnakeInfo
    {
        public float positionX;
        public float positionY;
        public float directionX;
        public float directionY;

        public SnakeInfo(float positionX, float positionY, float directionX, float directionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.directionX = directionX;
            this.directionY = directionY;
        }

        public override string ToString()
        {
            return "position : (" + positionX + ", " + positionY + "), direction : (" + directionX + ", " + directionY + ")";
        }
    }
}
