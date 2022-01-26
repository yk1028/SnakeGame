using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SnakeController : NetworkBehaviour
{
    private static readonly float SPEED = 5.0f;
    private static readonly int INIT_NUM_OF_TAILS = 2;
    private static readonly int NUM_OF_ADDITIONAL_TAILS = 1;
    private static readonly Vector2 INIT_DIRECTION = new Vector2(1, 0);
    private static readonly Vector3 INIT_POSITION = new Vector3(0, 0, 0);

    public GameObject tailPrefab;

    private Vector2 headDirection;

    private readonly SyncList<GameObject> tails = new SyncList<GameObject>();

    public override void OnStartLocalPlayer()
    {
        Init();
    }

    public void Init()
    {
        transform.position = INIT_POSITION;
        headDirection = INIT_DIRECTION;

        AddTails(INIT_NUM_OF_TAILS);
    }

    [Command]
    public void AddTails(int count)
    {
        Vector3 lastPosition = this.tails.Count == 0 ? this.transform.position : this.tails[tails.Count - 1].transform.position;

        for (int i = 1; i <= count; i++)
        {
            var tail = Instantiate(tailPrefab);
            tail.transform.localScale = new Vector3(0.05f, 0.05f, 1);
            tail.transform.position = new Vector3(lastPosition.x, lastPosition.y, 0);
            tail.GetComponent<Renderer>().sortingOrder = -1 * (this.tails.Count + i);
            tails.Add(tail);

            NetworkServer.Spawn(tail, this.gameObject);
          
            transform.DetachChildren();
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }

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
        var distanceToTarget = Vector2.Distance(targetPos, transform.position);

        headDirection.x = (targetPos.x - transform.position.x) / distanceToTarget;
        headDirection.y = (targetPos.y - transform.position.y) / distanceToTarget;

        RotateHead();
    }

    private void RotateHead()
    {
        var radian = Mathf.Atan2(headDirection.y, headDirection.x);
        var degree = Mathf.Rad2Deg * radian;

        KeepHeadUp(degree);

        transform.rotation = Quaternion.Euler(0, 0, degree);
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

        transform.position = new Vector2(transform.position.x + headDirection.x * unitPerSec,
            transform.position.y + headDirection.y * unitPerSec);
    }

    private void UpdateTails()
    {
        var prev = transform.position;

        foreach (var cur in tails)
        {
            var distance = Vector2.Distance(prev, cur.transform.position);
            var x = (prev.x - cur.transform.position.x) / distance;
            var y = (prev.y - cur.transform.position.y) / distance;

            if (distance > 1)
            {
                cur.transform.position = new Vector2(prev.x - x, prev.y - y);
            }
            prev = cur.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Apple")
        {
            AddTails(NUM_OF_ADDITIONAL_TAILS);
        } else
        {
            Reset();
        }
    }

    public void Reset()
    {
        CmdReset();
        Init();
    }

    [Command]
    private void CmdReset()
    {
        foreach (var tail in tails)
        {
            Destroy(tail);
        }

        tails.Clear();
    }
}
