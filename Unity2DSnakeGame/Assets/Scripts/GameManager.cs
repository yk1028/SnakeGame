using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly float MAP_WIDTH = 5.0f;
    private static readonly float MAP_HEIGHT = 4.0f;

    public GameObject snake;
    public GameObject apple;

    private SnakeController snakeCtrl;
    private AppleController appleCtrl;

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

    void Start()
    {
        snakeCtrl = snake.GetComponent<SnakeController>();
        appleCtrl = apple.GetComponent<AppleController>();

        snakeCtrl.Init();
        appleCtrl.Init(MAP_WIDTH, MAP_HEIGHT);
    }

    void Update()
    {
        snakeCtrl.UpdateSnake();
    }

    public void EatApple()
    {
        snakeCtrl.AddTails(1);
        appleCtrl.LocateApple();
    }
}
