using System.Collections;
using System.Collections.Generic;

namespace Hotfix.framework
{
    public interface IPoolObject
    {
        void OnInit();
        void OnSpawn();
        void OnUnspawn();
        void OnRelease();
    }
}
