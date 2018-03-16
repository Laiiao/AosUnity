using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hotfix.framework;

namespace Hotfix.runtime
{
    public static class Game
    {
        public static IEventManager EventMgr { get; set; }
        public static INetworkManager NetworkMgr { get; set; }
        public static IResourcesManager ResourcesMgr { get; set; }
        public static IFsmManager FsmMgr { get; set; }
        public static IProcedureManager ProcedureMgr { get; set; }
        public static IControllerManager ControllerMgr { get; set; }
        public static IWindowsManager WindowsMgr { get; set; }

        public static void Init()
        {
            InitGameModule();
            
            //依赖
            ResourcesMgr.LoadManifest();

            //UI
            ResourcesMgr.LoadBundleByType(EABType.UI, "UIRoot");
            GameObject tmpUIRoot = GameObject.Instantiate<GameObject>(ResourcesMgr.GetAssetByType<GameObject>(EABType.UI, "UIRoot"));
            GameObject.DontDestroyOnLoad(tmpUIRoot);
            Transform tmpCanvasRoot = tmpUIRoot.transform.Find("CanvasRoot");
            UIHelper.UIRootTrans = tmpUIRoot.transform;
            UIHelper.CanvasRootTrans = tmpCanvasRoot;
            WindowsMgr.SetWindowsRoot(tmpCanvasRoot);
            
            //流程
            ProcedureMgr.Initialize(FsmMgr);
            ProcedureMgr.AddProcedure<ProcedureCheckVersion>();
            ProcedureMgr.AddProcedure<ProcedureLogin>();
            ProcedureMgr.AddProcedure<ProcedureChangeScene>();
            ProcedureMgr.AddProcedure<ProcedureMain>();
            ProcedureMgr.StartProcedure<ProcedureCheckVersion>();
        }

        public static void InitGameModule()
        {
            GameModuleManager.CreateModule<EventManager>();
            EventMgr = GameModuleManager.GetModule<IEventManager>();
            GameModuleManager.CreateModule<NetworkManager>();
            NetworkMgr = GameModuleManager.GetModule<INetworkManager>();
            GameModuleManager.CreateModule<ResourcesManager>();
            ResourcesMgr = GameModuleManager.GetModule<IResourcesManager>();
            GameModuleManager.CreateModule<FsmManager>();
            FsmMgr = GameModuleManager.GetModule<IFsmManager>();
            GameModuleManager.CreateModule<ProcedureManager>();
            ProcedureMgr = GameModuleManager.GetModule<IProcedureManager>();
            GameModuleManager.CreateModule<ControllerManager>();
            ControllerMgr = GameModuleManager.GetModule<IControllerManager>();
            GameModuleManager.CreateModule<WindowsManager>();
            WindowsMgr = GameModuleManager.GetModule<IWindowsManager>();
        }
    }
}
