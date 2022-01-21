using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly float MAP_WIDTH = 16.0f;
    private static readonly float MAP_HEIGHT = 7.0f;
    private static readonly int NUM_OF_ADDITIONAL_TAILS = 1;

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject mainMenu;

    private SnakeController snakeCtrl;
    private AppleController appleCtrl;

    void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);

        snakeCtrl = this.GetComponent<SnakeController>();
        appleCtrl = this.GetComponent<AppleController>();
    }

    public void Init()
    {
        snakeCtrl.Init();
        appleCtrl.Init(MAP_WIDTH, MAP_HEIGHT);

        mainMenu.SetActive(false);
        enabled = true;
    }

    void Update()
    {
        snakeCtrl.UpdateSnake();
    }

    public void EatApple()
    {
        snakeCtrl.AddTails(NUM_OF_ADDITIONAL_TAILS);
        appleCtrl.LocateApple();
    }

    public void Reset()
    {
        mainMenu.SetActive(true);
        enabled = false;

        snakeCtrl.Reset();
        appleCtrl.Reset();
    }
}
