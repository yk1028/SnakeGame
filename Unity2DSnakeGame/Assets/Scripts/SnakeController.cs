using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private static float SPEED = 3.0f;

    public GameObject headPrefab;

    private GameObject head;
    private Vector2 headDirection;

    void Start()
    {
        head = Instantiate(headPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        headDirection = new Vector2(1, 0);
    }

    void Update()
    {
        ChangeOfHeadDirectionByTouch();
        UpdateHead();
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

    private void UpdateHead()
    {
        head.transform.position = new Vector2(head.transform.position.x + headDirection.x * SPEED * Time.deltaTime,
            head.transform.position.y + headDirection.y * SPEED * Time.deltaTime);
    }

    void MoveTo(Vector2 targetPos)
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
}
