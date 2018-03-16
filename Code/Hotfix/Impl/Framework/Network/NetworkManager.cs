using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix.framework
{
    internal sealed class NetworkManager : GameModuleBase, INetworkManager
    {
        IEventManager mEventManager;
        Service mTcpService;

        public NetworkManager()
        {
            CommandHelper.Init();
            mTcpService = new TcpService();
            mTcpService.ConnectSucceedHandle = OnConnectSucceedHandle;
            mTcpService.ConnectFailedHandle = OnConnectFailHandle;
            mTcpService.DisConnectedHandle = OnDisConnectHandle;

            GameModuleManager.CreateModule<EventManager>();
            mEventManager = GameModuleManager.GetModule<IEventManager>();
        }

        internal override int Priority
        {
            get
            {
                return 200;
            }
        }
        

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            mTcpService.Process();
        }

        internal override void Shutdown()
        {
            mTcpService.Close();
        }

        public bool ConnectServer(int customSrvID, string ip, int port)
        {
            return mTcpService.ConnectServer(customSrvID, ip, port);
        }

        public bool Send(int customSrvID, object cmd)
        {
            return mTcpService.Send(customSrvID, cmd);
        }

        private void OnConnectSucceedHandle(int customSrvID)
        {
            var tmpArg = ReferencePool.Fetch<ServiceConnectSucceedEventArgs>();
            tmpArg.CustomServerID = customSrvID;
            mEventManager.FireNow(this, tmpArg);
        }

        private void OnConnectFailHandle(int customSrvID)
        {
            var tmpArg = ReferencePool.Fetch<ServiceConnectFailedEventArgs>();
            tmpArg.CustomServerID = customSrvID;
            mEventManager.FireNow(this, tmpArg);
        }

        private void OnDisConnectHandle(int customSrvID)
        {
            var tmpArg = ReferencePool.Fetch<ServiceDisConnectEventArgs>();
            tmpArg.CustomServerID = customSrvID;
            mEventManager.FireNow(this, tmpArg);
        }
    }
}
