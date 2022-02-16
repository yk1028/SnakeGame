using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Yk1028.SnakeGame
{
    public class ReadyManager : MonoBehaviour
    {
        public GameObject readyButton;
        public GameObject connecting;
        public Text recordsText;

        public static ReadyManager Instance;

        private void Awake()
        {
            this.gameObject.SetActive(false);
            Instance = this;
        }

        public void Init()
        {
            this.gameObject.SetActive(true);
            readyButton.SetActive(true);
            connecting.SetActive(false);
        }

        public void Ready()
        {
            AsynchronousClient.SendStartRequest();
            readyButton.SetActive(false);
            connecting.SetActive(true);
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
            readyButton.SetActive(false);
            connecting.SetActive(false);
        }

        public void ShowRecords(List<bool> records)
        {
            string result = records.Count == 0 ? "최근 전적 없음" : "최근 전적 : ";

            foreach (bool record in records)
            {
                result += record ? "승 " : "패 ";
            }

            recordsText.text = result;
        }
    }
}
