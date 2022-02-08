using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class SnakeController : MonoBehaviour
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

        private void AddTails(int count)
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
            ChangeHeadDirectionByTouch();
            SnakeUpdateSupporter.UpdateHead(this.gameObject, headDirection);
            SnakeUpdateSupporter.UpdateTails(this.gameObject, tails);
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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Apple")
            {
                AddTails(NUM_OF_ADDITIONAL_TAILS);

                GameManager.Instance.SendAppleInfo();

                if (tails.Count >= NUM_OF_WINNIG_TAILS)
                {
                    // win
                }
            }
            else
            {
                // lose
            }
        }
    }
}
