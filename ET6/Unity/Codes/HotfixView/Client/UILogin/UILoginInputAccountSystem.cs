/*----------------------------------------------------------------
* 文件名:	UILoginInputAccountSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/25 13:36:19
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/


using UnityEngine.UI;


namespace ET
{
    // 构造函数
    public class UILoginInputAccount_IAwake : AwakeSystem<UILoginInputAccount>
    {
        public override void Awake(UILoginInputAccount self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UILoginInputAccount_IDestroy : DestroySystem<UILoginInputAccount>
    {
        public override void Destroy(UILoginInputAccount self)
        {
            self.m_window = null;
        }
    }

    [FriendClass(typeof(UILoginInputAccount))]


    public static class UILoginInputAccountSystem
    {

        
        public static void OnClickClose(this UILoginInputAccount self)
        {
            UIComponent.Instance.HideWindowDestroy("UILoginInputAccount");
        }

        public static void OnClickSure(this UILoginInputAccount self)
        {
            Account pAccount = self.DomainScene().GetComponent<Account>();

            InputField pAccountInput = UITools.FindChildComponent<InputField>(self.m_window, "EIf_Account");
            pAccount.m_szAccount = pAccountInput.text;

            // 调用主窗口函数
            UIComponent.Instance.GetWindow<UILoginMain>()?.OnRequestAccountLogin().Coroutine();
            self.OnClickClose();

        }
    }
}
