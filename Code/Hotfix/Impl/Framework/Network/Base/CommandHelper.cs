using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;
using framework;
using Hotfix.runtime;

namespace Hotfix.framework
{
    public sealed class CommandHelper
    {
        static DoubleMap<ushort, Type> sCommandOpcode2TypeDict = new DoubleMap<ushort, Type>();
        static Dictionary<Type, FieldInfo> sCommandType2FieldInfoDict = new Dictionary<Type, FieldInfo>();
        static Dictionary<ushort, List<MethodInfo>> sOpcode2MethodInfosDict = new Dictionary<ushort, List<MethodInfo>>();
        static object[] sParameterObjs = new object[1];

        public static void Init()
        {
            Type[] tmpTypes = HotFixHelper.GetHotfixTypes();

            Type tmpCommandAttributeType = typeof(CommandAttribute);
            Type tmpCommandHandlerAttributeType = typeof(CommandHandlerAttribute);
            Type tmpCommandHandleAttributeType = typeof(CommandHandleAttribute);

            foreach (Type typeElem in tmpTypes)
            {
                //协议信息
                var tmpCommandAttributes = typeElem.GetCustomAttributes(tmpCommandAttributeType, false);
                CommandAttribute tmpCommand = tmpCommandAttributes.Length > 0 ? tmpCommandAttributes[0] as CommandAttribute : null;

                if (null != tmpCommand)
                {
                    if (!sCommandOpcode2TypeDict.ContainsKey(tmpCommand.Opcode))
                        sCommandOpcode2TypeDict.Add(tmpCommand.Opcode, typeElem);

                    if (!sCommandType2FieldInfoDict.ContainsKey(typeElem))
                    {
                        FieldInfo tmpFieldInfo = typeElem.GetField("Data");
                        if (null == tmpFieldInfo)
                        {
                            Logger.LogError("消息中不包含data数据");
                        }
                        else
                        {
                            sCommandType2FieldInfoDict.Add(typeElem, tmpFieldInfo);
                        }
                    }
                }

                //协议观察者信息
                var tmpCommandHandlerAttris = typeElem.GetCustomAttributes(tmpCommandHandlerAttributeType, false);
                CommandHandlerAttribute tmpCommandHandler = null;

                if (tmpCommandHandlerAttris.Length > 0)
                    tmpCommandHandler = tmpCommandHandlerAttris[0] as CommandHandlerAttribute;

                if (null == tmpCommandHandler)
                    continue;
                
                MethodInfo[] tmpMethods = typeElem.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                foreach (MethodInfo methodInfoElem in tmpMethods)
                {
                    var tmpCommandHandleAttris = methodInfoElem.GetCustomAttributes(tmpCommandHandleAttributeType, false);
                    CommandHandleAttribute tmpCommandHandle = null;

                    if (tmpCommandHandleAttris.Length > 0)
                        tmpCommandHandle = tmpCommandHandleAttris[0] as CommandHandleAttribute;
                    
                    if (null == tmpCommandHandle)
                        continue;
                    //ILRT暂不支持GetParameters
                    //ParameterInfo[] tmpParameters = methodInfoElem.GetParameters();

                    //if (tmpParameters.Length > 0 && tmpParameters[0].ParameterType != tmpCommandHandle.CommandType)
                    //{
                    //    Logger.LogError("消息处理方法形参类型错误 " + typeElem + " -> " + methodInfoElem.Name);
                    //    continue;
                    //}

                    List<MethodInfo> tmpMethodInfoList = null;
                    if (!sOpcode2MethodInfosDict.TryGetValue(tmpCommandHandle.Opcode, out tmpMethodInfoList))
                    {
                        tmpMethodInfoList = new List<MethodInfo>();
                        sOpcode2MethodInfosDict.Add(tmpCommandHandle.Opcode, tmpMethodInfoList);
                    }

                    tmpMethodInfoList.Add(methodInfoElem);
                }
            }
        }

        public static object Process(BufferReader reader)
        {
            byte tmpFirst = 0, tmpSecond = 0;
            reader.Read(ref tmpFirst).Read(ref tmpSecond);
            ushort tmpOpcode = tmpFirst;
            tmpOpcode <<= 8;
            tmpOpcode |= tmpSecond;
            
            Type tmpType = sCommandOpcode2TypeDict.GetValueByKey(tmpOpcode);

            if (null == tmpType)
            {
                Logger.LogErrorFormat("找不到相应的协议包. first:{0} second:{1}", tmpFirst, tmpSecond);
                return null;
            }

            object tmpCommand = null;
            try
            {
                tmpCommand = Deserialize(reader, tmpType);
            }
            catch (Exception ex)
            {
                Logger.LogErrorFormat("协议反序列化异常. type:[0]  异常信息:{1}", tmpType, ex.Message);
            }

            if (null != tmpCommand)
            {
                List<MethodInfo> tmpMethodInfoList = null;

                if (sOpcode2MethodInfosDict.TryGetValue(tmpOpcode, out tmpMethodInfoList))
                {
                    sParameterObjs[0] = tmpCommand;

                    for (int i = 0, max = tmpMethodInfoList.Count; i < max; ++i)
                    {
                        MethodInfo tmpMethodInfo = tmpMethodInfoList[i];
                        tmpMethodInfoList[i].Invoke(null, sParameterObjs);
                    }
                }
            }

            return tmpCommand;
        }

        public static void Serialize(BufferWriter writer, object command)
        {
            Type tmpCommandType = command.GetType();
            ushort tmpOpcode = sCommandOpcode2TypeDict.GetKeyByValue(tmpCommandType);
            writer.Write((byte)(tmpOpcode >> 8));
            writer.Write((byte)(tmpOpcode & 0xff));

            FieldInfo tmpFieldInfo = null;
            if (!sCommandType2FieldInfoDict.TryGetValue(tmpCommandType, out tmpFieldInfo))
            {
                return;
            }

            IMessage tmpProtoMsg = tmpFieldInfo.GetValue(command) as IMessage;
            if (null == tmpProtoMsg)
                return;

            tmpProtoMsg.WriteTo(writer.stream);
        }

        public static object Deserialize(BufferReader reader, Type type)
        {
            FieldInfo tmpFieldInfo = null;
            if (!sCommandType2FieldInfoDict.TryGetValue(type, out tmpFieldInfo))
            {
                return null;
            }

            object tmpCommand = Activator.CreateInstance(type);
            IMessage tmpProtoMsg = tmpFieldInfo.GetValue(tmpCommand) as IMessage;

            if (null == tmpProtoMsg)
            {
                return null;
            }
            
            tmpProtoMsg.MergeFrom(reader.stream);
            return tmpCommand;
        }
    }
}
