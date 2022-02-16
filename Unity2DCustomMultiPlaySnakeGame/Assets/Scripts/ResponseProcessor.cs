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

        private static Dictionary<ResponseType, Process> dispatcher;

        public static void run(ResponseType type, ResponseMessage rm)
        {
            dispatcher[type](rm);
        }

        public static void Init()
        {
            dispatcher = new Dictionary<ResponseType, Process>();

            dispatcher.Add(ResponseType.Start_Game, (rm) => StartGame(rm));
            dispatcher.Add(ResponseType.Change_Snake, (rm) => ChangeSnake(rm));
            dispatcher.Add(ResponseType.Change_Apple, (rm) => ChangeApple(rm));
            dispatcher.Add(ResponseType.Game_End, (rm) => GameEnd(rm));
            dispatcher.Add(ResponseType.Find_User, (rm) => FindUser(rm));
            dispatcher.Add(ResponseType.Create_User_Success, (rm) => CreateUserSuccess(rm));
            dispatcher.Add(ResponseType.Find_User_Record, (rm) => FindUserRecord(rm));
        }

        private static void StartGame(ResponseMessage rm)
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
        }

        private static void ChangeSnake(ResponseMessage rm)
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
        }

        private static void ChangeApple(ResponseMessage rm)
        {
            JObject apple = (JObject)rm.message.GetValue("apple");
            float positionX = (float)apple.GetValue("posX");
            float positionY = (float)apple.GetValue("posY");
            bool isMine = (bool)rm.message.GetValue("isMine");

            UnityMainThread.thread.AddJob(() =>
            {
                GameManager.Instance.ReceiveAppleInfo(positionX, positionY, isMine);
            });
        }

        private static void GameEnd(ResponseMessage rm)
        {
            bool win = (bool)rm.message.GetValue("win");

            UnityMainThread.thread.AddJob(() =>
            {
                GameManager.Instance.GameEnd(win);
            });
        }

        private static void FindUser(ResponseMessage rm)
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
        }

        private static void CreateUserSuccess(ResponseMessage rm)
        {
            bool isSuccess = (bool)rm.message.GetValue("isSuccess");

            UnityMainThread.thread.AddJob(() =>
            {
                LoginManager.Instance.LoginSucess();
            });
        }

        private static void FindUserRecord(ResponseMessage rm)
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
        }
    }
}
