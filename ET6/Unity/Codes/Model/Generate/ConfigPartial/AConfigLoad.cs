/*----------------------------------------------------------------
* 文件名:	ActionConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/10/11 11:40:48
* 创建人:   王星莅
* 描  述:	优先加载配置配置表

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System.Collections.Generic;

namespace ET
{
    public static partial class AConfigLoad
    {
        public static bool isHave(string szName)
        {

            for (int i = 0; i < g_ConfigLoad.Length; i++)
            {
                if (szName == g_ConfigLoad[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 优先加载配置
        /// </summary>
        public static readonly string[] g_ConfigLoad = 
        {
            "StartMachineConfig",
            "StartProcessConfig",
            "StartSceneConfig",
            "StartZoneConfig",

            "GameServerInfoConfig",
            "TranslateProc",
            "Translate",
            "ExampleConfig",
            "PlayerDataConfig",
            "MaterialConfig",
            "PlayerPropertyConfig",

            "NerveConfig",
            "NerveLayerConfig",
            "NerveMouldConfig",

            "RoleLevelUpConfig",
        };

       
    }
}
