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

        private static GameManager instance = null;

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

            DontDestroyOnLoad(this.gameObject);
        }

        public void Init()
        {
            mainMenu.SetActive(false);
            enabled = true;

            CreateApple();
            CreateSnake();
        }

        private void CreateApple()
        {
            Instantiate(applePrefab);
        }

        private void CreateSnake()
        {
            Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
