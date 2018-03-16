using System.Collections;
using System.Collections.Generic;
using Hotfix.framework;

namespace Hotfix.runtime
{
    [CommandHandler]
    public class TestController : ControllerBase
    {
        [CommandHandle(0x1, 0x1)]
        static void TestCommandHandle(TestCommand arg)
        {
            Game.ControllerMgr.Get<TestController>().TestHandle(arg);
        }

        void TestHandle(TestCommand arg)
        {
            Logger.Log("TestController.TestHandle. arg.Data.ScString = " + arg.Data.ScString);
            Logger.Log("TestController.TestHandle. arg.Data.ScFloat = " + arg.Data.ScFloat);
        }
    }
}
