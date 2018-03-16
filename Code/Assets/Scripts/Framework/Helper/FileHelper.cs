using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace framework
{
    public static class FileHelper
    {
        public const string RES_VERSION_FILE = "version.bytes";
        public const string RES_DESCRIBE_FILE = "resources.bytes";

        private static uint msResVersionFileCrc = Crc.Crc32(RES_VERSION_FILE);
        public static uint ResVersionFileCRC
        {
            get { return msResVersionFileCrc; }
        }

        private static uint msResDescribeFileCrc = Crc.Crc32(RES_DESCRIBE_FILE);
        public static uint ResDescribeFileCRC
        {
            get { return msResDescribeFileCrc; }
        }

        public const string CODE_DLL_FILE = "main_impl.bytes";
        public const string CODE_VERSION_FILE = "code_version.bytes";

        private static uint msCodeVersionFileCrc = Crc.Crc32(CODE_VERSION_FILE);
        public static uint CodeVersionFileCrc
        {
            get { return msCodeVersionFileCrc; }
        }

        private static string msDataPath;
        private static string msPersistentPath = string.Empty;
        private static string msStreamingAssetsPath = string.Empty;

        public static string DataPath
        {
            get { return msDataPath; }
        }

        public static string StreamingAssetsPath
        {
            get { return msStreamingAssetsPath; }
        }

        public static string PersistentPath
        {
            get { return msPersistentPath; }
        }

        public static void Initalize()
        {
            msDataPath = Application.dataPath.Replace("\\", "/") + "/";
            msStreamingAssetsPath = Application.streamingAssetsPath + "/";

#if UNITY_EDITOR || UNITY_STANDALONE
            msPersistentPath = "D:/" + Application.productName + "/";
#else
            msPersistentPath = Application.persistentDataPath + "/" + Application.productName + "/";
#endif

            if (!Directory.Exists(msPersistentPath))
                Directory.CreateDirectory(msPersistentPath);
        }

        public static byte[] ReadFile(string filename)
        {
            FileStream fileStream = File.OpenRead(filename);
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, array.Length);
            fileStream.Close();
            fileStream.Dispose();
            return array;
        }

        public static void WriteFile(string filename, byte[] data)
        {
            FileStream fileStream = File.Open(filename, FileMode.Create, FileAccess.Write);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
            fileStream.Dispose();
        }
    }
}

