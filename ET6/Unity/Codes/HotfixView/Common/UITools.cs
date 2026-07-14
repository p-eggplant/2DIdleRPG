/*----------------------------------------------------------------
* 文件名:	GToolsUnity
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/20 22:03:23
* 创建人:   
* 描  述:	和unity常用操作相关的工具

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;


namespace ET
{


    public static class UITools
    {
        


        public static void Tips(string szTips)
        {
            UIComponent.Instance.ShowWindow("UITips", szTips);
        }

        /// <summary>
        /// 销毁子孩子
        /// </summary>
        /// <param name="trans"></param>
        public static void DestroyChildren(GameObject trans)
        {
            for (int i = 0; i < trans.transform.childCount; i++)
            {
                Transform pChild = trans.transform.GetChild(i);
                if (pChild)
                {
                    GameObject.Destroy(pChild.gameObject);
                }
            }
        }
        /// <summary>
        /// 找子节点并获取节点的组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="pParent">父节点</param>
        /// <param name="szName">子节点名字</param>
        /// <param name="eType">深度搜索/广度搜索</param>
        /// <returns>组件</returns>
        public static T FindChildComponent<T>(GameObject pParent, string szName, EUISearch eType = EUISearch.Deep) where T : Component
        {
            GameObject resultTrs = UITools.FindChild(pParent, szName, eType);
            T res = resultTrs.gameObject.GetComponent<T>();
            if (res == null)
            {
                throw new System.Exception("Component == null szName= " + szName);
            }
            return res;
        }


        /// <summary>
        /// 找子节点 
        /// </summary>
        /// <param name="pParent">父节点</param>
        /// <param name="szName">子节点名字</param>
        /// <param name="eType">深度搜索/广度搜索</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static GameObject FindChild(GameObject pParent, string szName, EUISearch eType = EUISearch.Deep)
        {
            GameObject child = null;
            if (eType == EUISearch.Deep)
            {
                child = FindChild_dps(pParent, szName);
            }
            else if (eType == EUISearch.Breadth)
            {
                child = FindChild_bfs(pParent, szName);
            }

            if (child == null)
            {
                throw new System.Exception("GameObject == null szName= " + szName);
            }
            return child;
        }
        /// <summary>
        /// 查询搜索 深度搜索
        /// </summary>
        /// <param name="pParent"></param>
        /// <param name="szName"></param>
        /// <returns></returns>
        private static GameObject FindChild_dps(GameObject pParent, string szName)
        {
            if (pParent == null || szName == string.Empty)
            {
                throw new Exception("传入参数为空");
            }

            if (pParent.name == szName)
            {
                return pParent;
            }
            GameObject[] arrayRootChildren = new GameObject[pParent.transform.childCount];
            for (int i = 0; i < arrayRootChildren.Length; i++)
            {
                arrayRootChildren[i] = pParent.transform.GetChild(i).gameObject;
            }
            foreach (GameObject go in arrayRootChildren)
            {
                if (go != null)
                {
                    GameObject res = FindChild_dps(go, szName);
                    if (res != null)
                    {
                        return res;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 查询子节点 广度搜索
        /// </summary>
        /// <param name="pParent"></param>
        /// <param name="szName"></param>
        /// <returns></returns>
        private static GameObject FindChild_bfs(GameObject pParent, string szName)
        {
            if(pParent == null || szName == string.Empty)
            {
                throw new Exception("传人参数为空");
            }
            if(pParent.name == szName)
            {
                return pParent;
            }
            // 队列
            Queue<GameObject> queue = new Queue<GameObject>();
            for(int i = 0;i < pParent.transform.childCount; i++) 
            {
                queue.Enqueue(pParent.transform.GetChild(i).gameObject);
            }

            while (queue.Count > 0) 
            {
                GameObject child = queue.Dequeue();
                if (child == null)
                {
                    continue;
                }
                if(child.name == szName) 
                {
                    return child;
                }

                for (int i = 0; i < child.transform.childCount; i++)
                {
                    queue.Enqueue(child.transform.GetChild(i).gameObject);
                }
            }
            return null;
        }

    }
}
