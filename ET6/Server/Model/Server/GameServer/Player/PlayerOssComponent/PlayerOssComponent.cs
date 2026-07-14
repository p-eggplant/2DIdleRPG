/*----------------------------------------------------------------
* 文件名:	PlayerOssComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/30 17:11:22
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{


    /// <summary>
    /// 属性数值统计结构体
    /// </summary>
    public class PropertyOss
    {
        public EPlayerPropertyType m_ePropType;
        public ESystemType m_eSystemType;

        public double m_dwChangeNum;
        public double m_dwRealNum;
        public string m_szReason;

        public EOssType m_eOssType;
    }

    // 数据统计结构体
    public class DataOss
    {
        public long m_lTime;
        public EPlayerDataType m_nConfigID;
        public long m_lChangeNum;
        public long m_lRealNum;
        public string m_szReason;
        public EOssType m_eType;
    }

    // 物资统计结构体
    public class MaterialOss
    {
        public long m_lTime;
        public int m_nConfigID;
        public int m_nChangeNum;
        public int m_nRealNum;
        public string m_szReason;
        public EOssType m_eType;
    }

    [ComponentOf(typeof(Player))]
    public class PlayerOssComponent : Entity, IAwake, IDestroy
    {
        // 物资记录
        public List<MaterialOss> m_listMaterialOss = new List<MaterialOss>();
        // 数值记录
        public List<DataOss> m_listDataOss = new List<DataOss>();
        // 属性记录
        public List<PropertyOss> m_listPropertyOss = new List<PropertyOss>();
    }
}
