using System.Collections;
using Gameplay.Food.Services;
using UnityEngine;

namespace Gameplay.Common
{
    public class Fridge : MonoBehaviour
    {
        public GameObject Light;
        public Transform Door;

        private bool _isOpen;
        private Coroutine _currentCoroutine;

        public void Open()
        {
            if (_isOpen) return;
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(RotateDoor(-161f, true));
            _isOpen = true;
        }

        public void Close()
        {
            if (!_isOpen) return;
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(RotateDoor(0, false));
            _isOpen = false;
        }

        private IEnumerator RotateDoor(float deltaY, bool isOpened)
        {
            Quaternion startRotation = Door.rotation;
            Quaternion endRotation = Quaternion.Euler(270f, 180 + deltaY, 0f);

            float duration = 0.6f;
            float time = 0f;

            while (time < duration)
            {
                Door.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            Door.rotation = endRotation;

            Light.SetActive(isOpened);
        }
    }
}