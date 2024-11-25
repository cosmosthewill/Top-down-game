using Runtime.Script.Pool;
using UnityEngine;

namespace Runtime.Script.CollectibleObject
{
    public class BaseCollectibleObject : MonoBehaviour, IPoolObject
    {
        [SerializeField] private CollectibleType type; 
        [SerializeField] private float value;
    }

    public enum CollectibleType
    {
        Exp,
        Health,
        Mana,
    }
}