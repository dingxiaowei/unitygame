﻿using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预设资源，通常就一个资源，主要是 Resources 目录下的资源加载
     */
    public class PrefabResItem : ResItem
    {
        protected UnityEngine.Object m_prefabObj;   // 加载完成的 Prefab 对象
        protected UnityEngine.Object[] mAllPrefabObj;   // 所有的 Prefab 对象
        protected GameObject m_retGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            base.init(item);
            if (!mIsLoaded)
            {
                m_prefabObj = (item as ResourceLoadItem).prefabObj;
            }
            else
            {
                mAllPrefabObj = (item as ResourceLoadItem).getAllPrefabObject();
            }
            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public UnityEngine.Object prefabObj()
        {
            return m_prefabObj;
        }

        public override string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return m_prefabName;
        }

        override public void unload()
        {
            //Resources.UnloadAsset(m_prefabObj);   // 这个是同步卸载
            m_prefabObj = null;
            mAllPrefabObj = null;
            //Resources.UnloadUnusedAssets();         // 这个事异步卸载
            //GC.Collect();
        }

        override public void InstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(asyncInstantiateObject(resName, evtHandle));
        }

        override public GameObject InstantiateObject(string resName)
        {
            m_retGO = null;

            if (null == m_prefabObj)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_prefabObj) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }
            return m_retGO;
        }

        override public IEnumerator asyncInstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            GameObject retGO = null;

            if (null == m_prefabObj)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                retGO = GameObject.Instantiate(m_prefabObj) as GameObject;
                if (null == retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }

            yield return null;
            evtHandle.setInsGO(retGO);
            evtHandle.dispatchEvent(evtHandle);
        }

        override public UnityEngine.Object getObject(string resName)
        {
            return m_prefabObj;
        }

        override public UnityEngine.Object[] getAllObject()
        {
            return mAllPrefabObj;
        }

        override public T[] loadAllAssets<T>()
        {
            ArrayList list = new ArrayList(mAllPrefabObj);
            int idx = mAllPrefabObj.Length - 1;
            while(idx >= 0)
            {
                if(mAllPrefabObj[idx] is T)
                {
                    list.Remove(mAllPrefabObj[idx]);
                }

                --idx;
            }
            return list.ToArray() as T[];
        }

        override public byte[] getBytes(string resName)            // 获取字节数据
        {
            if(m_prefabObj != null && (m_prefabObj as TextAsset) != null)
            {
                return (m_prefabObj as TextAsset).bytes;
            }

            return null;
        }

        override public string getText(string resName)
        {
            if (m_prefabObj != null && (m_prefabObj as TextAsset) != null)
            {
                return (m_prefabObj as TextAsset).text;
            }

            return null;
        }
    }
}