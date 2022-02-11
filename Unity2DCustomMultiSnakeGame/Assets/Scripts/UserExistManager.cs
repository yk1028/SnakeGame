using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class UserExistManager : MonoBehaviour
    {
        public static UserExistManager Instance;

        private void Awake()
        {
            this.gameObject.SetActive(false);
            Instance = this;
        }

        public void CreateUser()
        {
            this.gameObject.SetActive(false);
            AsynchronousClient.SendCreateUser(LoginManager.Instance.userName.text);
        }

        public void BackToLogin()
        {
            this.gameObject.SetActive(false);
            LoginManager.Instance.gameObject.SetActive(true);
        }
    }
}
