using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        private static readonly float SPEED = 5.0f;
        private static readonly int INIT_NUM_OF_TAILS = 2;
        private static readonly int NUM_OF_ADDITIONAL_TAILS = 1;
        private static readonly int NUM_OF_WINNIG_TAILS = 10;
        private static readonly Vector2 INIT_DIRECTION = new Vector2(1, 0);
        private static readonly Vector3 INIT_SCALE = new Vector3(0.05f, 0.05f, 1);

        private Vector2 headDirection;
        private List<GameObject> tails;

        public GameObject tailPrefab;

        void Awake()
        {
            this.tailPrefab.transform.localScale = INIT_SCALE;
            headDirection = INIT_DIRECTION;
            tails = new List<GameObject>();

            AddTails(INIT_NUM_OF_TAILS);
        }

        public void AddTails(int count)
        {
            Vector3 lastPosition = this.tails.Count == 0 ? this.transform.position : this.tails[tails.Count - 1].transform.position;

            for (int i = 1; i <= count; i++)
            {
                var tail = Instantiate(this.tailPrefab, new Vector3(lastPosition.x, lastPosition.y, 0), Quaternion.identity);
                tail.GetComponent<Renderer>().sortingOrder = -1 * (this.tails.Count + i);
                tails.Add(tail);
            }
        }

        public void Update()
        {
            ChangeHeadDirectionByTouch();
            UpdateHead();
            UpdateTails();
        }

        private void ChangeHeadDirectionByTouch()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touchPos = Input.mousePosition;
                var worldPos = Camera.main.ScreenToWorldPoint(touchPos);

                MoveTo(worldPos);
            }
        }

        private void MoveTo(Vector2 targetPos)
        {
            var distanceToTarget = Vector2.Distance(targetPos, transform.localPosition);

            headDirection.x = (targetPos.x - transform.localPosition.x) / distanceToTarget;
            headDirection.y = (targetPos.y - transform.localPosition.y) / distanceToTarget;

            RotateHead();
        }

        private void RotateHead()
        {
            var radian = Mathf.Atan2(headDirection.y, headDirection.x);
            var degree = Mathf.Rad2Deg * radian;

            KeepHeadUp(degree);

            transform.localRotation = Quaternion.Euler(0, 0, degree);
        }

        private void KeepHeadUp(float degree)
        {
            if (IsDirectionChanged(degree))
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1f, transform.localScale.z);
            }
        }

        private bool IsDirectionChanged(float degree)
        {
            return (IsLeft(degree) && transform.localScale.y >= 0)
                || (!IsLeft(degree) && transform.localScale.y < 0);
        }

        private bool IsLeft(float degree)
        {
            return degree <= -90 || degree > 90;
        }

        private void UpdateHead()
        {
            var unitPerSec = SPEED * Time.deltaTime;

            transform.localPosition = new Vector2(transform.localPosition.x + headDirection.x * unitPerSec,
                transform.localPosition.y + headDirection.y * unitPerSec);
        }

        private void UpdateTails()
        {
            var prev = transform.position;

            foreach (var cur in tails)
            {
                var distance = Vector2.Distance(prev, cur.transform.position);
                var x = (prev.x - cur.transform.localPosition.x) / distance;
                var y = (prev.y - cur.transform.localPosition.y) / distance;

                if (distance > 1)
                {
                    cur.transform.localPosition = new Vector2(prev.x - x, prev.y - y);
                }
                prev = cur.transform.localPosition;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Apple")
            {
                AddTails(NUM_OF_ADDITIONAL_TAILS);

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
