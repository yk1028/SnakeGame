using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject gameMenu;

    public void playGame()
    {
        GameManager.Instance.Init();
    }

    public void SetActive(bool isActive)
    {
        gameMenu.SetActive(isActive);
    }
}
