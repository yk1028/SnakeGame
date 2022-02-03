using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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

            transform.position = new Vector2(RandomGenerator.GenerateRandom(MAP_WIDTH), RandomGenerator.GenerateRandom(MAP_HEIGHT));
        }
        public void CreateApple()
        {
            if (AppleManager.LocalAppleInstance == null && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(applePrefab.name, new Vector3(RandomGenerator.GenerateRandom(15),
                    RandomGenerator.GenerateRandom(6), 0), Quaternion.identity);
            }
        }

        public void DestroyApple()
        {
            if (AppleManager.LocalAppleInstance != null && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(AppleManager.LocalAppleInstance);
            }
        }

    }
}
