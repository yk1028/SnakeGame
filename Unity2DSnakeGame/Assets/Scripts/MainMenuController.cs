using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;

    public void playGame()
    {
        GameManager.Instance.Init();
    }

    public void SetActive(bool isActive)
    {
        mainMenu.SetActive(isActive);
    }
}
