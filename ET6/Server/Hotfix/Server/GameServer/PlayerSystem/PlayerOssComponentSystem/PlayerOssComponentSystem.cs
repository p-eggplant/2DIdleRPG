/*----------------------------------------------------------------
* 文件名:	PlayerOssComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/30 17:13:31
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
    [FriendClassAttribute(typeof(PlayerOssComponent))]

    // 构造函数
    public class PlayerOssComponent_IAwake : AwakeSystem<PlayerOssComponent>
    {
        public override void Awake(PlayerOssComponent self)
        {
            self.m_listDataOss.Clear();
            self.m_listMaterialOss.Clear();
            self.m_listPropertyOss.Clear();
        }
    }

    // 析构函数
    public class PlayerOssComponent_IDestroy : DestroySystem<PlayerOssComponent>
    {
        public override void Destroy(PlayerOssComponent self)
        {
            self.m_listDataOss.Clear();
            self.m_listMaterialOss.Clear();
            self.m_listPropertyOss.Clear();
        }
    }



    [FriendClassAttribute(typeof(PlayerOssComponent))]
    public static class PlayerOssComponentSystem
    {

        public static void MaterialOss(this PlayerOssComponent self, int nConfigID, int nChangeNum, int nRealNum, EOssType eType, string szReason)
        {
            MaterialOss pMaterialOss = new MaterialOss();
            pMaterialOss.m_lTime = TimeHelper.ServerNow();
            pMaterialOss.m_nConfigID = nConfigID;
            pMaterialOss.m_nRealNum = nRealNum;
            pMaterialOss.m_nChangeNum = nChangeNum;
            pMaterialOss.m_szReason = szReason;
            pMaterialOss.m_eType = eType;
            self.m_listMaterialOss.Add(pMaterialOss);
        }


        public static void DataOss(this PlayerOssComponent self, EPlayerDataType eConfigID, long lChangeNum, long lRealNum, EOssType eType, string szReason)
        {
            DataOss pDataOss = new DataOss();
            pDataOss.m_lTime = TimeHelper.ServerNow();
            pDataOss.m_nConfigID = eConfigID;
            pDataOss.m_lRealNum = lRealNum;
            pDataOss.m_lChangeNum = lChangeNum;
            pDataOss.m_szReason = szReason;
            pDataOss.m_eType = eType;
            self.m_listDataOss.Add(pDataOss);
        }


        public static void PropertyOss(this PlayerOssComponent self, EPlayerPropertyType nConfigID, EOssType eType, ESystemType eToType, double dwChangeNum, double dwRealNum,string szReason)
        {
            PropertyOss pPropertyOss = new PropertyOss();
            pPropertyOss.m_ePropType = nConfigID;
            pPropertyOss.m_dwRealNum = dwRealNum;
            pPropertyOss.m_dwChangeNum = dwChangeNum;
            
            pPropertyOss.m_eOssType = eType;
            pPropertyOss.m_eSystemType = eToType;

            pPropertyOss.m_szReason = szReason;

            self.m_listPropertyOss.Add(pPropertyOss);
        }

    }
}


