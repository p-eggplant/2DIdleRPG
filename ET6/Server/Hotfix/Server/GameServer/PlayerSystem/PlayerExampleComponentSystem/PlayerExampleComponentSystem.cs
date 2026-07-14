/*----------------------------------------------------------------
* 文件名:	PlayerExampleComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 20:48:17
* 创建人:   王星莅
* 描  述:	玩家组件示例， 一定要写构造和析构函数，因为对象销毁时，不会真正的销毁
*           会进入回收站，若不写，下次拿出来时，数据就脏了，所以自己用前用后一定要清理干净数据

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
    [FriendClassAttribute(typeof(PlayerExampleComponent))]

    // 构造函数
    public class PlayerExampleComponent_IAwake : AwakeSystem<PlayerExampleComponent>
    {
        public override void Awake(PlayerExampleComponent self)
        {
            self.m_dicExample.Clear();
        }
    }

    // 析构函数
    public class PlayerExampleComponent_IDestroy : DestroySystem<PlayerExampleComponent>
    {
        public override void Destroy(PlayerExampleComponent self)
        {
            self.m_dicExample.Clear();
        }
    }



    [FriendClassAttribute(typeof(PlayerExampleComponent))]
    public static class PlayerExampleComponentSystem
    {

        public static int Get(this PlayerExampleComponent self, int nId)
        {
            int result = 0;
            if(false == self.m_dicExample.TryGetValue(nId, out result))
            {
                // 非常重要！！！！
                // 任何不能被认同的执行，都应抛出异常，并且把关键参数打印
                throw new Exception($"传入的nId不能被找到 nId = {nId}");
            }
            return result;
        }


    }
}