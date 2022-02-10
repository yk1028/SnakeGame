using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class AppleController : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(DateTime.Now);
            GameManager.Instance.SendAppleInfo();
        }
    }
}
