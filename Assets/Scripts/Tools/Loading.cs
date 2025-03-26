using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class Loading : MonoBehaviour
    {
        public Image Progress;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            Progress.fillAmount = 0;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            Progress.fillAmount = 1;
        }
    }
}