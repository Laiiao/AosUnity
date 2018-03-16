using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Hotfix.framework
{
    public abstract class WindowBase
    {
        //父节点
        protected Transform mParentNodeTrans;
        public Transform ParentNodeTrans
        {
            set { mParentNodeTrans = value; }
            get { return mParentNodeTrans; }
        }

        //自身
        protected GameObject mGameObejct;
        //是否显示
        protected bool mIsVisible;
        public bool IsVisible
        {
            get { return mIsVisible; }
        }
        //是否初始化
        protected bool mIsInited;
        //是否在加载完后显示
        protected bool mIsShowAfterLoaded;
        //显示后操作
        protected Action<WindowBase> mShowedHandle;
        //是否已销毁
        protected bool mIsDestroyed;
        public bool IsDestroyed
        {
            get { return mIsDestroyed; }
        }
        //父窗口
        protected WindowBase mParentWindow;
        //子窗口
        protected List<WindowBase> mChildWindows = new List<WindowBase>();
        //bundle
        public abstract string BundleName { get; }

        protected virtual void InitSubWindow() { }

        protected virtual void AfterInit() { }

        protected virtual void AfterShow() { }

        protected virtual void BeforeClose() { }

        protected virtual void Update(float deltaTime) { }

        protected virtual void UpdateOneSecond(float deltaTime) { }

        internal async Task LoadWindowAsync()
        {
            IResourcesManager tmpResMgr = GameModuleManager.GetModule<IResourcesManager>();
            await tmpResMgr.LoadBundleByTypeAsync(EABType.UI, BundleName);
            
            if (mIsDestroyed)
            {
                tmpResMgr.UnLoadBundleByType(EABType.UI, BundleName);
                return;
            }

            InitWindow(tmpResMgr.GetAssetByType<GameObject>(EABType.UI, BundleName));

            if (mIsShowAfterLoaded)
                Show();
        }

        internal void InitWindow(GameObject uiObj)
        {
            if (null == uiObj)
            {
                return;
            }

            GameObject tmpGo = Hotfix.Instantiate<GameObject>(uiObj);
            tmpGo.transform.SetParent(mParentNodeTrans, false);
            RectTransform tmpTestWndRT = tmpGo.transform as RectTransform;
            if (null != tmpTestWndRT)
            {
                tmpTestWndRT.anchorMin = Vector2.zero;
                tmpTestWndRT.anchorMax = Vector2.one;
                tmpTestWndRT.offsetMin = Vector2.zero;
                tmpTestWndRT.offsetMax = Vector2.zero;
            }
            
            mGameObejct = tmpGo;

            InitSubWindow();
            AfterInit();
            SetVisible(mIsShowAfterLoaded);
            mIsInited = true;
        }

        internal void InternalUpdate(float deltaTime)
        {
            Update(deltaTime);

            for (int i = 0, max = mChildWindows.Count; i < max; ++i)
            {
                WindowBase tmpChildWindow = mChildWindows[i];

                if (tmpChildWindow.IsVisible && !tmpChildWindow.IsDestroyed)
                {
                    tmpChildWindow.InternalUpdate(deltaTime);
                }
            }
        }

        internal void InternalUpdateOneSecond(float deltaTime)
        {
            UpdateOneSecond(deltaTime);

            for (int i = 0, max = mChildWindows.Count; i < max; ++i)
            {
                WindowBase tmpChildWindow = mChildWindows[i];

                if (tmpChildWindow.IsVisible && !tmpChildWindow.IsDestroyed)
                {
                    tmpChildWindow.InternalUpdateOneSecond(deltaTime);
                }
            }
        }

        private void SetVisible(bool visible)
        {
            mIsVisible = visible;

            if (mGameObejct)
            {
                mGameObejct.SetActive(visible);
            }
        }

        public void Show(Action<WindowBase> showedHandle = null)
        {
            if (!mIsInited)
            {
                if (null != showedHandle)
                {
                    mShowedHandle += showedHandle;
                }

                mIsShowAfterLoaded = true;
                LoadWindowAsync();
                return;
            }

            SetVisible(true);

            AfterShow();

            if (null != mShowedHandle)
            {
                mShowedHandle(this);
                mShowedHandle = null;
            }
        }

        public void Close()
        {
            mShowedHandle = null;

            if (!mIsInited)
            {
                mIsShowAfterLoaded = false;
                return;
            }

            BeforeClose();

            SetVisible(false);
        }

        public void Destroy()
        {
            mIsInited = false;
            mIsDestroyed = true;

            if (mGameObejct)
            {
                GameObject.Destroy(mGameObejct);
                mGameObejct = null;
                IResourcesManager tmpResMgr = GameModuleManager.GetModule<IResourcesManager>();
                tmpResMgr.UnLoadBundleByType(EABType.UI, BundleName);
            }
        }

        public T GetChildWindow<T>() where T : WindowBase
        {
            for (int i = 0, max = mChildWindows.Count; i < max; ++i)
            {
                WindowBase tmpChildWindow = mChildWindows[i];

                if (null != tmpChildWindow && tmpChildWindow.GetType() == typeof(T))
                    return tmpChildWindow as T;
            }

            return null;
        }

        protected T CreateChildWindow<T>(Transform parentNode, GameObject childGo = null) where T : WindowBase
        {
            WindowBase tmpChildWindow = GetChildWindow<T>();

            if (null == tmpChildWindow)
            {
                tmpChildWindow = Activator.CreateInstance<T>();
                tmpChildWindow.mParentWindow = this;
                tmpChildWindow.mParentNodeTrans = parentNode;
                tmpChildWindow.InitWindow(childGo);
                mChildWindows.Add(tmpChildWindow);
            }

            return tmpChildWindow as T;
        }
    }
}
