using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public GameObject applePrefab;

    private GameObject apple;

    public void Start()
    {
        apple = Instantiate(applePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        locateApple(10, 7);
    }
    
    public void locateApple(float widthBound, float heightBound)
    {
        apple.transform.position = new Vector2(GenerateRandom(widthBound), GenerateRandom(heightBound));
    }

    private float GenerateRandom(float bound)
    {
        var positionBound = bound - 1;
        return Mathf.Floor(Random.value * positionBound *2 + 1) - positionBound;
    }
}
