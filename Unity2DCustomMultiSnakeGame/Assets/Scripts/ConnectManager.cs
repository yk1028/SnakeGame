using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class ConnectManager : MonoBehaviour
    {
        public GameObject connectButton;
        public Text hostIP;
        public Text hostPort;
        public Text errorText;

        private static ConnectManager instance = null;

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

            try
            {
                AsynchronousClient.StartClient(hostIP.text, int.Parse(hostPort.text));
                LoginManager.Instance.Init();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                errorText.text = e.Message;
                this.gameObject.SetActive(true);
            }
        }
    }
}
