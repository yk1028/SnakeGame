using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class GameManager : MonoBehaviour
    {
        private static readonly int NUM_OF_ADDITIONAL_TAILS = 1;
        private static readonly Vector2[] INIT_POSITION = { new Vector2(-12, 0), new Vector2(12, 0) };
        private static readonly Vector2[] INIT_DIRECTION = { new Vector2(0, 0), new Vector2(0, 0) };
        private static readonly Vector3 INIT_SCALE = new Vector3(0.05f, 0.05f, 1);

        public GameObject mainMenu;
        public GameObject gameOverMenu;
        public Text result;

        public GameObject snakePrefab;
        public GameObject enemySnakePrefab;
        public GameObject applePrefab;
        public GameObject tailPrefab;

        private static GameManager instance = null;

        public static GameObject apple;
        public static EnemySnakeController enemySnakeCtrl;
        public static SnakeController mySnakeCtrl;

        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            if (instance)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            this.tailPrefab.transform.localScale = INIT_SCALE;

            gameOverMenu.SetActive(false);

            DontDestroyOnLoad(this.gameObject);
        }

        public void Init(int clientID, float appleX, float appleY)
        {
            mainMenu.SetActive(false);
            enabled = true;

            CreateApple(appleX, appleY);
            CreateMySnake(clientID);
            CreateEnemySnake(1 - clientID);
        }

        private void CreateApple(float appleX, float appleY)
        {
            apple = Instantiate(applePrefab, new Vector2(appleX, appleY), Quaternion.identity);
        }

        private void CreateMySnake(int clientId)
        {
            var mySnake = Instantiate(snakePrefab, INIT_POSITION[clientId], Quaternion.identity);
            mySnakeCtrl = mySnake.GetComponent<SnakeController>();
            mySnakeCtrl.ChangeHeadDirection(INIT_DIRECTION[clientId]);

        }

        private void CreateEnemySnake(int clientId)
        {
            var enemySnake = Instantiate(enemySnakePrefab, INIT_POSITION[clientId], Quaternion.identity);
            enemySnakeCtrl = enemySnake.GetComponent<EnemySnakeController>();
            enemySnakeCtrl.ChangeHeadDirection(INIT_DIRECTION[clientId]);

        }

        public void SendSnakeInfo(Vector2 position, Vector2 direction)
        {
            AsynchronousClient.Send(new SnakeInfo(position.x, position.y, direction.x, direction.y));
        }

        public void ReceiveEnemySnakeInfo(float positionX, float positionY, float directionX, float directionY)
        {
            enemySnakeCtrl.ChangeHeadValue(positionX, positionY, directionX, directionY);
        }

        public void SendAppleInfo()
        {
            AsynchronousClient.Send(new AppleInfo(apple.transform.localPosition.x, apple.transform.localPosition.y));
        }

        public void ReceiveAppleInfo(float positionX, float positionY, bool isMine)
        {
            apple.transform.localPosition = new Vector2(positionX, positionY);
            if (isMine)
            {
                mySnakeCtrl.AddTails(NUM_OF_ADDITIONAL_TAILS);
            }
            else
            {
                enemySnakeCtrl.AddTails(NUM_OF_ADDITIONAL_TAILS);
            }
        }

        public void SendGameWin()
        {
            AsynchronousClient.SendGameEnd(true);
        }

        public void SendGameOver()
        {
            AsynchronousClient.SendGameEnd(false);
        }

        public void GameEnd(bool win)
        {
            mySnakeCtrl.Destroy();
            enemySnakeCtrl.Destroy();
            Destroy(mySnakeCtrl.gameObject);
            Destroy(enemySnakeCtrl.gameObject);
            Destroy(apple);


            gameOverMenu.SetActive(true);

            if (win)
            {
                result.text = "Win";
            }
            else
            {
                result.text = "GameOver";
            }
        }

        public void Exit()
        {
            AsynchronousClient.EndClient();
            ConnectManager.thread.Join();

            Application.Quit();
        }
    }
}
