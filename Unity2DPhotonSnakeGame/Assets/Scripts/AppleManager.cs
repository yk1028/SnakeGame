using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Yk1028.SnakeGame
{

    public class AppleManager : MonoBehaviourPunCallbacks
    {
        private static readonly float MAP_WIDTH = 16.0f;
        private static readonly float MAP_HEIGHT = 7.0f;

        public static GameObject LocalAppleInstance;

        public void Awake()
        {
            if (photonView.IsMine)
            {
                LocalAppleInstance = this.gameObject;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            transform.position = new Vector2(GenerateRandom(MAP_WIDTH), GenerateRandom(MAP_HEIGHT));
        }

        private float GenerateRandom(float bound)
        {
            var positionBound = bound - 1;
            return Mathf.Floor(UnityEngine.Random.value * positionBound * 2 + 1) - positionBound;
        }
    }
}
