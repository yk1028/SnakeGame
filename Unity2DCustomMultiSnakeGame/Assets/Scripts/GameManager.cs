using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class GameManager : MonoBehaviour
    {
        public GameObject mainMenu;

        public GameObject snakePrefab;
        public GameObject enemySnakePrefab;
        public GameObject applePrefab;
        public GameObject tailPrefab;

        private static GameManager instance = null;

        public static GameObject apple;
        public static EnemySnakeController enemySnakeCtrl;

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

        public void Init()
        {
            mainMenu.SetActive(false);
            enabled = true;

            CreateApple();
            CreateMySnake();
        }

        private void CreateApple()
        {
            apple = Instantiate(applePrefab);
        }

        private void CreateMySnake()
        {
            Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
        }

        private GameObject CreateEnemySnake(SnakeInfo snakeInfo)
        {
            return Instantiate(enemySnakePrefab, new Vector2(snakeInfo.positionX, snakeInfo.positionY), Quaternion.identity);
        }

        public void SendSnakeInfo(Vector2 position, Vector2 direction)
        {
            AsynchronousClient.Send(new SnakeInfo(position.x, position.y, direction.x, direction.y));
        }

        public void ReceiveEnemySnakeInfo(SnakeInfo snakeInfo)
        {
            if (enemySnakeCtrl == null)
            {
                var enemySnake = CreateEnemySnake(snakeInfo);
                enemySnakeCtrl = enemySnake.GetComponent<EnemySnakeController>();
            }

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
