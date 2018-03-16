using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace framework
{

    public class Httper
    {
        public class Loader
        {
            public delegate void Handle(byte[] data, object param, bool error);
            private const int m_max_count = 5;
            private WWW m_www;
            private WWWForm m_form;
            private object m_param;
            private Loader.Handle m_handle;
            private string m_url = string.Empty;
            private int m_count;
            public bool done
            {
                get
                {
                    return m_www != null && m_www.isDone;
                }
            }
            public Loader(string url, object param, Loader.Handle handle)
            {
                m_url = url;
                m_param = param;
                m_handle = handle;
            }
            public Loader(string url, WWWForm form, object param, Loader.Handle handle)
            {
                m_url = url;
                m_form = form;
                m_param = param;
                m_handle = handle;
            }
            public bool OnLoaded()
            {
                m_count++;
                if (ms_reqDic.ContainsKey(m_url))
                {
                    ms_reqDic.Remove(m_url);
                }
                if (m_handle != null)
                {
                    bool error = false;
                    byte[] data;
                    if (!string.IsNullOrEmpty(m_www.error))
                    {
                        data = null;
                        error = true;
                        if (m_count < 5)
                        {
                            m_www.Dispose();
                            return false;
                        }
                        if (!ms_errorDic.ContainsKey(m_url))
                        {
                            ms_errorDic.Add(m_url, m_url);
                        }
                        Debug.LogFormat("============== down load fail WWW: {0}", m_url);
                    }
                    else
                    {
                        data = m_www.bytes;
                    }
                    m_handle(data, m_param, error);
                    m_handle = null;
                }
                m_www.Dispose();
                return true;
            }

            public IEnumerator Request()
            {
                if (m_form != null)
                    m_www = new WWW(m_url, m_form);
                else
                    m_www = new WWW(m_url);

                yield return m_www;
            }

            public void Close()
            {
                if (m_www != null)
                    m_www.Dispose();
            }

        }
        private static Httper ms_instance = new Httper();
        private MonoBehaviour m_mono;
        private int m_count = 5;
        private List<Loader> m_apply = new List<Loader>();
        private List<Loader> m_request = new List<Loader>();
        private List<Loader> m_list = new List<Loader>();

        public static Dictionary<string, string> ms_reqDic = new Dictionary<string, string>();
        public static Dictionary<string, string> ms_errorDic = new Dictionary<string, string>();

        public static Httper instance
        {
            get
            {
                return ms_instance;
            }
        }

        public int maxCount
        {
            get
            {
                return m_count;
            }
            set
            {
                m_count = value;
            }
        }

        public MonoBehaviour mono
        {
            get
            {
                return m_mono;
            }
        }

        public void Start(MonoBehaviour mono)
        {
            m_mono = mono;
        }

        public void Apply(string url, Loader.Handle handle)
        {
            Apply(url, handle, null);
        }

        public void Apply(string url, Loader.Handle handle, object param)
        {
            Loader loader = new Loader(url, param, handle);
            m_request.Add(loader);
        }

        public void Apply(string url, WWWForm form, Loader.Handle handle)
        {
            Apply(url, form, handle, null);
        }

        public void Apply(string url, WWWForm form, Loader.Handle handle, object param)
        {
            Loader loader = new Loader(url, form, param, handle);
            m_request.Add(loader);
        }

        public void Clear()
        {
            int i = 0;
            int count = m_apply.Count;
            while (i < count)
            {
                m_apply[i].Close();
                i++;
            }
            m_apply.Clear();
            int j = 0;
            int count2 = m_request.Count;
            while (j < count2)
            {
                m_request[j].Close();
                j++;
            }
            m_request.Clear();
        }

        public void Loop()
        {
            m_list.Clear();
            for (int i = 0; i < m_apply.Count; i++)
            {
                Loader loader = m_apply[i];
                if (loader.done)
                {
                    m_list.Add(loader);
                    if (!loader.OnLoaded())
                        m_request.Insert(0, loader);
                }
            }

            for (int j = 0; j < m_list.Count; j++)
            {
                m_apply.Remove(m_list[j]);
            }

            for (int k = m_apply.Count; k < m_count; k++)
            {
                if (m_request.Count <= 0)
                    break;
                Loader loader2 = m_request[0];
                if (mono != null)
                {
                    m_request.RemoveAt(0);
                    m_apply.Add(loader2);
                    mono.StartCoroutine(loader2.Request());
                }
            }
        }
    }
}
