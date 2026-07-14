/*----------------------------------------------------------------
* 文件名:	UIBase
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/20 13:56:04
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using ET;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class UIEventAttribute : BaseAttribute
{
    public string TypeName { get; }

    public UIEventAttribute(string szName)
    {
        this.TypeName = szName;
    }
}


public abstract class UIEventBase<T>: IUIEvent where T : Entity
{
    public void CreateWindow(ref UIComponent pUIComponent)
    {
        OnCreateWindow(ref pUIComponent);
    }
    public abstract void OnCreateWindow(ref UIComponent pUIComponent );


    public void LoadResouse(UIComponent pUIComponent, string szWindowName, Dictionary<EUILayer, Transform> dicLayer)
    {
        if (pUIComponent == null || pUIComponent.IsDisposed)
            return;

        T unitComponent = pUIComponent.GetComponent<T>();

        if (unitComponent == null || unitComponent.IsDisposed)
            return;

        OnLoadResouse(unitComponent, szWindowName, dicLayer);
    }
    public abstract void OnLoadResouse(T self, string szWindowName, Dictionary<EUILayer, Transform> dicLayer);



    public void DestroyResouse(UIComponent pUIComponent)
    {
        if (pUIComponent == null || pUIComponent.IsDisposed)
            return;

        T unitComponent = pUIComponent.GetComponent<T>();

        if (unitComponent == null || unitComponent.IsDisposed)
            return;

        OnDestroyResouse(unitComponent);
    }
    public abstract void OnDestroyResouse(T self);

    public void ShowWindow(UIComponent pUIComponent, object showData)
    {
        if (pUIComponent == null || pUIComponent.IsDisposed)
            return;

        T unitComponent = pUIComponent.GetComponent<T>();

        if (unitComponent == null || unitComponent.IsDisposed)
            return;

        OnShowWindow(unitComponent, showData);
    }
    public abstract void OnShowWindow(T self, object showData);



    public void HideWindow(UIComponent pUIComponent)
    {
        if (pUIComponent == null || pUIComponent.IsDisposed)
            return;

        T unitComponent = pUIComponent.GetComponent<T>();

        if (unitComponent == null || unitComponent.IsDisposed)
            return;

        OnHideWindow(unitComponent);
    }
    public abstract void OnHideWindow(T self);

}

public interface IUIEvent
{
    
    public  void CreateWindow(ref UIComponent pUIComponent);

    public  void LoadResouse(UIComponent pUIComponent, string szWindowName, Dictionary<EUILayer,Transform> dicLayer);

    public  void DestroyResouse(UIComponent pUIComponent);

    public  void ShowWindow(UIComponent pUIComponent, object showData);
  
    public  void HideWindow(UIComponent pUIComponent);
}

