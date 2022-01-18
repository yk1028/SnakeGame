using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private static float SPEED = 3.0f;

    public GameObject headPrefab;

    private GameObject head;
    private Vector2 nextDir;

    void Start()
    {
        head = Instantiate(headPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        nextDir = new Vector2(1, 0);
    }

    void Update()
    {
        CheckTouchForDirection();
        UpdateHead();

        
    }

    private void CheckTouchForDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var touchPos = Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(touchPos);

            MoveTo(worldPos);
        }
    }

    private void UpdateHead()
    {
        head.transform.position = new Vector2(head.transform.position.x + nextDir.x * SPEED * Time.deltaTime,
            head.transform.position.y + nextDir.y * SPEED * Time.deltaTime);
    }

    void MoveTo(Vector2 targetPos)
    {
        var distanceToTarget = Vector2.Distance(targetPos, head.transform.position);

        nextDir.x = (targetPos.x - head.transform.position.x) / distanceToTarget;
        nextDir.y = (targetPos.y - head.transform.position.y) / distanceToTarget;
    }
}
