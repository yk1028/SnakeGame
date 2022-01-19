using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly float MAP_WIDTH = 16.0f;
    private static readonly float MAP_HEIGHT = 7.0f;

    public GameObject snake;
    public GameObject apple;
    public GameObject mainMenu;

    private SnakeController snakeCtrl;
    private AppleController appleCtrl;

    public bool isActive = false;

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void Init()
    {
        snakeCtrl = snake.GetComponent<SnakeController>();
        appleCtrl = apple.GetComponent<AppleController>();

        snakeCtrl.Init();
        appleCtrl.Init(MAP_WIDTH, MAP_HEIGHT);

        mainMenu.SetActive(false);
        isActive = true;
    }

    void Update()
    {
        if (isActive)
        {
            snakeCtrl.UpdateSnake();
        }
    }

    public void EatApple()
    {
        snakeCtrl.AddTails(1);
        appleCtrl.LocateApple();
    }

    public void Reset()
    {
        mainMenu.SetActive(true);
        isActive = false;

        snakeCtrl.Reset();
        appleCtrl.Reset();
    }
}
