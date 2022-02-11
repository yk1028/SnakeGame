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

        public void Ready()
        {
            AsynchronousClient.SendStartRequest();
            readyButton.SetActive(false);
            connecting.SetActive(true);
        }

        public void ShowRecords(List<bool> records)
        {
            string result = "�ֱ� ���� ";

            if (records.Count == 0)
            {
                result += "����";
            } else
            {
                result += ": ";
            }

            foreach(bool record in records)
            {
                if (record)
                {
                    result += "�� ";
                }
                else
                {
                    result += "�� ";
                }
            }

            recordsText.text = result;
        }
    }
}
