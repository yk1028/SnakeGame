using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class EnemySnakeController : MonoBehaviour
    {
        private static readonly int INIT_NUM_OF_TAILS = 2;
        private static readonly int NUM_OF_ADDITIONAL_TAILS = 1;
        private static readonly int NUM_OF_WINNIG_TAILS = 10;
        private static readonly Vector2 INIT_DIRECTION = new Vector2(1, 0);

        private Vector2 headDirection;
        private List<GameObject> tails;

        void Awake()
        {
            headDirection = INIT_DIRECTION;
            tails = new List<GameObject>();

            AddTails(INIT_NUM_OF_TAILS);
        }

        public void AddTails(int count)
        {
            Vector3 lastPosition = this.tails.Count == 0 ? this.transform.position : this.tails[tails.Count - 1].transform.position;

            for (int i = 1; i <= count; i++)
            {
                var tail = Instantiate(GameManager.Instance.tailPrefab, new Vector3(lastPosition.x, lastPosition.y, 0), Quaternion.identity);
                tail.GetComponent<Renderer>().sortingOrder = -1 * (this.tails.Count + i);
                tails.Add(tail);
            }
        }

        public void Update()
        {
            SnakeUpdateSupporter.UpdateHead(this.gameObject, headDirection);
            SnakeUpdateSupporter.UpdateTails(this.gameObject, tails);
        }

        public void ChangeHead(SnakeInfo snakeInfo)
        {
            this.transform.localPosition = new Vector2(snakeInfo.positionX, snakeInfo.positionY);
            this.headDirection.x = snakeInfo.directionX;
            this.headDirection.y = snakeInfo.directionY;
            SnakeUpdateSupporter.RotateHead(this.gameObject, headDirection);
        }
    }
}
