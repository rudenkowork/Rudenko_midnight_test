using System.Collections;
using UnityEngine;

namespace Services.SceneManagement
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopAllCoroutines();
    }
}