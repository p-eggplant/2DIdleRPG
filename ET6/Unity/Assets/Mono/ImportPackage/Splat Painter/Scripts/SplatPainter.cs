//******************************************************************
// 文件名:	SplatPainter.cs
// 版  权:	深圳热区网络科技有限公司(C)
// 创建人:	郑长基
// 日  期:	
// 版  本:	
// 描  述:	
// 应  用:  

//************************** 修改记录 ******************************
// 修改人: 
// 日  期: 
// 描  述: 
//******************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatPainter : MonoBehaviour
{

    private void Awake()
    {
        MeshCollider pMeshCollider = this.GetComponent<MeshCollider>();
        if (pMeshCollider != null)
        {
            pMeshCollider.enabled = false;
        }
    }





}
