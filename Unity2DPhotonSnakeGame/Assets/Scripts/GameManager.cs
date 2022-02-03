using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        public GameObject applePrefab;

        public GameObject gameEndMenu;

        public Text gameEndText;

        public static GameManager Instance;

        #endregion


        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion


        #region Public Methods

        public void Start()
        {
            Instance = this;

            gameEndMenu.SetActive(false);

            CreateApple();

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);

                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading...");
            PhotonNetwork.LoadLevel("Main Room");
        }

        public void CreateApple()
        {
            if (AppleManager.LocalAppleInstance == null && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(applePrefab.name, new Vector3(GenerateRandom(15), GenerateRandom(6), 0), Quaternion.identity);
            }
        }

        public void DestroyApple()
        {
            if (AppleManager.LocalAppleInstance != null && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(AppleManager.LocalAppleInstance);
            }
        }

        private float GenerateRandom(float bound)
        {
            return Mathf.Floor(UnityEngine.Random.value * bound * 2 + 1) - bound;
        }

        public void Win()
        {
            DestroyApple();
            gameEndText.text = "You Win!";
            gameEndMenu.SetActive(true);
        }

        public void GameOver()
        {
            DestroyApple();
            gameEndText.text = "Game Over";
            gameEndMenu.SetActive(true);
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }


        #endregion
    }
}