using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class EnemySnakeController : MonoBehaviour
    {
        private static readonly int INIT_NUM_OF_TAILS = 2;

        private Vector2 headDirection;
        private List<GameObject> tails;

        void Awake()
        {
            tails = new List<GameObject>();

            AddTails(INIT_NUM_OF_TAILS);
        }

        public void AddTails(int count)
        {
            Vector3 lastPosition = tails.Count == 0 ? this.transform.position : tails[tails.Count - 1].transform.position;

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

            if (tails != null)
            {
                SnakeUpdateSupporter.UpdateTails(this.gameObject, tails);
            }

        }

        public void ChangeHead(float positionX, float positionY, float directionX, float directionY)
        {
            this.transform.localPosition = new Vector2(positionX, positionY);

            ChangeHeadDirection(new Vector2(directionX, directionY));
        }

        public void ChangeHeadDirection(Vector2 direction)
        {
            headDirection = new Vector2(direction.x, direction.y);
            SnakeUpdateSupporter.RotateHead(this.gameObject, headDirection);

        }

        public void Destroy()
        {
            foreach (var tail in tails)
            {
                Destroy(tail);
            }
            tails = null;
        }
    }
}
