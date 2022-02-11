using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class LoginManager : MonoBehaviour
    {
        public Text UserName;

        public void Login()
        {
            this.gameObject.SetActive(false);
            ConnectManager.Instace.Active();
        }
    }
}
