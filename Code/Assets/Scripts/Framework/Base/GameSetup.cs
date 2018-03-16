using UnityEngine;
using System.Collections;
using System.Xml;

public class GameSetup
{
    private static GameSetup msInstance = new GameSetup();

    public static GameSetup instance
    {
        get { return GameSetup.msInstance; }
    }
    
    bool mPublish = false;//是否发布
    bool mPlatform = false;//是否使用平台
    bool mUpdate;//是否更新资源
    bool mCodeUpdate = false;//是否更新代码
    string mPlatformIp = "192.168.3.6";//平台IP
    int mPlatformPort = 8183;//平台端口号
    string mUnionID = "1";
    string mUpdateUrl = "file://D:/Test";//资源更新地址
    string mCodeUrl = string.Empty;//代码更新地址
    string mClientUrl = string.Empty;//客户端地址
    string mCrashUrl = string.Empty;//崩溃信息地址
    string mExceptUrl = string.Empty;//异常信息地址

    #region Property

    public bool publish
    {
        get { return mPublish; }
        set { mPublish = value; }
    }

    public bool platform
    {
        get { return mPlatform; }
        set { mPlatform = value; }
    }

    public bool update
    {
        get { return mUpdate; }
        set { mUpdate = value; }
    }

    public bool codeUpdate
    {
        get { return mCodeUpdate; }
        set { mCodeUpdate = value; }
    }

    public string platformIp
    {
        get { return mPlatformIp; }
        set { mPlatformIp = value; }
    }

    public int platformPort
    {
        get { return mPlatformPort; }
        set { mPlatformPort = value; }
    }

    public string unionID
    {
        get { return mUnionID; }
        set { mUnionID = value; }
    }

    public string updateUrl
    {
        get { return mUpdateUrl; }
        set { mUpdateUrl = value; }
    }

    public string codeUrl
    {
        get { return mCodeUrl; }
        set { mCodeUrl = value; }
    }

    public string clientUrl
    {
        get { return mClientUrl; }
        set { mClientUrl = value; }
    }

    public string crashUrl
    {
        get { return mCrashUrl; }
        set { mCrashUrl = value; }
    }

    public string exceptUrl
    {
        get { return mExceptUrl; }
        set { mExceptUrl = value; }
    }

    #endregion

    public bool Load()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("setting", typeof(TextAsset));
        if (textAsset == null)
            return false;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        XmlNode rootNode = doc.SelectSingleNode("Setup");
        mPublish = rootNode.SelectSingleNode("publish").InnerText == "1";
        mPlatform = rootNode.SelectSingleNode("platform").InnerText == "1";
        mUpdate = rootNode.SelectSingleNode("update").InnerText == "1";
        mCodeUpdate = rootNode.SelectSingleNode("codeUpdate").InnerText == "1";
        mPlatformIp = rootNode.SelectSingleNode("platformIp").InnerText.Trim();
        mPlatformPort = int.Parse(rootNode.SelectSingleNode("platformPort").InnerText);
        mUnionID = rootNode.SelectSingleNode("unionID").InnerText;
        mUpdateUrl = rootNode.SelectSingleNode("updateUrl").InnerText.Trim();
        mCodeUrl = rootNode.SelectSingleNode("codeUrl").InnerText.Trim();
        mClientUrl = rootNode.SelectSingleNode("clientUrl").InnerText.Trim();
        mCrashUrl = rootNode.SelectSingleNode("crashUrl").InnerText.Trim();
        mExceptUrl = rootNode.SelectSingleNode("exceptUrl").InnerText.Trim();
        return true;
    }
}
