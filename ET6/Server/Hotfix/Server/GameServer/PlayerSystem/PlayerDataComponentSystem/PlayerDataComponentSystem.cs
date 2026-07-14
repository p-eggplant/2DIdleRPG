/*----------------------------------------------------------------
* 文件名:	PlayerDataComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 16:58:04
* 创建人:   王星莅
* 描  述:	玩家数值组件，用于存放玩家数值，例如钻石，绑定钻石，
*           以及其他简单的组件所用到的数据，例如抽奖限定次数

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
    [FriendClassAttribute(typeof(PlayerDataComponent))]

    // 构造函数
    public class PlayerDataComponent_IAwake : AwakeSystem<PlayerDataComponent>
    {
        public override void Awake(PlayerDataComponent self)
        {
            // 清理总数组
            Array.Fill(self.m_arrData, 0);
            self.m_dicDBData.Clear();
           
        }
    }

    // 析构函数
    public class PlayerDataComponent_IDestroy : DestroySystem<PlayerDataComponent>
    {
        public override void Destroy(PlayerDataComponent self)
        {
            // 清理总数组
            Array.Fill(self.m_arrData, 0);
            self.m_dicDBData.Clear();

        }
    }



    [FriendClassAttribute(typeof(PlayerDataComponent))]
    public static class PlayerDataComponentSystem
    {

        /// <summary>
        /// 得到玩家数值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">类型</param>
        /// <returns>数值</returns>
        public static long GetData(this PlayerDataComponent self, EPlayerDataType eType)
        {
            return self.m_arrData[(int)eType];
        }


        /// <summary>
        /// 是否能增加数值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">类型</param>
        /// <param name="value">数值 只能为正整数</param>
        /// <returns></returns>
        public static bool isCanAddData(this PlayerDataComponent self, EPlayerDataType eType, long value)
        {
            //数值判断
            if (value <= 0)
            {
                return false;
            }
            // 越界判断
            long curValue = self.m_arrData[(int)eType];
            if (long.MaxValue - value < curValue)
            {
                return false ;
            }
            return true;
        }



       /// <summary>
       /// 增加数值
       /// </summary>
       /// <param name="self"></param>
       /// <param name="eType">数值类型</param>
       /// <param name="value">数值 只能为正整数</param>
       /// <param name="eOssType">原因</param>
       /// <exception cref="Exception"></exception>
        public static void AddData(this PlayerDataComponent self, EPlayerDataType eType, long value, EOssType eOssType, string OssReason)
        {
            //数值判断
            if (value <= 0)
            {
                throw new Exception($"数值小于等于零 eType = {eType.ToString()} value = {value} Oss = {eOssType}");
            }

            long curValue = self.m_arrData[(int)eType];
            if (long.MaxValue - value < curValue)
            {
                throw new Exception($"数值越界 eType = {eType.ToString()} value = {value} curValue = {curValue} Oss = {eOssType}");
            }

            //执行代码
            self.m_arrData[(int)eType] += value;

            // 下发客户端
            self.SyncToClient(eType);

            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.DataOss(eType, value, self.m_arrData[(int)eType], eOssType, OssReason);
        }



        /// <summary>
        /// 是否能减数值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">数值类型</param>
        /// <param name="value">数值，只能为正整数</param>
        /// <returns>是否能</returns>
        public static bool isCanSubData(this PlayerDataComponent self, EPlayerDataType eType, long value)
        {
            //数值判断
            if (value <= 0)
            {
                return false;
            }

            long curValue = self.m_arrData[(int)eType];
            if (curValue < value)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 减数值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">数值类型</param>
        /// <param name="value">数值</param>
        /// <param name="eOssType">原因</param>
        /// <exception cref="Exception"></exception>
        public static void SubData(this PlayerDataComponent self, EPlayerDataType eType, long value, EOssType eOssType, string OssReason)
        {
            //数值判断
            if (value <= 0)
            {
                throw new Exception($"数值小于等于零 eType = {eType.ToString()} value = {value} Oss = {OssReason}");
            }

            long curValue = self.m_arrData[(int)eType];

            if (curValue < value)
            {
                throw new Exception($"数值越界 eType = {eType.ToString()} value = {value} curValue = {curValue} Oss = {OssReason}");
            }

            //执行代码
            self.m_arrData[(int)eType] -= value;


            self.SyncToClient(eType);


            self.Parent.GetComponent<PlayerOssComponent>()?.DataOss(eType, -value, self.m_arrData[(int)eType], eOssType, OssReason);
        }



        /// <summary>
        /// 是否可以设置数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">数值类型</param>
        /// <param name="value">数值 只能为正整数</param>
        /// <returns>是否能</returns>
        public static bool isCanSetData(this PlayerDataComponent self, EPlayerDataType eType, long value)
        {
            if (value < 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">数值类型</param>
        /// <param name="value">数值 只能为正整数</param>
        /// <param name="eOssType">记录原因</param>
        /// <exception cref="Exception"></exception>
        public static void SetData(this PlayerDataComponent self, EPlayerDataType eType, long value, EOssType eOssType, string OssReason)
        {
            if (value < 0)
            {
                throw new Exception($"数值小于零 eType = {eType.ToString()} value = {value} Oss = {OssReason}");
            }

            //设置数值
            long curValue = self.m_arrData[(int)eType];
            self.m_arrData[(int)eType] = value;

            self.SyncToClient(eType);

            self.Parent.GetComponent<PlayerOssComponent>()?.DataOss(eType, value - curValue, self.m_arrData[(int)eType], eOssType, OssReason);
        }



        private static void SyncToClient(this PlayerDataComponent self, EPlayerDataType eType)
        {

            Player player = self.GetParent<Player>();
            if (player == null)
            {
                throw new Exception("Player == null");
            }

            if (eType > EPlayerDataType.Send2ClientStart && eType <= EPlayerDataType.Send2ClientEnd)
            {
                Game2C_PlayerDataChange game2C_PlayerDataSet = new Game2C_PlayerDataChange();
                game2C_PlayerDataSet.EnumId = (int)eType;
                game2C_PlayerDataSet.Value = self.m_arrData[(int)eType];
                MessageHelper.SendToClient(player, game2C_PlayerDataSet);
            }
    
        }

    }
}
