using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


namespace Com.Yk1028.SnakeGame
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Private Field

        private static readonly float SPEED = 5.0f;
        private static readonly int INIT_NUM_OF_TAILS = 2;
        private static readonly int NUM_OF_ADDITIONAL_TAILS = 1;
        private static readonly Vector2 INIT_DIRECTION = new Vector2(1, 0);
        private static readonly Vector3 INIT_SCALE = new Vector3(0.05f, 0.05f, 1);

        private Vector2 headDirection;
        private List<GameObject> tails;

        #endregion

        #region Public Field

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        public GameObject tailPrefab;

        #endregion

        #region Public Methods

        public void Init()
        {
            headDirection = INIT_DIRECTION;
            tails = new List<GameObject>();

            AddTails(INIT_NUM_OF_TAILS);
        }

        #endregion


        #region MonoBehaviour CallBacks

        public void Awake()
        {
            this.tailPrefab.transform.localScale = INIT_SCALE;

            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                Init();
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            ChangeHeadDirectionByTouch();
            UpdateHead();
            UpdateTails();
        }

        #endregion

        #region Custom

        public void AddTails(int count)
        {
            Vector3 lastPosition = this.tails.Count == 0 ? this.transform.position : this.tails[tails.Count - 1].transform.position;

            for (int i = 1; i <= count; i++)
            {
                var tail = PhotonNetwork.Instantiate(this.tailPrefab.name, new Vector3(lastPosition.x, lastPosition.y, 0), Quaternion.identity, 0);
                tail.GetComponent<Renderer>().sortingOrder = -1 * (this.tails.Count + i);
                tails.Add(tail);
                DontDestroyOnLoad(tail);
            }
        }

        private void ChangeHeadDirectionByTouch()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touchPos = Input.mousePosition;
                var worldPos = Camera.main.ScreenToWorldPoint(touchPos);

                MoveTo(worldPos);
            }
        }

        private void MoveTo(Vector2 targetPos)
        {
            var distanceToTarget = Vector2.Distance(targetPos, transform.position);

            headDirection.x = (targetPos.x - transform.position.x) / distanceToTarget;
            headDirection.y = (targetPos.y - transform.position.y) / distanceToTarget;

            RotateHead();
        }

        private void RotateHead()
        {
            var radian = Mathf.Atan2(headDirection.y, headDirection.x);
            var degree = Mathf.Rad2Deg * radian;

            KeepHeadUp(degree);

            transform.rotation = Quaternion.Euler(0, 0, degree);
        }

        private void KeepHeadUp(float degree)
        {
            if (IsDirectionChanged(degree))
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1f, transform.localScale.z);
            }
        }

        private bool IsDirectionChanged(float degree)
        {
            return (IsLeft(degree) && transform.localScale.y >= 0)
                || (!IsLeft(degree) && transform.localScale.y < 0);
        }

        private bool IsLeft(float degree)
        {
            return degree <= -90 || degree > 90;
        }

        private void UpdateHead()
        {
            var unitPerSec = SPEED * Time.deltaTime;

            transform.position = new Vector2(transform.position.x + headDirection.x * unitPerSec,
                transform.position.y + headDirection.y * unitPerSec);
        }

        private void UpdateTails()
        {
            var prev = transform.position;

            foreach (var cur in tails)
            {
                var distance = Vector2.Distance(prev, cur.transform.position);
                var x = (prev.x - cur.transform.position.x) / distance;
                var y = (prev.y - cur.transform.position.y) / distance;

                if (distance > 1)
                {
                    cur.transform.position = new Vector2(prev.x - x, prev.y - y);
                }
                prev = cur.transform.position;
            }
        }

        #endregion

        void OnTriggerEnter2D(Collider2D other)
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }


            if (other.tag == "Apple")
            {
                AddTails(NUM_OF_ADDITIONAL_TAILS);
            }
            else
            {
                PhotonNetwork.Destroy(AppleManager.LocalAppleInstance);
                PhotonNetwork.Destroy(LocalPlayerInstance);

                GameManager.Instance.LeaveRoom();
            } 
        }
    }
}