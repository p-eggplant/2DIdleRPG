/*----------------------------------------------------------------
* 文件名:	PlayerPropertyComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 9:35:53
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{



    [ComponentOf(typeof(Player))]
    public class PlayerPropertyComponent : Entity, IAwake, IDestroy
    {
        public PlayerPropertyComponent()
        {
            for (int i = 0; i < (int)ESystemType.Max; i++)
            {
                m_arrSystemProp[i] = new double[(int)EPlayerPropertyType.Max];
            }
        }
        /// <summary>
        /// 各个系统
        /// </summary>
        [BsonIgnore]
        public double[][] m_arrSystemProp = new double[(int)ESystemType.Max][];
        public double[] m_arrayTotalProp = new double[(int)EPlayerPropertyType.Max];
    }
}
