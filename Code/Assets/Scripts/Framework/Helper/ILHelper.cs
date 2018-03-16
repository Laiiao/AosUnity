using System;
using System.Reflection;
using ILRuntime.Runtime.Enviorment;
using UnityEngine;
using System.Collections.Generic;

namespace framework
{
	public static class ILHelper
	{
		public static unsafe void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
            // 注册重定向函数

            // 注册委托
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<String, Int32>, Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<String, Int32>, String>();            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32>();            appdomain.DelegateManager.RegisterFunctionDelegate<Adapt_IMessage.Adaptor>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Threading.Tasks.Task>();

            // 注册适配器
            appdomain.RegisterCrossBindingAdaptor(new Adapt_IMessage());
            appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineClassInheritanceAdaptor());
        }
    }
}