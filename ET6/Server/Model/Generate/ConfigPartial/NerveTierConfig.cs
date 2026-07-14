/*----------------------------------------------------------------
* 文件名:	NerveTierConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/8 9:45:04
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Driver.Core.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET

{
    public partial class NerveTierConfig
    {
        public override void EndInit()
        {
            if (null == NerveConfigCategory.Instance.Get(NerveId))
            {
                throw new Exception("不合法的神经ID  NerveId = " + NerveId + " Id = " + Id);
            }
        }
    }

    public class ActiveConfig
    {
        //层数
        public int TierNum = 0;
        //属性id
        public EPlayerPropertyType numerciricId = 0;
        //属性数值
        public int numerciricNum = 0;
    }


    public partial class NerveTierConfigCategory
    {
        //key 神经ID
        //key2 层数
        //value 神经激活等级对应数值
        Dictionary<int, Dictionary<int, ActiveConfig>> m_NerveLevelConfigCategory = new Dictionary<int, Dictionary<int, ActiveConfig>>();



        /// <summary>
        /// 通过神经ID 和层数  得到激活效果配置
        /// </summary>
        /// <param name="NerveId">神经ID</param>
        /// <param name="NerveTier">神经层数</param>
        /// <returns>如果null,则无激活效果</returns>
        public ActiveConfig getMouldConfig(int NerveId, int NerveTier)
        {
            Dictionary<int, ActiveConfig> Dic = new Dictionary<int, ActiveConfig>();

            if (true == m_NerveLevelConfigCategory.TryGetValue(NerveId, out Dic))
            {
                ActiveConfig result = new ActiveConfig();
                if (true == Dic.TryGetValue(NerveTier, out result))
                {
                    return result;
                }
            }
            return null;

        }

        public override void AfterEndInit()
        {

            base.AfterEndInit();


            foreach (var config in list)
            {

                ActiveConfig pActiveConfig = new ActiveConfig();
                pActiveConfig.TierNum = config.NerveTier;
                //设置属性
                pActiveConfig.numerciricId = PlayerPropertyConfigCategory.Instance.ID2eType(config.TierNumericId);
                pActiveConfig.numerciricNum = config.TierNumericData;

                //安全判断
                if (0 == config.NerveId)
                {
                    Log.Error("NerveId  == 0  id =" + config.NerveId);
                    break;
                }

                // 组件字典
                if (m_NerveLevelConfigCategory.TryGetValue(config.NerveId, out Dictionary<int, ActiveConfig> Dic))
                {

                    if (true == Dic.ContainsKey(config.NerveTier))
                    {
                        Log.Error("请专心配表： NerveId =" + config.NerveId + " NerveTier = " + config.NerveTier);
                        break;
                    }
                    Dic.Add(config.NerveTier, pActiveConfig);
                }
                else
                {
                    Dic = new Dictionary<int, ActiveConfig>
                    {
                        {config.NerveTier, pActiveConfig }
                    };
                    m_NerveLevelConfigCategory[config.NerveId] = Dic;
                }
            }
        }
    }
}

