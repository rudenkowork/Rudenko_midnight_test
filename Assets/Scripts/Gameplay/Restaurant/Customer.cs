using System;
using System.Collections;
using Gameplay.Food;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Restaurant
{
    public class Customer : MonoBehaviour
    {
        private static readonly int _itch = Animator.StringToHash("Itch");

        public Transform FoodPosition;

        private Animator _animator;
        private bool _isEating;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(ItchSometimes());
        }

        public void TakeFood(FoodBase food)
        {
            food.transform.position = FoodPosition.position;
            _isEating = true;

            StartEating();
        }

        private IEnumerator ItchSometimes()
        {
            while (!_isEating)
            {
                _animator.SetTrigger(_itch);
                float randomTime = Random.Range(15f, 35f);
                yield return new WaitForSeconds(randomTime);
            }
        }

        private void StartEating()
        {
            // Eat Logic
            
            _isEating = false;
            StartCoroutine(ItchSometimes());
        }
    }
}