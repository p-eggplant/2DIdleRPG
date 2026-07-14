//******************************************************************
// 文件名:	CVectorUtil.cs
// 版  权:	深圳热区网络科技有限公司(C)
// 创建人:	郑长基
// 日  期:	2017/07/02
// 版  本:	
// 描  述:	向量工具类
// 应  用:  

//************************** 修改记录 ******************************
// 修改人: 
// 日  期: 
// 描  述: 
//******************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CVectorUtil 
{

    /// <summary>
    /// 计算夹角的角度 0~360  
    /// </summary>
    /// <param name="v3From"></param>
    /// <param name="v3To"></param>
    /// <returns></returns>
    public static float Angle_360(Vector3 v3From, Vector3 v3To)
    {
        Vector3 v3 = Vector3.Cross(v3From, v3To);
        if (v3.z > 0)
            return Vector3.Angle(v3From, v3To);
        else
            return -Vector3.Angle(v3From, v3To) + 360;
    }

    /// <summary>
    /// 计算夹角的角度 -180~180 顺时针为负数
    /// </summary>
    /// <param name="v3From"></param>
    /// <param name="v3To"></param>
    /// <returns></returns>
    public static float Angle_180(Vector3 v3From, Vector3 v3To)
    {
        Vector3 v3 = Vector3.Cross(v3From, v3To);
        if (v3.z > 0)
            return Vector3.Angle(v3From, v3To);
        else
            return -Vector3.Angle(v3From, v3To);
    }


    /// <summary>
    /// 旋转向量，使其方向改变，大小不变
    /// </summary>
    /// <param name="v">需要旋转的向量</param>
    /// <param name="angle">旋转的角度</param>
    /// <returns>旋转后的向量</returns>
    public static Vector2 RotationMatrix(Vector2 v, float angle)
    {
        var x = v.x;
        var y = v.y;
        var sin = Mathf.Sin(Mathf.PI * angle / 180);
        var cos = Mathf.Cos(Mathf.PI * angle / 180);
        var newX = x * cos + y * sin;
        var newY = x * -sin + y * cos;
        return new Vector2((float)newX, (float)newY);
    }

    /// <summary>
    /// 旋转向量，使其方向改变，大小不变
    /// </summary>
    /// <param name="v">需要旋转的向量</param>
    /// <param name="angle">旋转的角度</param>
    /// <returns>旋转后的向量</returns>
    public static Vector3 RotationMatrixV3(Vector3 v, float angle)
    {
        var x = v.x;
        var y = v.y;
        var sin = Mathf.Sin(Mathf.PI * angle / 180);
        var cos = Mathf.Cos(Mathf.PI * angle / 180);
        var newX = x * cos + y * sin;
        var newY = x * -sin + y * cos;
        return new Vector3((float)newX, (float)newY, v.z);
    }

    //Vector2 WorldToUIPoint(Vector3 v3WorldPos)
    //{
    //    Vector2 pos;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("LayoutMgr").GetComponent<Canvas>().transform as RectTransform,
    //        ProCamera2D.Instance.GetComponent<Camera>().WorldToScreenPoint(v3WorldPos), GameObject.Find("LayoutMgr").GetComponent<Canvas>().worldCamera, out pos);
    //    return pos;
    //}
}

