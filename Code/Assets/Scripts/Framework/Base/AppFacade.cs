using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace framework
{
    public class AppFacade
    {

        const string ENTRY_CLASS_NAME = "Hotfix.runtime.GameEntry" ;
        const string INIT_METHOD_NAME = "Initialize";
        const string UPDATE_METHOD_NAME = "Update";
        const string LATE_UPDATE_METHOD_NAME = "LateUpdate";
        const string FIXED_UPDATE_METHOD_NAME = "FixedUpdate";

        private static AppFacade msInstance;
        public static AppFacade Instance
        {
            get
            {
                if (null == msInstance)
                {
                    msInstance = new AppFacade();
                }

                return msInstance;
            }
        }
        
        private IStaticMethod mUpdateMethod;
        private IStaticMethod mLateUpdateMethod;
        private IStaticMethod mFixedUpdateMethod;
        
        public void StartUp()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            CheckCodeUpdate();
        }

        public void OnUpdate()
        {
            Httper.instance.Loop();

            if (null != mUpdateMethod)
                mUpdateMethod.Run();
        }

        public void OnLateUpdate()
        {
            if (null != mLateUpdateMethod)
                mLateUpdateMethod.Run();
        }

        public void OnFixedUpdate()
        {
            if (null != mFixedUpdateMethod)
                mFixedUpdateMethod.Run();
        }

        public void Close()
        {
        }


        #region codeupdate

        void CheckCodeUpdate()
        {

#if TEST_HOTFIX
            LoadHotFixAssembly(FileHelper.ReadFile(Application.streamingAssetsPath + "/Hotfix.dll"));
#else
            //检测更新代码
            if (GameSetup.instance.codeUpdate)
            {
                CodeUpdater.instance.Begin(BeginUpdateCode, FinishUpdateCode, FailUpdateCode, SkipUpdateCode);
            }
            else
            {
                LoadHotFixAssembly(null);
            }
#endif

        }

        //需要更新代码的回调
        void BeginUpdateCode()
        {

        }

        //更新代码失败回调
        void FailUpdateCode()
        {
            CodeUpdater.instance.Reset();
            CodeUpdater.instance.Begin(BeginUpdateCode, FinishUpdateCode, FailUpdateCode, SkipUpdateCode);
        }

        //完成更新代码回调
        void FinishUpdateCode(byte[] data)
        {
            LoadHotFixAssembly(data);
        }

        //代码已是最新，不需要更新的回调
        void SkipUpdateCode(byte[] data)
        {
            LoadHotFixAssembly(data);
        }

        void LoadHotFixAssembly(byte[] data)
        {
            HotFixHelper.LoadHotfixAssembly(data);

            IStaticMethod tmpInitMethod = null;
#if ILRuntime
            tmpInitMethod = new ILStaticMethod(HotFixHelper.Appdomain, ENTRY_CLASS_NAME, INIT_METHOD_NAME, 0);
            mUpdateMethod = new ILStaticMethod(HotFixHelper.Appdomain, ENTRY_CLASS_NAME, UPDATE_METHOD_NAME, 0);
            mLateUpdateMethod = new ILStaticMethod(HotFixHelper.Appdomain, ENTRY_CLASS_NAME, LATE_UPDATE_METHOD_NAME, 0);
            mFixedUpdateMethod = new ILStaticMethod(HotFixHelper.Appdomain, ENTRY_CLASS_NAME, FIXED_UPDATE_METHOD_NAME, 0);
#else
            Type tmpType = HotFixHelper.HotFixAssembly.GetType(ENTRY_CLASS_NAME);
            tmpInitMethod = new MonoStaticMethod(tmpType, INIT_METHOD_NAME);
            mUpdateMethod = new MonoStaticMethod(tmpType, UPDATE_METHOD_NAME);
            mLateUpdateMethod = new MonoStaticMethod(tmpType, LATE_UPDATE_METHOD_NAME);
            mFixedUpdateMethod = new MonoStaticMethod(tmpType, FIXED_UPDATE_METHOD_NAME);
#endif
            tmpInitMethod.Run();
        }


#endregion
    }
}
