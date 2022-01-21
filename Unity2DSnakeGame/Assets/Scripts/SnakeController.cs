using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private static readonly float SPEED = 5.0f;
    private static readonly int INIT_NUM_OF_TAILS = 2;
    private static readonly Vector2 INIT_DIRECTION = new Vector2(1, 0);
    private static readonly Vector3 INIT_POSITION = new Vector3(0, 0, 0);

    public GameObject headPrefab;
    public GameObject tailPrefab;

    private GameObject head;
    private Vector2 headDirection;
    private LinkedList<GameObject> tails;

    private List<GameObject> tailPool;

    void Start()
    {
        head = Instantiate(headPrefab, INIT_POSITION, Quaternion.identity);
        head.SetActive(false);

        tails = new LinkedList<GameObject>();
        tailPool = new List<GameObject>();
    }

    public void Init()
    {
        head.transform.position = INIT_POSITION;
        head.SetActive(true);
        headDirection = INIT_DIRECTION;

        AddTails(INIT_NUM_OF_TAILS);
    }

    public void AddTails(int count)
    {
        GameObject lastTail = this.tails.Count == 0 ? this.head : this.tails.Last.Value;

        for (int i = 1; i <= count; i++)
        {
            var tail = GetTailFromPool();
            tail.transform.position = new Vector3(lastTail.transform.position.x, lastTail.transform.position.y, 0);
            tail.GetComponent<Renderer>().sortingOrder = -1 * (this.tails.Count + i);
            tails.AddLast(tail);
        }

        head.transform.DetachChildren();
    }

    private GameObject GetTailFromPool()
    {
        foreach (var tail in tailPool)
        {
            if (!tail.activeSelf)
            {
                tail.SetActive(true);
                return tail;
            }
        }

        var newTail = Instantiate(tailPrefab, head.transform);
        tailPool.Add(newTail);

        return newTail;
    }

    public void UpdateSnake()
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
        var distanceToTarget = Vector2.Distance(targetPos, head.transform.position);

        headDirection.x = (targetPos.x - head.transform.position.x) / distanceToTarget;
        headDirection.y = (targetPos.y - head.transform.position.y) / distanceToTarget;

        RotateHead();
    }

    private void RotateHead()
    {
        var radian = Mathf.Atan2(headDirection.y, headDirection.x);
        var degree = Mathf.Rad2Deg * radian;

        head.transform.rotation = Quaternion.Euler(0, 0, degree);

        KeepHeadUp(degree);
    }

    private void KeepHeadUp(float degree)
    {
        var rigid = head.GetComponent<Rigidbody2D>();
        var renderer = rigid.GetComponent<SpriteRenderer>();

        renderer.flipY = degree <= -90 || degree > 90;
    }

    private void UpdateHead()
    {
        var unitPerSec = SPEED * Time.deltaTime;

        head.transform.position = new Vector2(head.transform.position.x + headDirection.x * unitPerSec,
            head.transform.position.y + headDirection.y * unitPerSec);
    }

    private void UpdateTails()
    {
        var prev = head;

        foreach (var cur in tails)
        {
            var distance = Vector2.Distance(prev.transform.position, cur.transform.position);
            var x = (prev.transform.position.x - cur.transform.position.x) / distance;
            var y = (prev.transform.position.y - cur.transform.position.y) / distance;

            if (distance > 1)
            {
                cur.transform.position = new Vector2(prev.transform.position.x - x, prev.transform.position.y - y);
            }
            prev = cur;
        }
    }

    public void Reset()
    {
        head.SetActive(false);

        foreach (var tail in tails)
        {
            tail.SetActive(false);
        }
        this.tails.Clear();
    }
}
