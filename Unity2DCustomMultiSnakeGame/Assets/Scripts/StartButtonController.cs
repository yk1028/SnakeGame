using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{

    public class StartButtonController : MonoBehaviour
    {
        public Text hostIP;
        public Text hostPort;

        public void PlayGame()
        {
            GameManager.Instance.Init();
            var thread = new Thread(() => AsynchronousClient.StartClient(hostIP.text, int.Parse(hostPort.text)));
            thread.Start();
        }
    }
}
