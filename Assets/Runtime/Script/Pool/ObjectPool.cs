using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Runtime.Script.Pool
{
    public class ObjectPool<T> : SingletonMonoBehaviour<ObjectPool<T>>, IPool<T> where T : Object, IPoolObject
    {
        private Dictionary<Type, Queue<T>> _objectPool = new Dictionary<Type, Queue<T>>();

        public virtual T GetFromPool(Type objectType)
        {
            if (!_objectPool.ContainsKey(objectType))
            {
                _objectPool.Add(objectType, new Queue<T>());
                return null;
            }
            
            var available = _objectPool[objectType];
            
            if (available.Count <= 0)
            {
                return null;
            }
            
            return available.Dequeue();
        }

        public virtual void ReturnToPool(T obj)
        {
            Type objectType = obj.GetType();
            _objectPool[objectType].Enqueue(obj);
        }
    }

    public interface IPool<T> where T : IPoolObject
    {
        public T GetFromPool(Type objectType);
        public void ReturnToPool(T obj);
    }

    public interface IPoolObject
    {
        
    }
}