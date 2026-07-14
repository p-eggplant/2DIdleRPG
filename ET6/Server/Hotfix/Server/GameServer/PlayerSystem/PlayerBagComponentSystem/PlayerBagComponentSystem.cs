/*----------------------------------------------------------------
* 文件名:	PlayerBagComponentSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 20:48:17
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
    [FriendClassAttribute(typeof(PlayerBagComponent))]

    // 构造函数
    public class PlayerBagComponent_IAwake : AwakeSystem<PlayerBagComponent>
    {
        public override void Awake(PlayerBagComponent self)
        {
            // 清理总数组
            self.m_dicBag.Clear();

        }
    }

    // 析构函数
    public class PlayerBagComponent_IDestroy : DestroySystem<PlayerBagComponent>
    {
        public override void Destroy(PlayerBagComponent self)
        {
            // 清理总数组
            self.m_dicBag.Clear();

        }
    }



    [FriendClassAttribute(typeof(PlayerBagComponent))]
    public static class PlayerBagComponentSystem
    {

        /// <summary>
        /// 如果背包中不存在该configId，则返回0
        /// </summary>
        public static int GetItemNum(this PlayerBagComponent self, int nConfigId)
        {
            int nNum = 0;
            self.m_dicBag.TryGetValue(nConfigId, out nNum);
            return nNum;
        }


        /// <summary>
        /// 判断是否能够添加物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nConfigId">物资ID</param>
        /// <param name="nNum">物资数量</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool isCanAddItem(this PlayerBagComponent self, int nConfigId, int nNum)
        {
            //检查背包配置Id是否存在，num值是否合规
            if (nNum <= 0)
            {
                return false;
            }

            // 判断配置ID是否存在
            if (MaterialConfigCategory.Instance.Contain(nConfigId) == false)
            {
                throw new Exception("不存在的物资ID configId =" + nConfigId);
            }

            // 判断有无越界
            int nHaveNum = 0;
            if (true == self.m_dicBag.TryGetValue(nConfigId, out nHaveNum))
            {
                if (int.MaxValue - nNum <= nHaveNum)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 执行添加物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId">物资ID</param>
        /// <param name="num">物资数量</param>
        /// <param name="eOSSType">OSS来源</param>
        /// <exception cref="Exception"></exception>
        public static void AddItem(this PlayerBagComponent self, int configId, int num, EOssType eOSSType, string OssReason)
        {
            //检查背包配置Id是否存在，num值是否合规
            if (num <= 0)
            {
                throw new Exception("非法的数量 num=" + num + " configId =" + configId);
            }

            // 判断物资ID是否存在
            if (MaterialConfigCategory.Instance.Contain(configId) == false)
            {
                throw new Exception("不存在的物资ID configId =" + configId);
            }

            int nHaveNum = 0;
            if (false == self.m_dicBag.TryGetValue(configId, out nHaveNum))
            {
                nHaveNum = num;
                self.m_dicBag.Add(configId, num);
            }
            else
            {

                // 判断是否越界
                if (int.MaxValue - num <= nHaveNum)
                {
                    throw new Exception("物资数量越界 configId =" + configId);
                }
                nHaveNum += num;
                self.m_dicBag[configId] = nHaveNum;
            }

            //下发更改的背包数据给客户端
            self.SendToClient(configId, nHaveNum);

            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(configId, num, nHaveNum, eOSSType, OssReason);

        }


        /// <summary>
        /// 批量添加物资（为了降低网络推送次数）
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dic">key = 物资ID， Value=物资数量</param>
        /// <param name="eOSSType">OSS来源</param>
        /// <exception cref="Exception"></exception>
        public static void AddItemBatch(this PlayerBagComponent self, Dictionary<int, int> dic, EOssType eOSSType, string OssReason)
        {
            if (dic.Count == 0)
            {
                throw new Exception("dic.Count == 0");
            }
            //检查背包配置Id是否存在，num值是否合规
            foreach (var item in dic)
            {
                if (item.Value <= 0)
                {
                    throw new Exception("非法的数量 num=" + item.Value + " configId =" + item.Key);
                }

                // 判断配置ID是否存在
                if (MaterialConfigCategory.Instance.Contain(item.Key) == false)
                {
                    throw new Exception("不存在的物资ID configId =" + item.Key);
                }

                
                int nHaveNum = 0;
                if (false == self.m_dicBag.TryGetValue(item.Key, out nHaveNum))
                {
                    nHaveNum = item.Value;
                    self.m_dicBag.Add(item.Key, nHaveNum);

                    // 记录OSS
                    self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(item.Key, nHaveNum, nHaveNum, eOSSType, OssReason);
                }

                else
                {
                    // 判断是否越界
                    if (int.MaxValue - item.Value <= nHaveNum)
                    {
                        throw new Exception("物资数量越界 configId =" + item.Key);
                    }

                    nHaveNum = nHaveNum + item.Value;
                    self.m_dicBag[item.Key] = nHaveNum;
                    dic[item.Key] = nHaveNum;
                    // 记录OSS
                    self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(item.Key, item.Value, nHaveNum, eOSSType, OssReason);
                }
            }

            //下发更改的背包数据给客户端
            self.SendToClient(dic);

            
        }



        /// <summary>
        /// 是否能够移除物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId">物资ID</param>
        /// <param name="num">物资数量</param>
        /// <returns></returns>
        public static bool isCanRemoveItem(this PlayerBagComponent self, int configId, int num)
        {
            //检查num值是否合规
            if (num <= 0)
            {
                return false;
            }

            // 查看备好是否有这个物品
            int nHaveNum = 0;
            if (false == self.m_dicBag.TryGetValue(configId, out nHaveNum))
            {
                return false;
            }

            // 判断数量是否可以足够扣除
            if (num > nHaveNum)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 执行移除物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId">物资ID</param>
        /// <param name="num">物资数量</param>
        /// <param name="eOSSType">OSS来源</param>
        /// <exception cref="Exception"></exception>
        public static void RemoveItem(this PlayerBagComponent self, int configId, int num, EOssType eOSSType, string OssReason)
        {
            //检查背包配置Id是否存在，num值是否合规
            if (num <= 0)
            {
                throw new Exception("非法的数量 num=" + num + " configId =" + configId);
            }


            int nHaveNum = 0;
            if (false == self.m_dicBag.TryGetValue(configId, out nHaveNum))
            {
                throw new Exception("背包里并不存在物品  configId =" + configId);

            }

            if (num > nHaveNum)
            {
                throw new Exception("数量不合法  num =" + num + "nHaveNum = " + nHaveNum);
            }


            nHaveNum = nHaveNum - num;
            if (nHaveNum > 0)
                self.m_dicBag[configId] = nHaveNum;
            else
            {
                self.m_dicBag.Remove(configId);
            }


            //下发更改的背包数据给客户端
            self.SendToClient(configId, nHaveNum);

            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(configId, -num, nHaveNum, eOSSType, OssReason);
        }


        /// <summary>
        /// 批量移除物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dic">key = 物资ID， Value=物资数量</param>
        /// <param name="eOSSType">OSS来源</param>
        /// <exception cref="Exception"></exception>
        public static void RemoveItemBatch(this PlayerBagComponent self, Dictionary<int, int> dic, EOssType eOSSType , string OssReason)
        {
            if (dic.Count == 0)
            {
                throw new Exception("dic.Count == 0");
            }

            foreach (var item in dic)
            {
                //检查背包配置Id是否存在，num值是否合规
                if (item.Value <= 0)
                {
                    throw new Exception("非法的数量 num=" + item.Value + " configId =" + item.Key);
                }


                int nHaveNum = 0;
                if (false == self.m_dicBag.TryGetValue(item.Key, out nHaveNum))
                {
                    throw new Exception("背包里并不存在物品  configId =" + item.Key);

                }

                if (item.Value > nHaveNum)
                {
                    throw new Exception("数量不合法  num =" + item.Value + "nHaveNum = " + nHaveNum);
                }


                nHaveNum = nHaveNum - item.Value;
                if (nHaveNum > 0)
                {
                    self.m_dicBag[item.Key] = nHaveNum;

                    // 记录Oss
                    self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(item.Key, -item.Value, nHaveNum, eOSSType, OssReason);
                }  
                else
                {
                    self.m_dicBag.Remove(item.Key);
                    self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(item.Key, -item.Value, nHaveNum, eOSSType, OssReason);
                }


                dic[item.Key] = nHaveNum;
            }
            //下发更改的背包数据给客户端
            self.SendToClient(dic);
        }

        /// <summary>
        /// 是否可以使用物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId">物资ID</param>
        /// <param name="num">使用数量</param>
        /// <returns></returns>
        public static bool isCanUseItem(this PlayerBagComponent self, int configId, int num)
        {
            //检查背包配置Id是否存在，num值是否合规
            if (num <= 0)
            {
                return false;
            }

            int nHaveNum = 0;
            if (false == self.m_dicBag.TryGetValue(configId, out nHaveNum))
            {
                return false;
            }

            if (num > nHaveNum)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 执行使用物资
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId">物资ID</param>
        /// <param name="num">物资数量</param>
        /// <param name="eOSSType">OSS来源</param>
        /// <exception cref="Exception"></exception>
        public static void UseItem(this PlayerBagComponent self, int configId, int num, EOssType eOSSType, string OssReason)
        {
            //检查背包配置Id是否存在，num值是否合规
            if (num <= 0)
            {
                throw new Exception("非法的数量 num=" + num + " configId =" + configId);
            }


            int nHaveNum = 0;
            if (false == self.m_dicBag.TryGetValue(configId, out nHaveNum))
            {
                throw new Exception("背包里并不存在物品  configId =" + configId);

            }

            if (num > nHaveNum)
            {
                throw new Exception("数量不合法  num =" + num + "nHaveNum = " + nHaveNum);
            }

            nHaveNum = nHaveNum - num;
            self.m_dicBag[configId] = nHaveNum;

           

            //下发更改的背包数据给客户端
            self.SendToClient(configId, nHaveNum);

            // 记录Oss
            self.Parent.GetComponent<PlayerOssComponent>()?.MaterialOss(configId, -num, nHaveNum, eOSSType, OssReason);

            // 使用物品函数以及事件

        }



        /// <summary>
        /// 修改单个物资时的通信下发
        /// </summary>
        private static void SendToClient(this PlayerBagComponent self, int configId, int bagNum)
        {
            Player player = self.GetParent<Player>();
            if (player == null)
            {
                throw new Exception("player == null");
            }
            //发送 改变的背包物资 消息 给客户端
            Game2C_BagChange game2CBagChange = new Game2C_BagChange();
            PlayerBagProto bagProto = new PlayerBagProto();
            bagProto.ConfigId = configId;
            bagProto.Num = bagNum;
            game2CBagChange.BagNodes.Add(bagProto);
            MessageHelper.SendToClient(player, game2CBagChange);

        }


        /// <summary>
        /// 修改多个物资时的通信下发
        /// </summary>
        private static void SendToClient(this PlayerBagComponent self, Dictionary<int, int> dicChangeItem)
        {
            Player player = self.GetParent<Player>();
            if (player == null)
            {
                throw new Exception("player == null");
            }

            //发送 改变的背包物资 消息 给客户端
            Game2C_BagChange game2CBagChange = new Game2C_BagChange();
            foreach (var item in dicChangeItem)
            {
                PlayerBagProto bagProto = new PlayerBagProto();
                bagProto.ConfigId = item.Key;
                bagProto.Num = item.Value;
                game2CBagChange.BagNodes.Add(bagProto);
            }
            MessageHelper.SendToClient(player, game2CBagChange);

        }


    }
}