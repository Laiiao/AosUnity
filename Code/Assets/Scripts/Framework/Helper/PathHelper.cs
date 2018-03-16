
using System;
using System.Text;
using UnityEngine;

namespace framework
{
    public static class PathHelper
    {     /// <summary>
          ///应用程序外部资源路径存放路径(热更新资源路径)
          /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                string path = $"{Application.persistentDataPath}/{Application.productName}/";
                
                return path;
            }

        }

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                string path = string.Empty;

                if (GameSetup.instance.publish)
                {
                    path = Application.streamingAssetsPath;
                }
                else
                {
                    switch (Application.platform)
                    {
                        case RuntimePlatform.Android:
                            path = "../RunTimeResources/Android/StreamingAssets/";
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            path = "../RunTimeResources/IOS/StreamingAssets/";
                            break;
                        default:
                            path = "../RunTimeResources/PC/StreamingAssets/";
                            break;
                    }
                }
                
                return path;
            }
        }
    }
}
