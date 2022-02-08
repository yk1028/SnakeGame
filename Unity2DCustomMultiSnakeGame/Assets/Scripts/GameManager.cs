using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class GameManager : MonoBehaviour
    {
        public GameObject mainMenu;

        public GameObject snakePrefab;
        public GameObject applePrefab;
        public GameObject tailPrefab;

        private static GameManager instance = null;

        public static GameObject enemySnake;

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
            Instantiate(applePrefab);
        }

        private void CreateMySnake()
        {
            GameObject snake = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
            snake.AddComponent<SnakeController>();
        }

        public void ReceiveEnemySnakeInfo(SnakeInfo snakeInfo)
        {
            if (enemySnake == null)
            {
                enemySnake = CreateEnemySnake(snakeInfo);
            }

            var ctrl = enemySnake.GetComponent<EnemySnakeController>();
            ctrl.ChangeHead(snakeInfo);
        }

        private GameObject CreateEnemySnake(SnakeInfo snakeInfo)
        {
            GameObject snake = Instantiate(snakePrefab, new Vector2(snakeInfo.positionX, snakeInfo.positionY), Quaternion.identity);
            snake.AddComponent<EnemySnakeController>();
            return snake;
        }
    }
}
