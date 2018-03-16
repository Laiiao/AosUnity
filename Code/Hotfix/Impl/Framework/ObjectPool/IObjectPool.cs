
namespace Hotfix.framework
{
    public interface IObjectPool<T> where T : IPoolObject
    {
        T Spawn();
        void Unspawn(T obj);
    }
}
