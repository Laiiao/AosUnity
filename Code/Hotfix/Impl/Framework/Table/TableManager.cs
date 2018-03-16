using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Table
{
    public class TableManager<T> where T : Binary
    {
        private List<T> m_list = new List<T>();

        protected TableManager()
        {
        }

        public int Size
        {
            get { return m_list.Count; }
        }

        public T Get(int index)
        {
            return index < Size ? m_list[index] : default(T);
        }

        protected T FindInternal(Int64 key)
        {
            int index = TableUtility.BinarySearch(m_list, key);
            return index == -1 ? default(T) : Get(index);
        }

        protected bool Load(string path, string filename, uint version)
        {
            string fullname = path + "/" + filename;
            byte[] array = null;

            try
            {
                FileStream stream = new FileStream(fullname, FileMode.Open, FileAccess.Read);
                array = new byte[stream.Length];
                stream.Read(array, 0, (int)stream.Length);
                stream.Close();
                stream.Dispose();
            }
            catch (IOException e)
            {
                Logger.LogError(e.Message);
                return false;
            }

            return Load(array, version, filename);
        }

        public bool Load(byte[] array, uint version, string fileName)
        {
            Reader reader = new Reader(array, 0, array.Length);
            uint tblVersion = reader.ReadUInt32_();

            if (tblVersion != version)
            {
                //版本不一致
                Logger.LogError("配置表版本不一致 -> " + fileName);
                return false;
            }

            //行描述
            uint index = reader.ReadUInt32_();
            reader.stream.Position += index;

            m_list = reader.ReadRepeatedItem(m_list);

            return true;
        }
    }
}
