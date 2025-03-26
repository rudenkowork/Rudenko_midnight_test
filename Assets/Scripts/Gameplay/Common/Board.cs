using System.Collections;
using Gameplay.Food;
using Gameplay.Food.Services;
using Gameplay.Infrastructure.Controllers;
using Plugins.RContainer;
using Services.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Common
{
    public class Board : MonoBehaviour
    {
        public Transform CuttingPosition;
        public Button CutButton;
        public Transform Knife;

        [Header("Knife Points")] public Transform InitialKnifePoint;
        public Transform ReadyKnifePoint;

        private IFoodService _foodService;
        private IFoodFactory _foodFactory;

        private FoodBase _cuttingProduct;
        private FoodType _cuttingType;
        private bool _isCutting;
        private bool _isCut;
        private FoodType _cutResultType;

        private Coroutine _moveKnifeRoutine;

        [Inject]
        private void Construct(IFoodService foodService, IFoodFactory foodFactory)
        {
            _foodService = foodService;
            _foodFactory = foodFactory;
        }

        private void OnEnable()
        {
            CutButton.onClick.AddListener(OnCutButtonClicked);

            GameplayEventBus.Instance.OnBroadcasterChanged += HandleUI;
        }

        private void OnDisable()
        {
            CutButton.onClick.RemoveListener(OnCutButtonClicked);
            
            GameplayEventBus.Instance.OnBroadcasterChanged -= HandleUI;
        }

        private void HandleUI(BroadcastType broadcastType)
        {
            CutButton.gameObject.SetActive(broadcastType == BroadcastType.BOARD);
        }

        public void Open()
        {
            if (_moveKnifeRoutine != null) StopCoroutine(_moveKnifeRoutine);
            _moveKnifeRoutine = StartCoroutine(
                MoveKnife(Knife, ReadyKnifePoint.position, ReadyKnifePoint.rotation, 0.5f)
            );
        }

        public void Close()
        {
            if (_moveKnifeRoutine != null) StopCoroutine(_moveKnifeRoutine);
            _moveKnifeRoutine = StartCoroutine(
                MoveKnife(Knife, InitialKnifePoint.position, InitialKnifePoint.rotation, 0.5f)
            );
        }

        private void OnCutButtonClicked()
        {
            if (_isCutting) return;

            if (_isCut)
            {
                _foodService.Take(_cutResultType);
                _cutResultType = FoodType.UNKNOWN;

                if (_cuttingProduct != null)
                {
                    Destroy(_cuttingProduct.gameObject);
                    _cuttingProduct = null;
                }

                _isCut = false;
                return;
            }

            if (_foodService.SelectedFood is FoodType.RAW_TOMATO
                or FoodType.RAW_POTATO
                or FoodType.RAW_CHEESE)
            {
                _cuttingProduct = _foodFactory.CreateFood(_foodService.SelectedFood, CuttingPosition);
                _cuttingType = _foodService.SelectedFood;

                _foodService.Put(_cuttingType);

                StartCoroutine(CutRoutine());
            }
        }

        private IEnumerator CutRoutine()
        {
            _isCutting = true;

            yield return StartCoroutine(KnifeBobbing(3f));

            if (_cuttingProduct != null)
            {
                Destroy(_cuttingProduct.gameObject);
                _cuttingProduct = null;

                switch (_cuttingType)
                {
                    case FoodType.RAW_TOMATO:
                        _cutResultType = FoodType.SLICED_TOMATO;
                        break;
                    case FoodType.RAW_POTATO:
                        _cutResultType = FoodType.CHOPPED_POTATO;
                        break;
                    case FoodType.RAW_CHEESE:
                        _cutResultType = FoodType.SLICED_CHEESE;
                        break;
                    default:
                        _cutResultType = FoodType.UNKNOWN;
                        break;
                }

                if (_cutResultType != FoodType.UNKNOWN)
                {
                    _cuttingProduct = _foodFactory.CreateFood(_cutResultType, CuttingPosition);
                    _isCut = true;
                }
            }

            _isCutting = false;
        }

        private IEnumerator KnifeBobbing(float duration)
        {
            float time = 0f;
            Vector3 startPos = Knife.localPosition;

            while (time < duration)
            {
                time += Time.deltaTime;
                float offsetY = Mathf.Sin(time * 10f) * 0.02f;
                Knife.localPosition = startPos + new Vector3(0f, offsetY, 0f);

                yield return null;
            }

            Knife.localPosition = startPos;
        }
        
        private IEnumerator MoveKnife(Transform knife, Vector3 targetPos, Quaternion targetRot, float duration)
        {
            Vector3 startPos = knife.position;
            Quaternion startRot = knife.rotation;

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                knife.position = Vector3.Lerp(startPos, targetPos, t);
                knife.rotation = Quaternion.Slerp(startRot, targetRot, t);
                yield return null;
            }

            knife.position = targetPos;
            knife.rotation = targetRot;
        }
    }
}