using UnityEngine;

namespace Gameplay.Infrastructure
{
    public class Broadcaster : MonoBehaviour
    {
        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}