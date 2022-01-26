using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AppleTrigger : NetworkBehaviour
{
    private static readonly float MAP_WIDTH = 16.0f;
    private static readonly float MAP_HEIGHT = 7.0f;
    void OnTriggerEnter2D(Collider2D other)
    {
        this.gameObject.transform.position = new Vector2(GenerateRandom(MAP_WIDTH), GenerateRandom(MAP_HEIGHT));
    }
    private float GenerateRandom(float bound)
    {
        var positionBound = bound - 1;
        return Mathf.Floor(Random.value * positionBound * 2 + 1) - positionBound;
    }

}
