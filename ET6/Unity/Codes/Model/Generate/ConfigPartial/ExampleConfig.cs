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
    public partial class ExampleConfig
    {
        public override void EndInit()
        {
            base.EndInit();
            // 配置数据 安全判断 
            if (param1 == "")
            {
                throw new Exception($"id = {Id} param1 = {param1}");
            }
        }
    }

    public partial class ExampleConfigCategory
    {
        public override void AfterEndInit()
        {
            base.AfterEndInit();
            foreach (var pConfig in list)
            {

            }

        }
    }
}
