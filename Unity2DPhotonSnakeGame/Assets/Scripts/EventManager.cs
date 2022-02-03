using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Com.Yk1028.SnakeGame
{
    public class EventManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        object[] content = new object[] { };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        private readonly byte winOthersEvent = 1;
        private readonly byte loseOthersEvent = 0;

        public static EventManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void RaiseWinOthersEvent()
        {
            PhotonNetwork.RaiseEvent(winOthersEvent, content, raiseEventOptions, sendOptions);
        }

        public void RaiseLoseOthersEvent()
        {
            PhotonNetwork.RaiseEvent(loseOthersEvent, content, raiseEventOptions, sendOptions);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == winOthersEvent)
            {
                if(PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.Win();
                }
            }
            else if (eventCode == loseOthersEvent)
            {
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.Lose();
                }
            }
        }

        public override void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}
