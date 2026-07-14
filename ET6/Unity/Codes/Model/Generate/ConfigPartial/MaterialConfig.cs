/*----------------------------------------------------------------
* 文件名:	MaterialConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 20:27:34
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public partial class MaterialConfig
    {

        public override void EndInit()
        {
            base.EndInit();
        }
    }

    public partial class MaterialConfigCategory
    {
        public List<MaterialConfig> m_listFirstValue = new List<MaterialConfig>();

        public override void AfterEndInit()
        {
            base.AfterEndInit();
            foreach (var pConfig in list)
            {
                if (pConfig.Hide == 0)
                {
                    m_listFirstValue.Add(pConfig);
                }
            }
        }
    }
}
