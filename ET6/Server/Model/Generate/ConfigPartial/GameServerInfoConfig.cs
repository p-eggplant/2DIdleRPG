/*----------------------------------------------------------------
* 文件名:	ExampleConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/19 20:20:50
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/


using System;

namespace ET
{
    public partial class GameServerInfoConfig
    {
        public override void EndInit()
        {

            // 配置数据 安全判断 
            if(false == StartSceneConfigCategory.Instance.Contain(Id))
            {
                throw new Exception($"不存在的服务器 id = {Id}");
            }

            if (false == StartZoneConfigCategory.Instance.Contain(Zoneid))
            {
                throw new Exception($"不存在的游戏区 id = {Id} Zoneid = {Zoneid}");
            }
        }
    }

    public partial class GameServerInfoConfigCategory
    {
        // 推荐 服务器id
        public int nDefaultGameServerID = 0;
        public string szDefaultGameServerName = "";
        public override void AfterEndInit()
        {

            base.AfterEndInit();
            foreach (var pConfig in list)
            {
                if(pConfig.isDefault  > 0)
                {
                    nDefaultGameServerID = pConfig.Id;
                    szDefaultGameServerName = pConfig.ServerName;
                }
            }

        }
    }
}
