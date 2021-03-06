using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        private static readonly int INIT_NUM_OF_TAILS = 2;
        private static readonly int NUM_OF_WINNIG_TAILS = 5;

        private Vector2 headDirection;
        private List<GameObject> tails;

        void Awake()
        {
            tails = new List<GameObject>();

            AddTails(INIT_NUM_OF_TAILS);
        }

        public void AddTails(int count)
        {
            if (tails.Count + 1 >= NUM_OF_WINNIG_TAILS)
            {
                GameManager.Instance.SendGameWin();
            }

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
            ChangeHeadDirectionByTouch();
            SnakeUpdateSupporter.UpdateHead(this.gameObject, headDirection);
            if(tails != null)
            {
                SnakeUpdateSupporter.UpdateTails(this.gameObject, tails);
            }
        }

        private void ChangeHeadDirectionByTouch()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touchPos = Input.mousePosition;
                var worldPos = Camera.main.ScreenToWorldPoint(touchPos);

                MoveTo(worldPos);

                GameManager.Instance.SendSnakeInfo(transform.position, headDirection);
            }
        }

        private void MoveTo(Vector2 targetPos)
        {
            var distanceToTarget = Vector2.Distance(targetPos, transform.localPosition);

            headDirection.x = (targetPos.x - transform.localPosition.x) / distanceToTarget;
            headDirection.y = (targetPos.y - transform.localPosition.y) / distanceToTarget;

            SnakeUpdateSupporter.RotateHead(this.gameObject, headDirection);
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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Apple"))
            {
                GameManager.Instance.SendGameOver();
            }
        }
    }
}
