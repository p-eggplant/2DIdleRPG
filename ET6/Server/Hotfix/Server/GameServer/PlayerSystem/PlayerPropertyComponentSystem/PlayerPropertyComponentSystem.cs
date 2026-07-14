/*----------------------------------------------------------------
* 文件名:	PlayerPropertyComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 10:01:59
* 创建人:
* 描  述:	

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
    [FriendClassAttribute(typeof(PlayerPropertyComponent))]

    // 构造函数
    public class PlayerPropertyComponent_IAwake : AwakeSystem<PlayerPropertyComponent>
    {
        public override void Awake(PlayerPropertyComponent self)
        {
            // 清理总数组
            Array.Fill(self.m_arrayTotalProp, 0);

            // 根据系统清理每个系统数组
            for (var i = 0; i < (int)ESystemType.Max; i++)
            {
                Array.Fill(self.m_arrSystemProp[i], 0);
            }

            // 将初始化的值，放在RankSystem中
            for (var i = 0; i < (int)EPlayerPropertyType.Max; i++)
            {
                PlayerPropertyConfig pAPlayerPropertyConfig = PlayerPropertyConfigCategory.Instance.Get((EPlayerPropertyType)i);
                if (pAPlayerPropertyConfig != null && pAPlayerPropertyConfig.InitPropNum > 0)
                {
                    self.InitAddProperty(ESystemType.RankSystem, (EPlayerPropertyType)i, pAPlayerPropertyConfig.InitPropNum, EOssType.RankSystem, "境界初始化");
                    //测试！！！！！！！！！！！！！！！！！！！！！！记得删
                    self.InitAddProperty(ESystemType.FoodSystem, (EPlayerPropertyType)i, pAPlayerPropertyConfig.InitPropNum, EOssType.FoodSystem, "密药初始化");
                }
            }
        }
    }

    // 析构函数
    public class PlayerPropertyComponent_IDestroy : DestroySystem<PlayerPropertyComponent>
    {
        public override void Destroy(PlayerPropertyComponent self)
        {
            // 清理整个数组
            for (var i = 0; i < (int)ESystemType.Max; i++)
            {
                Array.Fill(self.m_arrSystemProp[i], 0);
            }
            Array.Fill(self.m_arrayTotalProp, 0);
        }
    }



    [FriendClassAttribute(typeof(PlayerPropertyComponent))]
    public static class PlayerPropertyComponentSystem
    {

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eSystemType">系统类型</param>
        /// <param name="eType">属性类型</param>
        /// <returns></returns>
        public static double GetProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            return self.m_arrSystemProp[(int)eSystemType][(int)eType];
        }

        /// <summary>
        /// 获取所有系统的属性
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">属性类型</param>
        /// <returns></returns>
        public static double GetTotalProperty(this PlayerPropertyComponent self, EPlayerPropertyType eType)
        {
            return self.m_arrayTotalProp[(int)eType];
        }

        public static void SetProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwNum)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }


            if (dwNum < 0)
            {
                throw new Exception("传入设置属性小于0");
            }
            double dwOldValue = self.m_arrSystemProp[(int)eSystemType][(int)eType];
            //设置数值
            self.m_arrSystemProp[(int)eSystemType][(int)eType] = dwNum;


            // 计算总值
            self.CulTotalProperty(eSystemType, eType, dwNum - dwOldValue);

            //发送事件
            //Game.EventSystem.Publish(new EventType.PropertyChangeEventChange() { Player = self.GetParent<Player>(), eSystemType = eSystemType, ePropType = eType, OldPropertyNum = dwOldValue, NewPropertyNum = dwNum });
        }




        /// <summary>
        /// 初始化时调用，这个方法只加数值，不会进行其他任何操作
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eSystemType"></param>
        /// <param name="eType"></param>
        /// <param name="dwNum"></param>
        /// <returns></returns>
        public static void InitAddProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwNum, EOssType eOssType,string szReason)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            //设置数值
            self.m_arrSystemProp[(int)eSystemType][(int)eType] += dwNum;
            // 计算总值
            self.CulTotalProperty(eSystemType, eType, dwNum);
            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.PropertyOss(eType, eOssType, eSystemType, dwNum, self.m_arrSystemProp[(int)eSystemType][(int)eType], szReason);
        }

        public static bool isCanAddProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwNum)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            //数值判断
            if (dwNum < 0)
                return false;

            double curValue = self.GetProperty(eSystemType, eType);

            if (double.MaxValue - dwNum < curValue)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加属性
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eSystemType"></param>
        /// <param name="eType"></param>
        /// <param name="dwAddNum"></param>
        /// <returns></returns>
        public static void AddProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwAddNum, EOssType eOssType, string szReason)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            //数值判断
            if (dwAddNum < 0)
            {
                throw new Exception("传入设置属性小于0 eType = " + eType);
            }

            double curValue = self.GetProperty(eSystemType, eType);

            if (double.MaxValue - dwAddNum < curValue)
            {
                throw new Exception("属性添加后超出double值上限 eType = " + eType);
            }

            //设置数据
            self.SetProperty(eSystemType, eType, curValue + dwAddNum);
            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.PropertyOss(eType, eOssType, eSystemType, dwAddNum, self.m_arrSystemProp[(int)eSystemType][(int)eType], szReason);
            return;
        }

        public static bool isCamSubProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwNum)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            //数值判断
            if (dwNum < 0)
                return false;

            double curValue = self.GetProperty(eSystemType, eType);

            if (curValue < dwNum)
                return false;

            return true;
        }
        public static void SubProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwSubNum, EOssType eOssType, string szReason)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            //数值判断
            if (dwSubNum < 0)
            {
                throw new Exception("传入设置属性小于0 eType = " + eType);
            }

            double curValue = self.GetProperty(eSystemType, eType);

            if (curValue < dwSubNum)
            {
                throw new Exception("属性扣除后小于0 eType = " + eType);
            }

            //设置数据
            self.SetProperty(eSystemType, eType, curValue - dwSubNum);
            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.PropertyOss(eType, eOssType, eSystemType, -dwSubNum, self.m_arrSystemProp[(int)eSystemType][(int)eType], szReason);
            return;
        }

        /// <summary>
        /// 计算总属性，需要特殊处理加成值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eSystemType"></param>
        /// <param name="eType"></param>
        /// <param name="OldNum"></param>
        /// <param name="NewNum"></param>
        private static void CulTotalProperty(this PlayerPropertyComponent self, ESystemType eSystemType, EPlayerPropertyType eType, double dwChangeNum)
        {
            if (eSystemType >= ESystemType.Max)
            {
                throw new Exception("不合法的类型传入");
            }

            if (dwChangeNum == 0)
                return;

            switch (eType)
            {
                // 血量改变
                case EPlayerPropertyType.MaxHp:
                    double dwMarkup_MaxHp = self.m_arrSystemProp[(int)eSystemType][(int)EPlayerPropertyType.Markup_MaxHp];
                    self.m_arrayTotalProp[(int)eType] += dwChangeNum * (1 + dwMarkup_MaxHp);
                    break;
                // 血量加成改变
                case EPlayerPropertyType.Markup_MaxHp:
                    double dwMaxHp = self.m_arrSystemProp[(int)eSystemType][(int)EPlayerPropertyType.MaxHp];
                    self.m_arrayTotalProp[(int)eType] += dwMaxHp * dwChangeNum;
                    break;
                // 攻击改变
                case EPlayerPropertyType.Attack:
                    double dwMarkup_Attack = self.m_arrSystemProp[(int)eSystemType][(int)EPlayerPropertyType.Markup_Attack];
                    self.m_arrayTotalProp[(int)eType] += dwChangeNum * (1 + dwMarkup_Attack);
                    break;
                // 攻击加成改变
                case EPlayerPropertyType.Markup_Attack:
                    double dwAttack = self.m_arrSystemProp[(int)eSystemType][(int)EPlayerPropertyType.Attack];
                    self.m_arrayTotalProp[(int)eType] += dwAttack * dwChangeNum;
                    break;
                // 防御改变
                case EPlayerPropertyType.Defense:
                    double dwMarkup_Defense = self.m_arrSystemProp[(int)eSystemType][(int)EPlayerPropertyType.Markup_Defense];
                    self.m_arrayTotalProp[(int)eType] += dwChangeNum * (1 + dwMarkup_Defense);
                    break;
                // 防御加成改变
                case EPlayerPropertyType.Markup_Defense:
                    double dwDefense = self.m_arrSystemProp[(int)eSystemType][(int)EPlayerPropertyType.Defense];
                    self.m_arrayTotalProp[(int)eType] += dwDefense * dwChangeNum;
                    break;
                default:
                    self.m_arrayTotalProp[(int)eType] += dwChangeNum;
                    return;
            }
        }
    }
}
