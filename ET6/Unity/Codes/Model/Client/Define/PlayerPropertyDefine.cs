/*----------------------------------------------------------------
* 文件名:	PlayerPropertyDefine
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 9:34:10
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
    /// <summary>
    /// 用于属性存储和战力计算
    /// </summary>
    public enum ESystemType
    {
        // ========================================= 属性组件创建需要分配内存的系统 开始=============================
        RankSystem,                 // 境界系统 
        NerveSystem,                // 神经系统
        DNASystem,                  // 基因系统
        EquipmentSystem,            // 装备系统
        FoodSystem,                 // 密药系统
        RoleupSystem,               // 等级系统
        Max,                        // 放在最后
    }
    /// <summary>
    /// Player的属性类型
    /// </summary>
    public enum EPlayerPropertyType
    {
        None = 0,
        UnitStart = None,                        // Unit属性 开始-------------------
        MaxHp,                                   // 最大血量
        Attack,                                  // 攻击
        Defense,                                 // 防御
        Speed,                                   // 移速

        Overwhelmed,                             // 破防
        Blocking,                                // 格挡
        HitValue,                                // 命中值
        DodgeValue,                              // 闪避值
        CriticalHit,                             // 暴击值
        CriticalResist,                          // 暴击抗性
        CriticalMulti,                           // 暴击倍率
        CriticalBlocking,                        // 暴击格挡
        OrganDamagePct,                          // 器官伤害加成
        OrganDamageReducePct,                    // 器官伤害减免
        SkillDamagePct,                          // 技能伤害加成
        SkillDamageReducePct,                    // 技能伤害减免
        MonsterDamagePct,                       // 攻击怪物伤害加成
        MonsterDamageReducePct,                 // 收到怪物伤害减免
        UnitEnd = MonsterDamageReducePct,    //  Unit 属性 结束-------------------



        // 加成影响值，会影响基础之，需要特殊处理
        Markup_MaxHp,               // 各系统血量加成
        Markup_Attack,              // 各系统攻击加成
        Markup_Defense,             // 各系统防御加成



        //Player专属属性
        BaseAutoExp,                //基础灵气
        AutoExpPct,                 //基础灵气加成
        EvolveLimit,                //每日吐纳次数
        EvolveExpPct,               //呼吸经验加成
        EatLimit,                   //每日进食次数
        EatExpPct,                  //进食经验加成
        AbsorbRate,                 //境界吸收率
        AutoGoldPct,                //秘境获得金币加成
        AutoTimePct,                //秘境挂机时长加成
        MaxHpRecoverPct,            //血量回复加成
        OreSpeedPct,                //矿藏采集速率加成

        Max,
    }
}
