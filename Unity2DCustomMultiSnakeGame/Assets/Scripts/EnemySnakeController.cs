using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class EnemySnakeController : MonoBehaviour
    {
        private static readonly int INIT_NUM_OF_TAILS = 2;

        private Vector2 headDirection;
        private ConcurrentBag<GameObject> tails;

        private bool isChanged = false;
        private Vector2 nextPosition;
        private Vector2 nextDirection;

        private readonly object _lockObj = new object();

        void Awake()
        {
            tails = new ConcurrentBag<GameObject>();

            AddTails(INIT_NUM_OF_TAILS);
        }

        public void AddTails(int count)
        {
            GameObject lastTail;
            Vector3 lastPosition = tails.TryPeek(out lastTail) ?  lastTail.transform.position : this.transform.position;

            for (int i = 1; i <= count; i++)
            {
                var tail = Instantiate(GameManager.Instance.tailPrefab, new Vector3(lastPosition.x, lastPosition.y, 0), Quaternion.identity);
                tail.GetComponent<Renderer>().sortingOrder = -1 * (this.tails.Count + i);
                tails.Add(tail);
            }
        }

        public void Update()
        {
            lock (_lockObj)
            {
                if (isChanged)
                {
                    ChangeHead();
                    isChanged = false;
                }
            }

            SnakeUpdateSupporter.UpdateHead(this.gameObject, headDirection);

            if (tails != null)
            {
                SnakeUpdateSupporter.UpdateTails(this.gameObject, tails);
            }

        }

        public void ChangeHeadValue(float positionX, float positionY, float directionX, float directionY)
        {
            lock (_lockObj)
            {
                this.nextPosition = new Vector2(positionX, positionY);
                this.nextDirection = new Vector2(directionX, directionY);
                this.isChanged = true;
            }
        }

        private void ChangeHead()
        {
            this.transform.localPosition = this.nextPosition;

            ChangeHeadDirection(this.nextDirection);
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
