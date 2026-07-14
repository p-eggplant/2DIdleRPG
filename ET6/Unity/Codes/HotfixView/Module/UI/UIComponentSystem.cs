/*----------------------------------------------------------------
* 文件名:	UIManagerComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/20 17:36:19
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;


namespace ET
{

    [ObjectSystem]
    public class UIComponent_AwakeSystem : AwakeSystem<UIComponent>
    {
        public override void Awake(UIComponent self)
        {
            UIComponent.Instance = self;
            self.m_dicEvent.Clear();

            // 找item临时挂在点
            self.m_ItemParent = GameObject.Find("/Global/UI/ItemTemp");

            // 找到所有的layer
            GameObject uiRoot = GameObject.Find("/Global/UI");
            for(int i = 0; i < (int)EUILayer.Max; i++ )
            {
                EUILayer LayerType = (EUILayer)i;
                GameObject Layer = GameObject.Find("/Global/UI/" + LayerType.ToString());
                if(Layer == null)
                {
                    throw new Exception($"未能找到: {LayerType.ToString()}");
                }
                Layer.SetActive(true);
                self.m_dicLayer[LayerType] = Layer.transform;
            }


            // 创建事件对象
            var uiEvents = Game.EventSystem.GetTypes(typeof(global::UIEventAttribute));
            foreach (Type type in uiEvents)
            {
                object[] attrs = type.GetCustomAttributes(typeof(global::UIEventAttribute), false);
                if (attrs.Length != 0)
                {
                    global::UIEventAttribute uiEventAttribute = attrs[0] as global::UIEventAttribute;
                    IUIEvent aUIEvent = Activator.CreateInstance(type) as IUIEvent;
                    self.m_dicEvent.Add(uiEventAttribute.TypeName, aUIEvent);

                    aUIEvent.CreateWindow(ref self);
                }
            }
            Log.Debug("");
        }
    }



    [FriendClass(typeof(UIComponent))]
    public static class UIComponentSystem
    {


        /// <summary>
        /// 清理掉缓存的item，当你窗口资源被关闭时。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="szItemName"></param>
        public static void ItemDestory(this UIComponent self, string szItemName)
        {
            Transform pChild = self.m_ItemParent.transform.Find(szItemName);
            if (pChild == null)
            {
                return;
            }
            GameObject.Destroy(pChild.gameObject);
        }



        /// <summary>
        /// 帮你缓存列表item, 这样策划可以把列表拼装在list中，方便查看效果，也不会产生过多的文件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="objItem"></param>
        /// <exception cref="Exception"></exception>
        public static void ItemHold(this UIComponent self, GameObject objItem)
        { 
            Transform pChild = self.m_ItemParent.transform.Find(objItem.name);
            if (pChild != null)
            {
                throw new Exception("之前Hold的Item 没有清理掉，或者遇到了同名的 Item = " + objItem.name);
            }
            GameObject pItem = GameObject.Instantiate(objItem, self.m_ItemParent.transform);
            pItem.SetActive(false);
            pItem.name = objItem.name;
            GameObject.Destroy(objItem);
        }


        /// <summary>
        /// 通过列表ITEM的名字查询，返回给你缓存的GameObject
        /// </summary>
        /// <param name="self"></param>
        /// <param name="szItemName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static GameObject ItemGet(this UIComponent self, string szItemName)
        {
            Transform pChild =  self.m_ItemParent.transform.Find(szItemName);
            if(pChild == null)
            {
                throw new Exception("未找到你的item = " + szItemName);
            }
            pChild.gameObject.SetActive(true);
            return pChild.gameObject;
        }


        public static T GetWindow<T>(this UIComponent self) where T : Entity
        {
            T window = self.GetComponent<T>();
            if (window == null || window.IsDisposed)
            {
                throw new Exception($"窗口不存在  nameof(T) = {nameof(T)}");
            }
            return window;
        }



        public static void ShowWindow(this UIComponent self,string szWindowName, object showData = null) 
        {
            try
            {
                // 通过T找到对应的事件分发器
                
                IUIEvent uIEvent = null;

                if (false == self.m_dicEvent.TryGetValue(szWindowName, out uIEvent))
                {
                    throw new Exception($"无法找到窗口事件类 nameof(T) = {szWindowName}");
                }
               

                // 查询所有层
                GameObject pWindow  = self.FindWindowTrans(szWindowName);
                if(pWindow == null)
                {
                    uIEvent.LoadResouse(self, szWindowName, self.m_dicLayer);
                    pWindow = self.FindWindowTrans(szWindowName);
                    if (pWindow == null)
                    {
                        throw new Exception($"窗口并没有被创建请实现OnLoadResource WindowName = {szWindowName}");
                    }
                    pWindow.SetActive(true);
                    uIEvent.ShowWindow(self, showData);
                }
                else
                {
                    if (pWindow.activeSelf == false)
                    {
                        pWindow.SetActive(true);
                        uIEvent.ShowWindow(self, showData);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }

        public static void HideWindowDestroy(this UIComponent self, string szWindowName)
        {
            try
            {
                // 通过T找到对应的事件分发器
  
                IUIEvent uIEvent = null;

                if (false == self.m_dicEvent.TryGetValue(szWindowName, out uIEvent))
                {
                    throw new Exception($"无法找到窗口事件类 WindowName = {szWindowName}");
                }

                // 查询所有层
                GameObject pWindow = self.FindWindowTrans(szWindowName);
                if (pWindow == null)
                {
                    return;
                }

                // 根据情况，进行各个回调
                if (pWindow.activeSelf == true)
                {
                    uIEvent.HideWindow(self);
                    pWindow.SetActive(false);
                }

                uIEvent.DestroyResouse(self);
                if (null != self.FindWindowTrans(szWindowName))
                {
                    throw new Exception($"窗口并没有被销毁请实现OnDestroyResource  WindowName ={szWindowName}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void HideWindow(this UIComponent self, string szWindowName)
        {
            try
            {
                // 通过T找到对应的事件分发器
                IUIEvent uIEvent = null;

                if (false == self.m_dicEvent.TryGetValue(szWindowName, out uIEvent))
                {
                    throw new Exception($"无法找到窗口事件类 WindowName = {szWindowName}");
                }

                // 查询所有层
                GameObject pWindow = self.FindWindowTrans(szWindowName);
                if (pWindow == null)
                {
                    return;
                }

                // 根据情况，进行各个回调
                if(pWindow.activeSelf == true)
                {
                    uIEvent.HideWindow(self);
                    pWindow.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }


        public static void DestroyAllWindow(this UIComponent self)
        {
            // 清除缓存的ITEM
            UITools.DestroyChildren(self.m_ItemParent);

            foreach(var node in self.m_dicEvent)
            {
                string szWindowName = node.Key;
                IUIEvent uiEvent = node.Value;

                GameObject pWindow = self.FindWindowTrans(szWindowName);
                if(pWindow != null)
                {
                    if(pWindow.gameObject.activeSelf == true) 
                    {
                        uiEvent.HideWindow(self);
                        pWindow.SetActive(false);
                    }

                    uiEvent.DestroyResouse(self);
                    if (null != self.FindWindowTrans(szWindowName))
                    {
                        throw new Exception($"窗口并没有被销毁请实现OnDestroyResource  WindowName ={szWindowName}");
                    }
                }
            }
          
        }




        private static GameObject FindWindowTrans(this UIComponent self, string szWindowName)
        {
            foreach (var node in self.m_dicLayer)
            {
                Transform wind = node.Value.Find(szWindowName);
                if (null != wind)
                {
                    return wind.gameObject;
                }
            }
            return null;
        }

        public static void AddLock(this UIComponent self)
        {
            self.m_AsyncLock++;
        }

        public static void UnLock(this UIComponent self)
        {
            if(self.m_AsyncLock > 0)
            self.m_AsyncLock--;
        }

        public static bool isLocked(this UIComponent self)
        {
            return self.m_AsyncLock != 0;
        }


        


    }
}
