/*----------------------------------------------------------------
* 文件名:	PlayerDataComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 15:05:51
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/


using System.Linq;

namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerDataComponent : Entity, IAwake, IDestroy
    {
 
        /// <summary>
        /// 玩家内存数据
        /// </summary>
        public long[] m_arrData = new long[(int)EPlayerDataType.Max];


    }
}

