using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class ReadyManager : MonoBehaviour
    {
        public GameObject readyButton;
        public GameObject connecting;

        public static ReadyManager Instance;

        private void Awake()
        {
            this.gameObject.SetActive(false);
            Instance = this;
        }

        public void Ready()
        {
            AsynchronousClient.SendStartRequest();
            readyButton.SetActive(false);
            connecting.SetActive(true);
        }
    }
}
