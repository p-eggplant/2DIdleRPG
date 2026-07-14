/*----------------------------------------------------------------
* 文件名:	UnitCacheNode
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 14:54:44
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
    /// 该接口用于识别 需要数据库存储 的组件
    /// </summary>
    public interface IUnitCacheNode
    {

    }

    [ComponentOf]
    public class UnitCacheNode : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 当前组件类型名称
        /// </summary>
        public string ComponentTypeName;

        /// <summary>
        /// 根据玩家Id获取组件
        /// </summary>
        public Dictionary<long, Entity> UnitCacheDic = new Dictionary<long, Entity>();
    }
}
