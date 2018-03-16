using System.Threading.Tasks;

namespace Hotfix.framework
{
    public interface IResourcesManager
    {
        void LoadManifest();

        K GetAssetByType<K>(EABType abType, string bundleName) where K : class;

        void UnLoadBundleByType(EABType type, string abName);

        void LoadBundleByType(EABType type, string abName);

        Task LoadBundleByTypeAsync(EABType abType, string abName);
    }
}
