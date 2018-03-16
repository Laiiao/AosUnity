using System;
using framework;

namespace Hotfix.framework
{
    public class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : IPoolObject
    {
        private readonly EQueue<T> mQueue = new EQueue<T>();

        public T Spawn()
        {
            T tmpPoolObj;

            if (mQueue.Count > 0)
            {
                tmpPoolObj = mQueue.Dequeue();
            }
            else
            {
                tmpPoolObj = Activator.CreateInstance<T>();
                tmpPoolObj.OnInit();
            }

            tmpPoolObj.OnSpawn();

            return tmpPoolObj;
        }

        public void Unspawn(T obj)
        {
            if (null == obj)
                return;

            obj.OnUnspawn();
            mQueue.Enqueue(obj);
        }

        public override void Release()
        {
            while (mQueue.Count > 0)
            {
                T tmpPoolObj = mQueue.Dequeue();
                tmpPoolObj.OnRelease();
            }
        }
    }
}
