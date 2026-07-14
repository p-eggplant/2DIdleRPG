/*----------------------------------------------------------------
* 文件名:	GeneratePlayerIdComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/24 18:14:37
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/



namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class GenerateIdComponent : Entity , IAwake
    {
        public long m_NowTime = 0;          // 当前时间
        public ushort m_Index = 0;             // 计数
    }
}
