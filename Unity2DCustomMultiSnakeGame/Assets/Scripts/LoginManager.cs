using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class LoginManager : MonoBehaviour
    {
        public InputField userNameField;

        public static LoginManager Instance;

        private void Awake()
        {
            this.gameObject.SetActive(false);
            Instance = this;
        }

        public void Init()
        {
            this.gameObject.SetActive(true);

            userNameField.text = PlayerPrefs.GetString("userName", "");
        }

        public void Login()
        {
            AsynchronousClient.SendFindUser(userNameField.text);
        }

        public void LoginSucess()
        {
            this.gameObject.SetActive(false);
            ReadyManager.Instance.Init();

            PlayerPrefs.SetString("userName", userNameField.text);
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
            Init();
        }
    }
}
