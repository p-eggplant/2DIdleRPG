/*----------------------------------------------------------------
* 文件名:	PlayerDataConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 16:37:17
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

    public partial class PlayerDataConfig
    {
        public EPlayerDataType EDataType;
        public override void EndInit()
        {
            if(false == EPlayerDataType.TryParse(DataTypeString, out EDataType))
            {
                throw new Exception($"数值类型配置错误 id = {Id} DataTypeString = {DataTypeString}");
            }
         
        }
    }

    public partial class PlayerDataConfigCategory
    {
        public List<PlayerDataConfig> m_listFirstValue = new List<PlayerDataConfig>();
        public override void AfterEndInit()
        {
            m_listFirstValue.Clear();
            base.AfterEndInit();
            foreach (var pConfig in list)
            {
                if (pConfig.FirstCreateValue > 0)
                {
                    m_listFirstValue.Add(pConfig);
                }
            }
        }
    }
}
