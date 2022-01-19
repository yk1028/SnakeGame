using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.LocateApple();
    }
}
