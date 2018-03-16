using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hotfix.framework
{
    internal sealed class WindowsManager : GameModuleBase, IWindowsManager
    {
        Transform mWindowRootTrans;
        List<WindowBase> mWindowsList = new List<WindowBase>();

        internal override int Priority
        {
            get
            {
                return 0;
            }
        }

        private float mDeltaTime = 0;

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            mDeltaTime += elapseSeconds;
            bool tmpIsSecondTime = mDeltaTime >= 1f;

            for (int i = 0, max = mWindowsList.Count; i < max; ++i)
            {
                WindowBase tmpWindow = mWindowsList[i];

                if (tmpWindow.IsVisible && !tmpWindow.IsDestroyed)
                {
                    tmpWindow.InternalUpdate(elapseSeconds);

                    if (tmpIsSecondTime)
                    {
                        tmpWindow.InternalUpdateOneSecond(mDeltaTime);
                    }
                }
            }

            if (tmpIsSecondTime)
                mDeltaTime = 0f;
        }

        internal override void Shutdown()
        {
            for (int i = 0, max = mWindowsList.Count; i < max; ++i)
            {
                WindowBase tmpWindow = mWindowsList[i];
                tmpWindow.Destroy();
            }

            mWindowsList.Clear();
        }

        public void SetWindowsRoot(Transform rootTrans)
        {
            mWindowRootTrans = rootTrans;
        }

        public T FindWindow<T>() where T : WindowBase
        {
            for (int i = 0, max = mWindowsList.Count; i < max; ++i)
            {
                WindowBase tmpWindow = mWindowsList[i];

                if (tmpWindow.GetType() == typeof(T))
                    return tmpWindow as T;
            }

            return default(T);
        }

        public T CreateWindow<T>() where T : WindowBase
        {
            T tmpWindow = FindWindow<T>();

            if (null == tmpWindow)
            {
                tmpWindow = Activator.CreateInstance<T>();
                tmpWindow.ParentNodeTrans = mWindowRootTrans;
                mWindowsList.Add(tmpWindow);
            }

            return tmpWindow;
        }

        public T ShowWindow<T>(Action<WindowBase> showedHandle = null) where T : WindowBase
        {
            T tmpWindow = CreateWindow<T>();
            tmpWindow.Show(showedHandle);

            return tmpWindow;
        }

        public void DestroyWindow<T>() where T : WindowBase
        {
            T tmpWindow = FindWindow<T>();

            if (null != tmpWindow)
            {
                tmpWindow.Destroy();
            }
        }
    }
}
