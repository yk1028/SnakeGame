using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{

    public class StartButtonController : MonoBehaviour
    {
        public Text hostIP;
        public Text hostPort;
        public GameObject connecting;

        public void PlayGame()
        {
            this.gameObject.SetActive(false);
            connecting.SetActive(true);

            var thread = new Thread(() => AsynchronousClient.StartClient(hostIP.text, int.Parse(hostPort.text)));
            thread.Start();
        }
    }
}
