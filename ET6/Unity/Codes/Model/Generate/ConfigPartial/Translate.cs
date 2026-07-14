/*----------------------------------------------------------------
* 文件名:	Translate
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/19 20:04:55
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/



using System;
using System.Collections.Generic;
using UnityEngine;
namespace ET
{
    public enum ETransLate
    {
        cn,
        en,
        hk,
    }


    public partial class Translate
    {
        public override void EndInit()
        {
            if (Id == 0)
            {
                throw new Exception($"id = {Id} " + "");
            }
        }
    }

    public partial class TranslateCategory
    {
        public Dictionary<string , Translate> m_dicTrans = new Dictionary<string , Translate>();
       
        public ETransLate eCurrent = ETransLate.cn;


        /// <summary>
        /// 获得多国语言
        /// </summary>
        /// <param name="szCn"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetCurrentLanguage(string szCn)
        {
            Translate Node = null;
            if(false ==  m_dicTrans.TryGetValue(szCn, out Node))
            {
                
                return szCn + "(未翻译)";
            }

            switch (eCurrent)
            {
                case ETransLate.cn:
                    return Node.cn;
                case ETransLate.en:
                    return Node.en;
                case ETransLate.hk:
                    return Node.hk;
                default:
                    throw new Exception("未知语言类型");
            }
        }


        public override void AfterEndInit()
        {
            m_dicTrans.Clear();
            base.AfterEndInit();
            foreach (var pConfig in list)
            {
                if(m_dicTrans.ContainsKey(pConfig.key) == true)
                {
                    throw new Exception($"已经存在的文本 id = {pConfig.Id}  Key={pConfig.key}");
                }
                m_dicTrans.Add(pConfig.key, pConfig);
            }

            // 将TranslateProc表也加入到这边管理
            Dictionary<int, TranslateProc> pTranslateProc = TranslateProcCategory.Instance.GetAll();
            foreach( var pConfigProc in pTranslateProc)
            {
                Translate pTranslate = new Translate();
                pTranslate.key = pConfigProc.Value.key;
                pTranslate.cn = pConfigProc.Value.cn;
                pTranslate.en = pConfigProc.Value.en;
                pTranslate.hk = pConfigProc.Value.hk;

                if (m_dicTrans.ContainsKey(pConfigProc.Value.key) == true)
                {
                    throw new Exception($"TranslateProc表和 Translate表的key值重复了，key={pConfigProc.Value.key}");
                }
                m_dicTrans.Add(pConfigProc.Value.key, pTranslate);
            }

        }
    }




}
