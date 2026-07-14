/*----------------------------------------------------------------
* 文件名:	UnitCacheInitComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 14:59:44
* 创建人:   黎晨希
* 描  述:	对继承了IUnitCache的组件类进行管理，记录player对象的组件，
*           使不同组件可以通过接口统一调用Create和Init方法。

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
    [ComponentOf(typeof(Scene))]
    public class UnitCacheInitComponent : Entity, IAwake, ILoad
    {
        //字典<组件名, IUnitCacheInit接口对象>
        public Dictionary<string, IPlayerComponentEvent> allIUnitCacheInits;
    }
}
