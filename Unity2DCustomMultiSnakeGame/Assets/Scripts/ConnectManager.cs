using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class ConnectManager : MonoBehaviour
    {
        public Text hostIP;
        public Text hostPort;

        public static ConnectManager Instace = null;

        private void Awake()
        {
            Instace = this;
        }

        public void PlayGame()
        {
            this.gameObject.SetActive(false);
            LoginManager.Instance.gameObject.SetActive(true);
           
            var thread = new Thread(() => AsynchronousClient.StartClient("34.64.155.34", 3000));
            //var thread = new Thread(() => AsynchronousClient.StartClient(hostIP.text, int.Parse(hostPort.text)));
            thread.Start();
        }
    }
}