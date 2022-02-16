using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    public class SnakeUpdateSupporter
    {
        private static readonly float SPEED = 5.0f;

        public static void UpdateHead(GameObject head, Vector2 headDirection)
        {
            var unitPerSec = SPEED * Time.deltaTime;

            head.transform.localPosition = new Vector2(head.transform.localPosition.x + headDirection.x * unitPerSec,
                head.transform.localPosition.y + headDirection.y * unitPerSec);
        }

        public static void UpdateTails(GameObject head, List<GameObject> tails)
        {
            var prev = head.transform.position;

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

        public static void RotateHead(GameObject head, Vector2 headDirection)
        {
            var radian = Mathf.Atan2(headDirection.y, headDirection.x);
            var degree = Mathf.Rad2Deg * radian;

            KeepHeadUp(head, degree);

            head.transform.localRotation = Quaternion.Euler(0, 0, degree);
        }

        private static void KeepHeadUp(GameObject head, float degree)
        {
            if (IsDirectionChanged(head, degree))
            {
                head.transform.localScale = new Vector3(head.transform.localScale.x, head.transform.localScale.y * -1f, head.transform.localScale.z);
            }
        }

        private static bool IsDirectionChanged(GameObject head, float degree)
        {
            return (IsLeft(degree) && head.transform.localScale.y >= 0)
                || (!IsLeft(degree) && head.transform.localScale.y < 0);
        }

        private static bool IsLeft(float degree)
        {
            return degree <= -90 || degree > 90;
        }
    }
}
