using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class ConnectManager : MonoBehaviour
    {
        public Text hostIP;
        public Text hostPort;

        private static ConnectManager instance = null;

        public static Thread thread;

        public static ConnectManager Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            if (instance)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
        }

        public void PlayGame()
        {
            this.gameObject.SetActive(false);
            LoginManager.Instance.gameObject.SetActive(true);

            try
            {
                thread = new Thread(() => AsynchronousClient.StartClient("34.64.92.18", 3000));
                //var thread = new Thread(() => AsynchronousClient.StartClient(hostIP.text, int.Parse(hostPort.text)));
                thread.Start();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        }
    }
}
