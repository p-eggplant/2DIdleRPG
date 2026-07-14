/*----------------------------------------------------------------
* 文件名:	Translate
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/19 20:04:55
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
    public partial class Translate
    {
        public override void EndInit()
        {
            if (Id == 0)
            {
                throw new Exception($"id = {Id} " + "");
            }
        }
    }

    public partial class TranslateCategory
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
