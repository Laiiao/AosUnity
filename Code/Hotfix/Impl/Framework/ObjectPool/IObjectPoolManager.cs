
namespace Hotfix.framework
{
    public interface IObjectPoolManager
    {
        IObjectPool<T> GetObjectPool<T>() where T : IPoolObject;

        bool DestroyObjectPool<T>() where T : IPoolObject;

        void Release();
    }
}
