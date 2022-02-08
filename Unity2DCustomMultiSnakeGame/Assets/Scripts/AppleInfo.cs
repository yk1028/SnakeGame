using System;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    [SerializableAttribute]
    public class AppleInfo
    {
        public float positionX;
        public float positionY;

        public AppleInfo(Vector3 position)
        {
            positionX = position.x;
            positionY = position.y;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(positionX, positionY);
        }
    }
}
