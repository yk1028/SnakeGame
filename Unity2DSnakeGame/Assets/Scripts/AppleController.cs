using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public GameObject applePrefab;

    private GameObject apple;
    private float widthBound;
    private float heightBound;

    void Start()
    {
        this.apple = Instantiate(applePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        this.apple.SetActive(false);
        this.apple.AddComponent<AppleTrigger>();
    }

    public void Init(float widthBound, float heightBound)
    {
        this.apple.SetActive(true);
        this.widthBound = widthBound;
        this.heightBound = heightBound;

        LocateApple();
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

    public void Reset()
    {
        apple.SetActive(false);
    }
}
