/*----------------------------------------------------------------
* 文件名:	UnitCacheInitComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2023/5/5 15:04:52
* 创建人:   黎晨希
* 描  述:	IUnitCacheInit的方法，对继承了IUnitCache的组件类进行管理，
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
    [FriendClassAttribute(typeof(ET.UnitCacheInitComponent))]
    public static class UnitCacheInitComponentSystem
    {
        [ObjectSystem]
        public class UnitCacheInitComponentAwakeSystem : AwakeSystem<UnitCacheInitComponent>
        {
            public override void Awake(UnitCacheInitComponent self)
            {
                self.Init();
            }
        }

        public class UnitCacheInitComponentLoadSystem : LoadSystem<UnitCacheInitComponent>
        {
            public override void Load(UnitCacheInitComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this UnitCacheInitComponent self)
        {
            //实例化IUnitCacheInit接口管理字典
            self.allIUnitCacheInits = new Dictionary<string, IPlayerComponentEvent>();
            //获取所有挂载了UnitCacheAttribute特性的类型
            List<Type> types = Game.EventSystem.GetTypes(typeof(IPlayerComponentEvent));

            foreach (Type type in types)
            {
                //得到该初始化方法类的UnitCacheAttribute特性上 标识的组件类型TypeName（如BagComponent_S）
                object[] attr = type.GetCustomAttributes(typeof(PlayerComponentEventAttribute), false);
                PlayerComponentEventAttribute unitCacheAttribute = (PlayerComponentEventAttribute)attr[0];

                //实例化该组件的 初始化方法类，并强转成其接口对象
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                if (!self.allIUnitCacheInits.ContainsKey(unitCacheAttribute.TypeName))
                {
                    self.allIUnitCacheInits.Add(unitCacheAttribute.TypeName, obj);
                }
            }
        }

        public static Dictionary<string, IPlayerComponentEvent> GetIUnitCacheInits(this UnitCacheInitComponent self)
        {
            return self.allIUnitCacheInits;
        }

    }
}
