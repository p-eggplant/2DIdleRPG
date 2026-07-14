/*----------------------------------------------------------------
* 文件名:	TextTools
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/30 15:46:32
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;

using UnityEngine;




namespace ET
{

    public static class MyExtensions
    {
        public static string Trans(this System.String target)
        {
            return TranslateCategory.Instance.GetCurrentLanguage(target);
        }
    }


    public static class TextTools
    {
        // 翻译
       
    }

}
