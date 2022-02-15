using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class LoginManager : MonoBehaviour
    {
        public Text userName;

        public static LoginManager Instance;

        private void Awake()
        {
            this.gameObject.SetActive(false);
            Instance = this;

            userName.text = PlayerPrefs.GetString("userName", "");
        }

        public void Login()
        {
            AsynchronousClient.SendFindUser(userName.text);
        }

        public void LoginSucess()
        {
            this.gameObject.SetActive(false);
            ReadyManager.Instance.Init();

            PlayerPrefs.SetString("userName", userName.text);
            PlayerPrefs.Save();

            AsynchronousClient.SendFindUserRecord();
        }

        public void LoginFail()
        {
            this.gameObject.SetActive(false);
            UserExistManager.Instance.gameObject.SetActive(true);
        }

        public void Restart()
        {
            this.gameObject.SetActive(true);
            userName.text = PlayerPrefs.GetString("userName", "");
        }
    }
}
