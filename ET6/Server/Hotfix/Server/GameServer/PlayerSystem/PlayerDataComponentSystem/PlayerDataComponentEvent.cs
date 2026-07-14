/*----------------------------------------------------------------
* 文件名:	PlayerDataComponentEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 17:10:01
* 创建人:   王星莅
* 描  述:	玩家数值组件 事件处类

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [PlayerComponentEvent(nameof(PlayerDataComponent))]
    [FriendClassAttribute(typeof(PlayerDataComponent))]
    public class PlayerDataComponentEvent : ComponentBase<PlayerDataComponent>
    {
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerDataComponent>(true);
        }


        /// <summary>
        /// 玩家第一次创建初始化
        /// </summary>
        /// <param name="self"></param>
        public override void OnFirstCreate(PlayerDataComponent self) 
        {
            foreach(var pConfig in PlayerDataConfigCategory.Instance.m_listFirstValue)
            {
                self.m_arrData[(int)pConfig.EDataType] = pConfig.FirstCreateValue;
            }

        }


        /// <summary>
        /// 导入数据库数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dBUnitComponent"></param>
        /// <exception cref="Exception"></exception>
        public override void OnImprotDBData(PlayerDataComponent self, PlayerDataComponent dBUnitComponent)
        {
            // 数组初始化
            Array.Fill(self.m_arrData, 0);
            // 进行复制
            foreach (var node in dBUnitComponent.m_dicDBData)
            {
                EPlayerDataType eType = EPlayerDataType.None;
                if (true == EPlayerDataType.TryParse(node.Key, out eType))
                {
                    self.m_arrData[(int)eType] = node.Value;
                }
                else
                {
                    throw new Exception($"不存在的DataType  数据库中TypeString ={node.Key}");
                }
            }
        }



        /// <summary>
        /// 导出数据库数据
        /// </summary>
        /// <param name="self"></param>
        public override void OnExportDBData(PlayerDataComponent self)
        {
            self.m_dicDBData.Clear();
            for (int i = 0; i < self.m_arrData.Length; i++)
            {
                EPlayerDataType eDataType = (EPlayerDataType)i;
                long lValue = self.m_arrData[i];
                self.m_dicDBData.Add(eDataType.ToString(), lValue);
            }
        }


        /// <summary>
        /// 导出登录数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="info"></param>
        public override void OnExportLoginData(PlayerDataComponent self, ref PlayerLoginInfo info)
        {
            for(int i = 0; i < self.m_arrData.Length; i++)
            {
                if (self.m_arrData[i] != 0)
                {
                    PlayerDataProto pData = new PlayerDataProto();
                    pData.EnumId = i;
                    pData.Value = self.m_arrData[i];
                    info.PlayerDataComponent.Add(pData);
                }
            }            
        }



    }
}
