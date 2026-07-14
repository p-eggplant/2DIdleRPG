/*----------------------------------------------------------------
* 文件名:	CommonManuals
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/5 13:52:01
* 创建人:   陈澍
* 描  述:	客户端UI代码常用手册

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/
/*
//////////////////资源加载与场景挂载///////////////////////UILogin.unity3d////注意ab包
// 当AB被加载后，会调用LoadUI
self.Domain.GetComponent<ResourcesLoaderComponent>().Load("UIxxx.unity3d");
GameObject pUIxxx = (GameObject)ResourcesComponent.Instance.GetAsset("UIxxx.unity3d", "UIxxx");
self.m_window = GameObject.Instantiate(pUIxxx, dicLayer[EUILayer.low]);
self.m_window.name = szWindowName;

pUIxxx.GetComponent<RectTransform>().localScale = Vector3.one;
pUIxxx.GetComponent<RectTransform>().localPosition = Vector3.zero;

//////////////////获取组件//////////////////////////////////
Player pPlayer = self.DomainScene().GetComponent<Player>();
//获取玩家组件
PlayerxxxComponent pPlayerxxxComponent = pPlayer.GetComponent<PlayerxxxComponent>();
if (pPlayerxxxComponent == null)
{
    throw new Exception("pPlayerxxxComponent == null");
}

//////////////////显示/隐藏窗口/////////////////////////////
UIComponent.Instance.ShowWindow("UIxxx");
UIComponent.Instance.HideWindow("UIxxx");

//////////////////按钮监听//////////////////////////////////
UITools.FindChildComponent<Button>(self.m_window, "EBt_")?.AddListener(self.OnClick);

//////////////////设置文本//////////////////////////////////
UITools.FindChildComponent<Text>(pItem, "ELbxxx")?.SetText(sz);

//////////////////发起网络请求//////////////////////////////
Scene zoneScene = self.DomainScene();
C2Game_xxx pC2Game_xxx = new C2Game_xxx();
SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

Game2C_xxx pGame2C_xxx = (Game2C_xxx)await pSessionComponent.Session.Call(
    pC2Game_xxx);
// 返回成功
if (pGame2C_xxx.Error == ErrorCode.ERR_Success)
{

}

//////////////////创建列表（Event）/////////////////////////
//Item交给UIComponent Hold住
var pESV_List = UITools.FindChild(self.m_window, "xxx");
GameObject pItem_LoginPlayerList = UITools.FindChild(pESV_List, "Item_GMOssList");
UIComponent.Instance.ItemHold(pItem_LoginPlayerList);

//////////////////生成列表（System）////////////////////////
var pE_SL_MaterialList = UITools.FindChild(self.m_window, "E_SL_MaterialList");
var pContent = UITools.FindChild(pE_SL_MaterialList, "Content");
UITools.DestroyChildren(pContent);
GameObject objItem = UIComponent.Instance.ItemGet("Item_dlgpackagemain_MaterialList");
foreach (var item in )
{
    //生成Item
    GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
}

//////////////////获取UI窗口////////////////////////////////
UIComponent.Instance.GetWindow<UIxxx>()?.;
*/