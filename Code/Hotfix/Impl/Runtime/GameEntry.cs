using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hotfix.framework;

namespace Hotfix.runtime
{
    public static class GameEntry
    {

        public static void Initialize()
        {
            Logger.Log("GameEntry.Initialize");
            Game.Init();
            /*
            GameModuleManager.CreateModule<FsmManager>();
            GameModuleManager.CreateModule<ProcedureManager>();
            IProcedureManager tmpProceMgr = GameModuleManager.GetModule<IProcedureManager>();
            tmpProceMgr.Initialize(GameModuleManager.GetModule<IFsmManager>(),
                new ProcedureCheckVersion(),
                new ProcedureLogin(),
                new ProcedureChangeScene(),
                new ProcedureMain());
            tmpProceMgr.StartProcedure<ProcedureCheckVersion>();

            GameModuleManager.CreateModule<ResourcesManager>();
            IResourcesManager tmpResMgr = GameModuleManager.GetModule<IResourcesManager>();
            tmpResMgr.LoadManifest();

            GameModuleManager.CreateModule<WindowsManager>();
            IWindowsManager tmpWindowsMgr = GameModuleManager.GetModule<IWindowsManager>();
            tmpResMgr.LoadBundleByType(EABType.UI, "UIRoot");
            GameObject tmpUIRoot = GameObject.Instantiate<GameObject>(tmpResMgr.GetAssetByType<GameObject>(EABType.UI, "UIRoot"));
            Transform tmpCanvasRoot = tmpUIRoot.transform.Find("CanvasRoot");
            tmpResMgr.LoadBundleByType(EABType.UI, "TestWnd");
            Transform tmpTestWnd = GameObject.Instantiate<GameObject>(tmpResMgr.GetAssetByType<GameObject>(EABType.UI, "TestWnd")).transform;
            tmpTestWnd.SetParent(tmpCanvasRoot, false);
            RectTransform tmpTestWndRT = tmpTestWnd as RectTransform;
            tmpTestWndRT.anchorMin = Vector2.zero;
            tmpTestWndRT.anchorMax = Vector2.one;
            //tmpTestWndRT.anchoredPosition3D = Vector3.zero;
            //tmpTestWndRT.sizeDelta = Vector2.zero;
            tmpTestWndRT.offsetMin = Vector2.zero;
            tmpTestWndRT.offsetMax = Vector2.zero;*/
        }

        public static void Update()
        {
            GameModuleManager.Update(Time.deltaTime, Time.deltaTime);
        }

        public static void LateUpdate()
        {
        }

        public static void FixedUpdate()
        {
        }
    }
}
