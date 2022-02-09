using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class AppleController : MonoBehaviour
    {
        private static readonly float MAP_WIDTH = 16.0f;
        private static readonly float MAP_HEIGHT = 7.0f;

        public void LocateApple()
        {
            transform.localPosition = new Vector2(GenerateRandom(MAP_WIDTH), GenerateRandom(MAP_HEIGHT));
        }

        private float GenerateRandom(float bound)
        {
            var positionBound = bound - 1;
            return Mathf.Floor(UnityEngine.Random.value * positionBound * 2 + 1) - positionBound;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            LocateApple();
            GameManager.Instance.SendAppleInfo();
        }
    }
}
