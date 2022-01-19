using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public GameObject applePrefab;

    private GameObject apple;
    private float widthBound;
    private float heightBound;

    public void Init(float widthBound, float heightBound)
    {
        this.apple = Instantiate(applePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        this.widthBound = widthBound;
        this.heightBound = heightBound;

        LocateApple();
        apple.AddComponent<AppleTrigger>();
    }

    public void LocateApple()
    {
        apple.transform.position = new Vector2(GenerateRandom(widthBound), GenerateRandom(heightBound));
    }

    private float GenerateRandom(float bound)
    {
        var positionBound = bound - 1;
        return Mathf.Floor(Random.value * positionBound * 2 + 1) - positionBound;
    }
}
