/*----------------------------------------------------------------
* 文件名:	Common_manuals
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/5 13:31:20
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/
/*
//////////////////////////////常用代码手册//////////////////////////////////
//获取玩家组件
PlayerxxxComponent pPlayerxxxComponent = player.GetComponent<PlayerxxxComponent>();
if (pPlayerxxxComponent == null)
{
    throw new Exception(" pPlayerxxxComponent = null");
}

//判断配置ID是否存在
if (xxxConfigCategory.Instance.Contain(nConfigId) == false)
{
    throw new Exception("不存在的xxID configId =" + nConfigId);
}
*/