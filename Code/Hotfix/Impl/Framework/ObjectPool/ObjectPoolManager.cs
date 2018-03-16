using System.Collections;
using System.Collections.Generic;
using System;

namespace Hotfix.framework
{
    public class ObjectPoolManager : IObjectPoolManager
    {
        private readonly Dictionary<Type, ObjectPoolBase> mObjectPools = new Dictionary<Type, ObjectPoolBase>();

        public IObjectPool<T> GetObjectPool<T>() where T : IPoolObject
        {
            ObjectPoolBase tmpObjectPool;

            if (!mObjectPools.TryGetValue(typeof(T), out tmpObjectPool))
            {
                tmpObjectPool = new ObjectPool<T>();
                mObjectPools.Add(typeof(T), tmpObjectPool);
            }

            return (IObjectPool<T>)tmpObjectPool;
        }

        public bool DestroyObjectPool<T>() where T : IPoolObject
        {
            return mObjectPools.Remove(typeof(T));
        }

        public void Release()
        {
            Dictionary<Type, ObjectPoolBase>.Enumerator tmpItor = mObjectPools.GetEnumerator();
            while (tmpItor.MoveNext())
            {
                ((ObjectPoolBase)tmpItor.Current.Value).Release();
            }

            mObjectPools.Clear();
        }
    }
}
