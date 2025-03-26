using System;
using System.Collections;
using Services.AssetManagement;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        private Loading _loadingCurtain;

        public SceneLoader(ICoroutineRunner coroutineRunner, IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _coroutineRunner = coroutineRunner;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        public void Reload(Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(ReloadScene(onLoaded));

        public void InitialLoad(string sceneName, Action onLoaded = null)
        {
            var loadingCurtain = _assetProvider.LoadResource<Loading>(AssetPath.LoadingPath);
            _loadingCurtain = _instantiator.InstantiatePrefabForComponent(loadingCurtain);
            Load(sceneName, onLoaded);
        }

        private IEnumerator ReloadScene(Action onLoaded)
        {
            _loadingCurtain.Open();
            yield return AnimateProgressBar(0f, 0.7f, UnityEngine.Random.Range(1f, 1.3f));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
            yield return AnimateProgressBar(0.7f, 0.8f, 0.3f);

            var asyncOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            if (asyncOp != null)
            {
                asyncOp.allowSceneActivation = false;

                while (asyncOp.progress < 0.9f)
                    yield return null;

                float progress = 0.8f;
                float fillTime = 0.5f;
                while (progress < 1f)
                {
                    progress = Mathf.MoveTowards(progress, 1f, Time.deltaTime / fillTime);
                    _loadingCurtain.Progress.fillAmount = progress;
                    yield return null;
                }

                asyncOp.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(1f);

            _loadingCurtain.Close();
            onLoaded?.Invoke();
        }

        private IEnumerator LoadScene(string nextScene, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            _loadingCurtain.Open();
            yield return AnimateProgressBar(0f, 0.7f, UnityEngine.Random.Range(1f, 1.3f));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
            yield return AnimateProgressBar(0.7f, 0.8f, 0.3f);

            var asyncOp = SceneManager.LoadSceneAsync(nextScene);
            if (asyncOp != null)
            {
                asyncOp.allowSceneActivation = false;

                while (asyncOp.progress < 0.9f)
                    yield return null;

                float progress = 0.8f;
                float fillTime = 0.5f;
                while (progress < 1f)
                {
                    progress = Mathf.MoveTowards(progress, 1f, Time.deltaTime / fillTime);
                    _loadingCurtain.Progress.fillAmount = progress;
                    yield return null;
                }

                asyncOp.allowSceneActivation = true;
            }

            yield return new WaitUntil(() => asyncOp!.isDone);

            _loadingCurtain.Close();
            onLoaded?.Invoke();
        }

        private IEnumerator AnimateProgressBar(float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float value = Mathf.Lerp(from, to, elapsed / duration);
                _loadingCurtain.Progress.fillAmount = value;
                yield return null;
            }

            _loadingCurtain.Progress.fillAmount = to;
        }
    }
}