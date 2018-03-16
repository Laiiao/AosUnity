using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace framework
{
    public sealed class CodeUpdater
    {
        public static CodeUpdater ms_instance = new CodeUpdater();

        private bool m_error;
        private string m_path = string.Empty;

        VersionData mServerVersionData;
        VersionData mLocalVersionData;
        VersionData mApkVersionData;
        byte[] mServerVersionBytes = null;

        Action m_updateHandle;//需要更新的补丁的回调
        Action<byte[]> m_finishHandle;//代码补丁更新完成的回调
        Action<byte[]> m_skipHandle;
        Action m_failHandle;//下载失败

        public static CodeUpdater instance
        {
            get { return ms_instance; }
        }

        public bool error
        {
            get { return this.m_error; }
        }

        public void Initialize(string path)
        {
            this.m_path = path.Trim();
        }

        public void Begin(Action updateHandle, Action<byte[]> finishHandle, Action failHandle, Action<byte[]> skipHandle)
        {
            m_updateHandle = updateHandle;
            m_finishHandle = finishHandle;
            m_skipHandle = skipHandle;
            m_failHandle = failHandle;

            //加载包内VersionData
            TextAsset apkTextAsset = Resources.Load<TextAsset>(FileHelper.CodeVersionFileCrc.ToString());
            if (apkTextAsset != null)
                mApkVersionData = VersionData.LoadVersionData(apkTextAsset.bytes);

            //加载本地VersionData
            string localVersionPath = FileHelper.PersistentPath + FileHelper.CodeVersionFileCrc;
            if (File.Exists(localVersionPath))
            {
                byte[] localData = FileHelper.ReadFile(localVersionPath);
                mLocalVersionData = VersionData.LoadVersionData(localData);
            }

            //下载服务端CodeVersion.bytes
            Httper.instance.Apply(m_path + FileHelper.CodeVersionFileCrc + "?time=" + DateTime.Now.Ticks, OnLoadVersion);
        }

        //下载服务端Version文件回调
        void OnLoadVersion(byte[] data, object param, bool error)
        {
            //下载Version出错
            if (error)
            {
                Debug.LogError("下载cdn的代码Version:" + FileHelper.CodeVersionFileCrc + "文件出错");
                if (m_failHandle != null)
                    m_failHandle();
            }
            else
            {
                mServerVersionBytes = data;
                mServerVersionData = VersionData.LoadVersionData(data);
                if (!mServerVersionData.initalize)
                {
                    Debug.LogError("加载cdn的Version:" + FileHelper.CodeVersionFileCrc + "失败");
                    if (m_failHandle != null)
                        m_failHandle();
                    return;
                }

                //需要强更客户端
                if (mApkVersionData.initalize && mApkVersionData.major != mServerVersionData.major)
                {
                    ForceUpdateClient();
                    return;
                }

                bool needUpdate = false;

                if (!mApkVersionData.initalize)
                {
                    Debug.LogError("包内VersionData资源不应该为空");
                    return;
                }

                //如果包外资源为空，只检测包内的VersionData
                if (!mLocalVersionData.initalize)
                {
                    if (mApkVersionData.fileListRCR != mServerVersionData.fileListRCR)
                        needUpdate = true;
                }
                else
                {
                    if (mLocalVersionData.fileListRCR != mServerVersionData.fileListRCR)
                        needUpdate = true;
                }

                //检测包外FileList的缺失
                if (mLocalVersionData.initalize && mLocalVersionData.fileListRCR == mServerVersionData.fileListRCR)
                {
                    string fileListPath = FileHelper.PersistentPath + mServerVersionData.fileListRCR;
                    if (!File.Exists(fileListPath))
                        needUpdate = true;
                }

                //需要更新
                if (needUpdate)
                {
                    if (null != m_updateHandle)
                        m_updateHandle();

                    Httper.instance.Apply(m_path + mServerVersionData.fileListRCR, OnLoadDescribe, null);
                }
                //不需要更新
                else
                {
                    string fileListPath = FileHelper.PersistentPath + mServerVersionData.fileListRCR;
                    byte[] dllData = null;
                    //用包外dll文件
                    if (mLocalVersionData.fileListRCR == mServerVersionData.fileListRCR)
                    {
                        dllData = FileHelper.ReadFile(fileListPath);
                        if (m_skipHandle != null)
                            m_skipHandle(dllData);
                    }
                    //用包内dll文件
                    else if (mApkVersionData.fileListRCR == mServerVersionData.fileListRCR)
                    {
                        TextAsset textAsset = Resources.Load(mServerVersionData.fileListRCR.ToString()) as TextAsset;
                        if (textAsset != null)
                        {
                            dllData = textAsset.bytes;
                            if (m_skipHandle != null)
                                m_skipHandle(dllData);
                        }
                    }
                    else
                    {
                        if (m_failHandle != null)
                            m_failHandle();
                        Debug.LogError("更新流程出错，请检测代码");
                        return;
                    }
                }
            }
        }

        //下载服务端描述文件回调
        void OnLoadDescribe(byte[] data, object param, bool error)
        {
            if (error)
            {
                Debug.LogError("下载服务端" + FileHelper.CODE_DLL_FILE + "文件出错");
                if (m_failHandle != null)
                    m_failHandle();
            }
            else
            {
                //保存最新Dll的文件描述
                Save(data, mServerVersionData.fileListRCR.ToString());
                Save(mServerVersionBytes, FileHelper.CodeVersionFileCrc.ToString());
                if (m_finishHandle != null)
                    m_finishHandle(data);
            }
        }


        void Save(byte[] data, string fileName)
        {
            if (data == null)
                return;
            string path = FileHelper.PersistentPath + fileName;
            string newPath = path + ".tmp";
            FileHelper.WriteFile(newPath, data);
            if (File.Exists(path))
                File.Delete(path);
            File.Move(newPath, path);
        }

        public void Reset()
        {
            m_error = false;
            m_finishHandle = null;
            m_skipHandle = null;
            m_updateHandle = null;
            m_failHandle = null;
        }

        public static void ForceUpdateClient()
        {
            if (GameSetup.instance.clientUrl.Length > 0)
            {
                string str = GameSetup.instance.clientUrl[GameSetup.instance.clientUrl.Length - 1].ToString();
                if (str != "/")
                    GameSetup.instance.clientUrl += "/";
            }

            /*DownLoadMessageBoxWnd.ShowUpdateClientMessageWnd(DownloadWnd.instance.root.transform, delegate()
            {

                if (!string.IsNullOrEmpty(GameSetup.instance.clientUrl))
                {
                    string DownLoadUrl = string.Empty;
                    int channelType = 0;
                    if (GameSetup.instance.platform)
                    {
                        channelType = aossdk.AosSDK.getInstance().channelType();
                    }
                    else
                    {
                        channelType = int.Parse(GameSetup.instance.unionID);
                    }


                    DownLoadUrl = GameSetup.instance.clientUrl + channelType;

                    DownLoadUrl += "?time=" + DateTime.Now.Ticks;
                    DownLoadUrl = DownLoadUrl.Replace("\\", "/");
                    Logger.Log("DownLoadUrl【" + DownLoadUrl + "】");

                    Httper.instance.Apply(DownLoadUrl, delegate(byte[] data, object param, bool error)
                    {
                        if (error)
                        {
                            Logger.LogError("下载文件【" + DownLoadUrl + "】失败");
                        }
                        else
                        {
                            string clientUrl = System.Text.Encoding.UTF8.GetString(data);
                            Logger.Log("ClientUrl【" + clientUrl + "】");
                            if (!string.IsNullOrEmpty(clientUrl))
                                Application.OpenURL(clientUrl);
                        }
                    });
                }
                else
                {
                    Application.Quit();
                }

            });*/
        }
    }
}
