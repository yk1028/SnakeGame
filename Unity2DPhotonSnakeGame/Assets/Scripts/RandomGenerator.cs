using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class RandomGenerator : MonoBehaviour
    {
        public static float GenerateRandom(float bound)
        {
            var positionBound = bound - 1;
            return Mathf.Floor(UnityEngine.Random.value * positionBound * 2 + 1) - positionBound;
        }
    }
}

