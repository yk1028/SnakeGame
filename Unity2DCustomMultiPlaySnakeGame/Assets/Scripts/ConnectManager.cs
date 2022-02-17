using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class ConnectManager : MonoBehaviour
    {
        public GameObject connectButton;
        public InputField hostIPField;
        public InputField hostPortField;
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

            hostIPField.text = PlayerPrefs.GetString("hostIP", "");
            hostPortField.text = PlayerPrefs.GetString("hostPort", "");
        }

        public void PlayGame()
        {
            this.gameObject.SetActive(false);

            try
            {
                AsynchronousClient.StartClient(hostIPField.text, int.Parse(hostPortField.text));
                PlayerPrefs.SetString("hostIP", hostIPField.text);
                PlayerPrefs.SetString("hostPort", hostPortField.text);
                PlayerPrefs.Save();
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
