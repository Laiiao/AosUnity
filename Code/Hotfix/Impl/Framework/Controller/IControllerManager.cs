using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix.framework
{
    public interface IControllerManager
    {

        T Get<T>() where T : ControllerBase;
    }
}
