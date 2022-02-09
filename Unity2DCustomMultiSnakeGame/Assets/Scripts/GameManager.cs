using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class GameManager : MonoBehaviour
    {
        private static readonly Vector2 INIT_APPLE_POSITION = new Vector2(0, 0);
        private static readonly Vector2[] INIT_POSITION = { new Vector2(-12, 0), new Vector2(12, 0) };
        private static readonly Vector2[] INIT_DIRECTION = { new Vector2(1, 0), new Vector2(-1, 0) };

        public GameObject mainMenu;

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

            this.tailPrefab.transform.localScale = new Vector3(0.05f, 0.05f, 1);

            DontDestroyOnLoad(this.gameObject);
        }

        public void Init(int clientID)
        {
            mainMenu.SetActive(false);
            enabled = true;

            CreateApple();
            CreateMySnake(clientID);
            CreateEnemySnake(1- clientID);
        }

        private void CreateApple()
        {
            apple = Instantiate(applePrefab, INIT_APPLE_POSITION, Quaternion.identity);
        }

        private void CreateMySnake(int initIndex)
        {
            var mySnake = Instantiate(snakePrefab, INIT_POSITION[initIndex], Quaternion.identity);
            mySnakeCtrl = mySnake.GetComponent<SnakeController>();
            mySnakeCtrl.headDirection = INIT_DIRECTION[initIndex];
        }

        private void CreateEnemySnake(int initIndex)
        {
            var enemySnake = Instantiate(enemySnakePrefab, INIT_POSITION[initIndex], Quaternion.identity);
            enemySnakeCtrl = enemySnake.GetComponent<EnemySnakeController>();
            enemySnakeCtrl.headDirection = INIT_DIRECTION[initIndex];
        }

        public void SendSnakeInfo(Vector2 position, Vector2 direction)
        {
            AsynchronousClient.Send(new SnakeInfo(position.x, position.y, direction.x, direction.y));
        }

        public void ReceiveEnemySnakeInfo(SnakeInfo snakeInfo)
        {
            enemySnakeCtrl.ChangeHead(snakeInfo);
        }
        public void SendAppleInfo()
        {
            AsynchronousClient.Send(new AppleInfo(apple.transform.localPosition));
        }

        public void ReceiveAppleInfo(AppleInfo appleInfo)
        {
            apple.transform.localPosition = appleInfo.GetPosition();
            enemySnakeCtrl.AddTails(1);
        }
    }
}
