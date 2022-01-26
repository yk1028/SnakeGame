using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AppleController : NetworkBehaviour
{
    private static readonly float MAP_WIDTH = 16.0f;
    private static readonly float MAP_HEIGHT = 7.0f;

    public GameObject applePrefab;

    private GameObject apple;

    public override void OnStartServer()
    {
        this.apple = Instantiate(applePrefab, new Vector3(GenerateRandom(MAP_WIDTH), GenerateRandom(MAP_HEIGHT), 0), Quaternion.identity);
        apple.AddComponent<AppleTrigger>();
        NetworkServer.Spawn(apple);
    }

    private float GenerateRandom(float bound)
    {
        var positionBound = bound - 1;
        return Mathf.Floor(Random.value * positionBound * 2 + 1) - positionBound;
    }
}
