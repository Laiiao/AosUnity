using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hotfix.framework
{
    public interface IWindowsManager
    {
        void SetWindowsRoot(Transform rootTrans);

        T FindWindow<T>() where T : WindowBase;

        T CreateWindow<T>() where T : WindowBase;

        T ShowWindow<T>(Action<WindowBase> showedHandle = null) where T : WindowBase;

        void DestroyWindow<T>() where T : WindowBase;
    }
}
