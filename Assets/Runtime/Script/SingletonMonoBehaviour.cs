using UnityEngine;

namespace Runtime.Script
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"Duplicate singleton {typeof(T)}");
                return;
            }
            Instance = (T)this;
        }
    }
}