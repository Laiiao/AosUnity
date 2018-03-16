using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hotfix.framework;
using System.Reflection;

namespace Hotfix.runtime
{
    public class TestWnd : WindowBase
    {
        public override string BundleName { get { return "TestWnd"; } }

        protected override void AfterInit()
        {
            Logger.Log("TestWnd.AfterInit");
        }

        protected override void AfterShow()
        {
            Logger.Log("TestWnd.AfterShow");
        }

        protected override void BeforeClose()
        {
            Logger.Log("TestWnd.BeforeClose");
            
        }

        protected override void Update(float deltaTime)
        {
        }

        protected override void UpdateOneSecond(float deltaTime)
        {
        }
    }
}
