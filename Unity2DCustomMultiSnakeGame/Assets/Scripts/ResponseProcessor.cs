using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Yk1028.SnakeGame
{
    enum ResponseType : int
    {
        Start_Game = 0,
        Change_Snake,
        Change_Apple,
        Game_End,
        Find_User,
        Create_User_Success,
        Find_User_Record

    }

    class ResponseProcessor
    {
        private delegate void Process(ResponseMessage rm);

        private static Dictionary<ResponseType, Process> dispatchers;

        public static void run(ResponseType type, ResponseMessage rm)
        {
            dispatchers[type](rm);
        }
        public static void Init()
        {
            dispatchers = new Dictionary<ResponseType, Process>();

            dispatchers.Add(ResponseType.Start_Game, (rm) =>
            {
                int clientId = (int)rm.message.GetValue("clientId");
                bool canStart = (bool)rm.message.GetValue("start");

                if (canStart)
                {
                    JObject apple = (JObject)rm.message.GetValue("apple");
                    float positionX = (float)apple.GetValue("posX");
                    float positionY = (float)apple.GetValue("posY");

                    UnityMainThread.thread.AddJob(() =>
                    {
                        GameManager.Instance.Init(clientId, positionX, positionY);
                    });
                }
            });

            dispatchers.Add(ResponseType.Change_Snake, (rm) =>
            {
                JObject snake = (JObject)rm.message.GetValue("snake");
                float positionX = (float)snake.GetValue("posX");
                float positionY = (float)snake.GetValue("posY");
                float directionX = (float)snake.GetValue("dirX");
                float directionY = (float)snake.GetValue("dirY");

                UnityMainThread.thread.AddJob(() =>
                {
                    GameManager.Instance.ReceiveEnemySnakeInfo(positionX, positionY, directionX, directionY);
                });
            });

            dispatchers.Add(ResponseType.Change_Apple, (rm) =>
            {
                JObject apple = (JObject)rm.message.GetValue("apple");
                float positionX = (float)apple.GetValue("posX");
                float positionY = (float)apple.GetValue("posY");
                bool isMine = (bool)rm.message.GetValue("isMine");

                UnityMainThread.thread.AddJob(() =>
                {
                    GameManager.Instance.ReceiveAppleInfo(positionX, positionY, isMine);
                });
            });

            dispatchers.Add(ResponseType.Game_End, (rm) =>
            {
                bool win = (bool)rm.message.GetValue("win");

                UnityMainThread.thread.AddJob(() =>
                {
                    GameManager.Instance.GameEnd(win);
                });
            });

            dispatchers.Add(ResponseType.Find_User, (rm) =>
            {
                bool isExist = (bool)rm.message.GetValue("isExist");

                if (isExist)
                {
                    UnityMainThread.thread.AddJob(() =>
                    {
                        LoginManager.Instance.LoginSucess();
                    });
                }
                else
                {
                    UnityMainThread.thread.AddJob(() =>
                    {
                        LoginManager.Instance.LoginFail();
                    });
                }
            });

            dispatchers.Add(ResponseType.Create_User_Success, (rm) =>
            {
                bool isSuccess = (bool)rm.message.GetValue("isSuccess");

                UnityMainThread.thread.AddJob(() =>
                {
                    LoginManager.Instance.LoginSucess();
                });
            });

            dispatchers.Add(ResponseType.Find_User_Record, (rm) =>
            {
                JArray records = (JArray)rm.message.GetValue("records");

                List<bool> list = new List<bool>();

                foreach (JObject record in records)
                {
                    bool win = (bool)record.GetValue("win");
                    list.Add(win);
                }

                UnityMainThread.thread.AddJob(() =>
                {
                    ReadyManager.Instance.ShowRecords(list);
                });
            });
        }
    }
}
