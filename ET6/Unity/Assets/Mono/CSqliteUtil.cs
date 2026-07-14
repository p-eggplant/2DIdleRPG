//******************************************************************
// 文件名:	CSqliteUtil.cs
// 版  权:	深圳热区网络科技有限公司(C)
// 创建人:	郑长基
// 日  期:	
// 版  本:	
// 描  述:	SQL工具
// 应  用:  

//************************** 修改记录 ******************************
// 修改人: 
// 日  期: 
// 描  述: 
//******************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

/// <summary>
/// 数据相关定义
/// </summary>
public class DSQLDefine
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum eValueType
    {
        Int,
        Long,
        String,
        Bytes,
    }

    #region SQL属性类型定义
    /// <summary>
    /// int型数据
    /// </summary>
    public static readonly string SQLValue_Int = "INTEGER";            //int

    /// <summary>
    /// long数据
    /// </summary>
    public static readonly string SQLValue_Long = "BIGINT";            //long

    /// <summary>
    /// string型数据
    /// </summary>
    public static readonly string SQLValue_String = "TEXT";            //string

    /// <summary>
    /// byte[]型数据
    /// </summary>
    public static readonly string SQLValue_Bytes = "BLOB";             //byte[]
    #endregion
}

/// <summary>
/// sqlite属性格数据
/// </summary>
public struct sSQLFieldData
{
    public sSQLFieldData(DSQLDefine.eValueType e, object p)
    {
        eType = e;
        pData = p;
    }
    public DSQLDefine.eValueType eType;     //数据类型
    public object pData;                    //数据
}


public class CSqliteUtil 
{
    #region 数据库操作方法
    /// <summary>
    /// 表是否存在
    /// </summary>
    /// <param name="pDB">db</param>
    /// <param name="szTableName">表名</param>
    /// <returns></returns>
    public static bool IsTableExist(SQLiteDB pDB, string szTableName)
    {
        if (pDB == null)
        {
            return false;
        }
        return pDB.GetDB().listTableName.Contains(szTableName);
    }

    /// <summary>
    /// 插入值
    /// </summary>
    /// <param name="pDB">db对象</param>
    /// <param name="szTableName">表名</param>
    /// <param name="valueType">数据类型枚举数组</param>
    /// <param name="valueName">数据类型名字数组</param>
    /// <param name="values">数据列表，一个数组构成一行数据</param>
    /// <returns></returns>
    public static bool InsertValue(SQLiteDB pDB, string szTableName,DSQLDefine.eValueType[] valueType, string[] valueName, List<sSQLFieldData[]> values)
    {
        if (pDB == null)
        {
            return false;
        }

        //判断是否需要创建表格
        if (IsTableExist(pDB, szTableName) == false)
        {
            string[] types = new string[valueType.Length];
            for (int i = 0; i < valueType.Length; i++)
            {
                types[i] = ConverValueType2String(valueType[i]);
            }
            if (CreateTable(pDB, szTableName, types, valueName) == false)
            {
                return false;
            }
        }

        //拼装字符
        string szValue = valueName[0];
        for (int i = 1; i < valueName.Length; i++)
        {
            szValue += ", " + valueName[i];
        }
        string szValueField = "?";
        for (int i = 1; i < valueName.Length; i++)
        {
            szValueField += ",?";

        }
        string queryInsert = "INSERT INTO " + szTableName + " (" + szValue + ") VALUES(" + szValueField + ");";

        //开启事务,批量写入数据
        SQLiteQuery qrTransaction = new SQLiteQuery(pDB, "");
        qrTransaction.BeginTransaction();

        SQLiteQuery qr = new SQLiteQuery(pDB, queryInsert);
        for (int i = 0; i < values.Count; i++)
        {
            foreach (sSQLFieldData pNode in values[i])
            {
                if (pNode.eType == DSQLDefine.eValueType.Int)
                {
                    qr.Bind((int)pNode.pData);
                }
                else if (pNode.eType == DSQLDefine.eValueType.String)
                {
                    qr.Bind((string)pNode.pData);
                }
                else if (pNode.eType == DSQLDefine.eValueType.Bytes)
                {
                    qr.Bind((byte[])pNode.pData);
                }
                else if (pNode.eType == DSQLDefine.eValueType.Long)
                {
                    qr.Bind((long)pNode.pData);
                }
            }
            qr.Step();
            qr.Reset();
        }
        qr.Release();

        //关闭事务
        qrTransaction.EndTransaction();
        qrTransaction.Release();
        return true;
    }


    /// <summary>
    /// 获取表格Query
    /// </summary>
    public static SQLiteQuery GetTableQuery(SQLiteDB pDB , string szTableName)
    {
        string szQuery = "SELECT * FROM " + szTableName + ";";
        SQLiteQuery pQuery = new SQLiteQuery(pDB, szQuery);
        return pQuery;
    }
    #endregion


    /// <summary>
    /// 生成table
    /// </summary>
    /// <param name="pDB">db对象</param>
    /// <param name="szTableName">表名</param>
    /// <param name="valueNames">表格一行中所包含的属性类型</param>
    /// <param name="valueTypes">表格一行中所包含的属性名</param>
    private static bool CreateTable(SQLiteDB pDB, string szTableName, string[] valueTypes, string[] valueNames)
    {
        if (pDB == null)
        {
            return false;
        }

        //命令字符拼接
        string queryCreate = "CREATE TABLE IF NOT EXISTS " + szTableName + "( " + valueNames[0] + " " + valueTypes[0];
        for (int i = 1; i < valueNames.Length; i++)
        {
            queryCreate += ", " + valueNames[i] + " " + valueTypes[i];
        }
        queryCreate += "  ) ";

        SQLiteQuery qr = new SQLiteQuery(pDB, queryCreate);
        qr.Step();
        qr.Release();
        return true;
    }


    /// <summary>
    /// 根据SQL数据类型，返回对应的字符串
    /// </summary>
    /// <param name="eType"></param>
    /// <returns></returns>
    private static string ConverValueType2String(DSQLDefine.eValueType eType)
    {
        switch (eType)
        {
            case DSQLDefine.eValueType.Int:
                return DSQLDefine.SQLValue_Int;
            case DSQLDefine.eValueType.Long:
                return DSQLDefine.SQLValue_Long;
            case DSQLDefine.eValueType.String:
                return DSQLDefine.SQLValue_String;
            case DSQLDefine.eValueType.Bytes:
                return DSQLDefine.SQLValue_Bytes;
        }
        return string.Empty;
    }


}

