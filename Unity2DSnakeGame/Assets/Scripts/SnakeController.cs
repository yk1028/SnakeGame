using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private static float SPEED = 5.0f;
    private static int INIT_NUM_OF_TAILS = 10;
    private static Vector2 INIT_DIRECTION = new Vector2(1,0);

    public GameObject headPrefab;
    public GameObject tailPrefab;

    private GameObject head;
    private Vector2 headDirection;
    private LinkedList<GameObject> tails;

    void Start()
    {
        head = Instantiate(headPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        headDirection = INIT_DIRECTION;
        tails = new LinkedList<GameObject>();

        for (int i = 1; i <= INIT_NUM_OF_TAILS; i++)
        {
            var tail = Instantiate(tailPrefab, new Vector3(-1 * i , 0, 0), Quaternion.identity, head.transform);

            tail.GetComponent<Renderer>().sortingOrder = -1 * i;
            tails.AddLast(tail);
        }

        head.transform.DetachChildren();

    }

    void Update()
    {
        ChangeOfHeadDirectionByTouch();
        UpdateHead();
        UpdateTails();
    }
    
    private void ChangeOfHeadDirectionByTouch()
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

        rotateHead();
    }

    private void rotateHead()
    {
        var radian = Mathf.Atan2(headDirection.y, headDirection.x);
        var degree = Mathf.Rad2Deg * radian;

        head.transform.rotation = Quaternion.Euler(0, 0, degree);
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

        foreach(var cur in tails)
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
}
