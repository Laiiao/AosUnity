using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hotfix.framework;
using ProcedureOwner = Hotfix.framework.IFsm<Hotfix.framework.IProcedureManager>;
using Google.Protobuf;

namespace Hotfix.runtime
{
    public class ProcedureLogin : ProcedureBase
    {
        protected internal override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Game.WindowsMgr.ShowWindow<TestWnd>(delegate (WindowBase window)
            {
                Logger.Log("TestWnd Showed!");
            });

            BufferWriter tmpBuffWriter = new BufferWriter();
            TestCommand tmpCmd = new TestCommand();
            tmpCmd.Data.ScInt32 = 100001;
            tmpCmd.Data.ScBool = true;
            tmpCmd.Data.ScFloat = 0.01f;
            tmpCmd.Data.ScString = "helloworld";
            tmpCmd.Data.RepInt32.Add(5);
            tmpCmd.Data.RepInt32.Add(4);
            tmpCmd.Data.RepInt32.Add(1);
            CommandHelper.Serialize(tmpBuffWriter, tmpCmd);

            BufferReader tmpBuffReader = new BufferReader();
            tmpBuffReader.Load(tmpBuffWriter.stream.GetBuffer(), 0, (int)tmpBuffWriter.stream.Position);
            CommandHelper.Process(tmpBuffReader);

            ItemBaseManager.instance.Load("../RunTimeResources/table");
            ItemBase tmpItemBase = ItemBaseManager.instance.Get(0);
            Logger.Log("配置表测试: " + tmpItemBase.useRemark + "->" +tmpItemBase.name);
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected internal override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}
