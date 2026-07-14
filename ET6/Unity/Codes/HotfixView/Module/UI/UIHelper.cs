using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public static class EUIHelper
    {

        #region UI辅助方法

        public static void SetText(this Text Label, string content)
        {
            if (null == Label)
            {
                Log.Error("label is null");
                return;
            }
            Label.text = content;
        }

        public static void SetSprite(this Image pImage, string szPath)
        {
            Sprite pSprite = (Sprite)ResourcesComponent.Instance.GetAsset($"UILogin.unity3d", szPath);    
            if (pSprite == null)
            {
                //Log.Error($"名称{szPath}，pSprite == null");
                return;
            }
            pImage.sprite = pSprite;
        }
        //public static void SetText(this CTranslateText Label, int ConfigId)
        //{
        //    if (null == Label)
        //    {
        //        Log.Error("label is null");
        //        return;
        //    }
        //    UITextConfig uITextConfig = null;
        //    if (UITextConfigCategory.Instance.Contain(ConfigId))
        //    {
        //        uITextConfig = UITextConfigCategory.Instance.Get(ConfigId);
        //        if (uITextConfig == null)
        //        {
        //            Log.Error("uITextConfig is null");
        //            return;
        //        }
        //    }

        //    //文本
        //    Label.text = uITextConfig.DefaultContent;
        //    //翻译id
        //    Label.m_nTranslateID = uITextConfig.TranslateId;
        //    //刷新UI
        //    Label.Refresh();
        //}


        #endregion

        #region UI按钮事件

        public static void AddListenerAsyncWithId(this Button button, Func<int, ETTask> action, int id)
        {
            button.onClick.RemoveAllListeners();

            async ETTask clickActionAsync()
            {
                UIComponent.Instance?.AddLock();
                await action(id);
                UIComponent.Instance?.UnLock();
            }

            button.onClick.AddListener(() =>
            {
                if (UIComponent.Instance == null)
                {
                    return;
                }

                if (UIComponent.Instance.isLocked())
                {
                    return;
                }

                clickActionAsync().Coroutine();
            });
        }

        /// <summary>
        /// 添加按钮异步事件监听，如有按钮音效需求，点进来看代码
        /// </summary>
        /// <param name="button"></param>
        /// <param name="clickEventHandler">按钮事件监听</param>
        /// <param name="clickSound">按钮音效：默认音效为点击弹窗的音效</param>
        /// <param name="playSound">是否播放按钮音效</param>
        public static void AddListenerAsync(this Button button, Func<ETTask> action)
        {
            button.onClick.RemoveAllListeners();

            async ETTask clickActionAsync()
            {
                UIComponent.Instance?.AddLock();
                await action();
                UIComponent.Instance?.UnLock();
            }



            button.onClick.AddListener(() =>
            {
                if (UIComponent.Instance == null)
                {
                    return;
                }

                if (UIComponent.Instance.isLocked())
                {
                    return;
                }

                clickActionAsync().Coroutine();
            });

        }

        public static void AddListener(this Toggle toggle, UnityAction<bool> selectEventHandler)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(selectEventHandler);
        }

        /// <summary>
        /// 添加按钮事件监听，如有按钮音效需求，点进来看代码
        /// </summary>
        /// <param name="button"></param>
        /// <param name="clickEventHandler">按钮事件监听</param>
        /// <param name="clickSound">按钮音效：默认音效为点击弹窗的音效</param>
        /// <param name="playSound">是否播放按钮音效</param>
        public static void AddListener(this Button button, UnityAction clickEventHandler)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickEventHandler);

        }

        public static void AddListenerWithId(this Button button, Action<int> clickEventHandler, int id)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(id); });
        }

        public static void AddListenerWithId(this Button button, Action<long> clickEventHandler, long id)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(id); });
        }

        public static void AddListenerWithParam<T>(this Button button, Action<T> clickEventHandler, T param)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(param); });
        }

        public static void AddListenerWithParam<T, A>(this Button button, Action<T, A> clickEventHandler, T param1, A param2)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickEventHandler(param1, param2); });
        }


        public static void AddListener(this ToggleGroup toggleGroup, UnityAction<int> selectEventHandler)
        {
            var togglesList = toggleGroup.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < togglesList.Length; i++)
            {
                int index = i;
                togglesList[i].AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        selectEventHandler(index);
                    }
                });
            }
        }



        public static void RegisterEvent(this EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> callback)
        {
            EventTrigger.Entry entry = null;

            // 查找是否已经存在要注册的事件
            foreach (EventTrigger.Entry existingEntry in trigger.triggers)
            {
                if (existingEntry.eventID == eventType)
                {
                    entry = existingEntry;
                    break;
                }
            }

            // 如果这个事件不存在，就创建新的实例
            if (entry == null)
            {
                entry = new EventTrigger.Entry();
                entry.eventID = eventType;
            }
            // 添加触发回调并注册事件
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }


        #endregion

    }
}

