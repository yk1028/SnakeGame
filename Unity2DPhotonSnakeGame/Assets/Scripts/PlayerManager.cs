using System;
using System.Collections;


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
        private static readonly Vector3 INIT_POSITION = new Vector3(0, 0, 0);
        private static readonly Quaternion INIT_ROTATION = new Quaternion(0, 0, 0, 0);

        private Vector2 headDirection;

        #endregion

        #region Public Field

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region Public Methods



        #endregion

        #region MonoBehaviour CallBacks

        public void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);

            headDirection = INIT_DIRECTION;
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
        }

        #endregion

        #region Custom

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

        #endregion
    }
}