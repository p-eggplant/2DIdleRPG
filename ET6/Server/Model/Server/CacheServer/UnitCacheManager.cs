/*----------------------------------------------------------------
* 文件名:	UnitCacheManager
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 14:56:27
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
    [ChildType]
    [ComponentOf]
    public class UnitCacheManager : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 根据类型获取当前组件的信息
        /// </summary>
        public Dictionary<string, UnitCacheNode> UnitCaches = new Dictionary<string, UnitCacheNode>();

        /// <summary>
        /// 组件类型Type
        /// </summary>
        public List<string> UnitCacheKeyList = new List<string>();

        /// <summary>
        /// 数据库未存储数据<玩家ID, 未存储的组件字典>
        /// </summary>
        public Dictionary<long, DBNotStoredNode> DBNotStoredEntityDic = new Dictionary<long, DBNotStoredNode>();

        public class DBNotStoredNode
        {
            public int dbZoneId;
            public Dictionary<string, Entity> notStroredDic = new Dictionary<string, Entity>();
        }

        //数据库更新时间（毫秒）当前为半分钟
        private int update_time = 30000;

        /// <summary>
        /// 数据库更新时间
        /// </summary>
        public int Update_time { get => update_time; }

        //定时器
        public long Timer;

        //邮件↓
        ////全体系统邮件
        //public Dictionary<long, Dictionary<int, SystemEmailState>> SystemEmailStateDic = new Dictionary<long, Dictionary<int, SystemEmailState>>();

        ////未存储的全体系统邮件
        //public Dictionary<long, Dictionary<int, SystemEmailState>> DBNotStoredSystemEmailStateDic = new Dictionary<long, Dictionary<int, SystemEmailState>>();
        //邮件↑
    }
}
