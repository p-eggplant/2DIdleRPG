/*----------------------------------------------------------------
* 文件名:	UIDoTweenTestEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/3 16:00:11
* 创建人:   陈澍
* 描  述:	数值持续增长动画脚本

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
public class DoTweenObj : MonoBehaviour
{
    public Text m_Test = null;
    public float m_fMoveTime = 1f;       //动画时间(默认1秒)

    /// <summary>
    /// 执行数值逐渐增长动画
    /// </summary>
    /// <param name="m_dwStartNum">起始数值</param>
    /// <param name="m_dwEndNum">结束数值</param>
    public void NumJump(double m_dwStartNum, double m_dwEndNum)
    {
        DOTween.To(() => m_dwStartNum, m_dwEndNum => UpdateNumberText(m_dwEndNum), m_dwEndNum, m_fMoveTime).SetEase(Ease.Linear);
    }

    // 这个方法将更新UI文本中的数字  
    void UpdateNumberText(double newValue)
    {
        m_Test.text = newValue.ToString("F0"); // 将新值转换为字符串并更新到UI文本上,并去除小数点 
    }
}
