/*----------------------------------------------------------------
* 文件名:	Proto2CSBatches
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2022/12/28 14:51:11
* 创建人:   黎晨希
* 描  述:	Proto合并导出，能够根据proto文件的名字，将多个proto文件进行合并、生成cs代码。
* 
*           使用规则：1.为不同的系统创建不同的InnerMessage_XXX.proto、OuterMessage_XXX.proto、MongoMessage_XXX.proto文件。
*                    2.文件命名规则如：OuterMessage_Bag.proto（即后缀加“_系统名”）。
*                    3.自己创建的.proto文件，要从第一行就开始写消息类，不要加版本头 syntax = "proto3";package ET;
*           
*           报错功能：检测各种书写规范性，是否有重复定义的消息名和结构体，
*                    消息名所属类型是否正确，引用的自定义结构类是否存在，
*                    发送消息类是否能找到响应它的回复消息类，
*                    是否有回复消息类 没有其对应的发送消息类。
*                    根据错误能检索到其所属文件名、消息名、字段名或行内容。
*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ET
{
    public static class Proto2CSBatches
    {
        private const string importPath = "../../proto/";
        private const string clientMessagePath = "../Unity/Codes/Model/Generate/Message/";
        private const string serverMessagePath = "../Server/Model/Generate/Message/";
        private static readonly char[] splitChars = { ' ', '\t' };
        private static readonly List<OpcodeInfo> msgOpcode = new List<OpcodeInfo>();
        //字典<Proto类型名，该类型的所有Proto文件的路径>
        private static readonly Dictionary<string, List<string>> protoFilePathDic = new Dictionary<string, List<string>>();
        //队列 记录当前proto类型下待导出的模块的文件名
        private static readonly Queue<string> waitForExProtoNameQue = new Queue<string>();
        //队列 记录当前proto类型下待导出的模块合并时的起始行号
        private static readonly Queue<int> waitForExLineNumQue = new Queue<int>();
        //当前正在解析的消息名
        private static string crtExMsgName = "";

        //用于检查的变量
        //字典<responseType后接的消息名, 所在文件名>
        private static readonly Dictionary<string, string> responseTypeDic = new Dictionary<string, string>();
        //字典<回复消息类名, 所在文件名>
        private static readonly Dictionary<string, string> responseMsgNameDic = new Dictionary<string, string>();
        //字典<所有消息类名, 所在文件名>
        private static readonly Dictionary<string, string> allMsgPosDic = new Dictionary<string, string>();
        //字典<自定义结构类类名, 所在文件名>
        private static readonly Dictionary<string, string> structMsgDic = new Dictionary<string, string>();
        //记录基础类型的列表
        private static readonly List<string> basicTypeList = new List<string>() { "string", "float", "Entity", "Unit" };
        //字典<引用了的自定义结构类类名, 列表<引用该结构类的消息类名>>
        private static readonly Dictionary<string, List<string>> allUseStructMsgDic = new Dictionary<string, List<string>>();

        public static void Export(OutputType outputType)
        {
            responseTypeDic.Clear();
            responseMsgNameDic.Clear();
            allMsgPosDic.Clear();
            allUseStructMsgDic.Clear();
            structMsgDic.Clear();

            //按照Proto类型（Inner、Outer、Mongo），记录相应Proto文件的路径到列表字典
            bool isSucceed = GetFilePath();
            if (!isSucceed)
                return;

            //将所有系统的Proto文件合并到主Proto文件，并导出
            if (outputType == OutputType.Server)
            {
                BatchesAndExport("ET", "InnerMessage", serverMessagePath, "InnerOpcode", OpcodeRangeDefine.InnerMinOpcode);

                BatchesAndExport("ET", "MongoMessage", serverMessagePath, "MongoOpcode", OpcodeRangeDefine.MongoMinOpcode);

                BatchesAndExport("ET", "OuterMessage", serverMessagePath, "OuterOpcode", OpcodeRangeDefine.OuterMinOpcode);
            }
            else if (outputType == OutputType.Client)
            {
                //BatchesAndExport("ET", "OuterMessage", clientMessagePath, "OuterOpcode", OpcodeRangeDefine.OuterMinOpcode, false);
                BatchesAndExport("ET", "OuterMessage", clientMessagePath, "OuterOpcode", OpcodeRangeDefine.OuterMinOpcode);
            }

            //导出后的全局检查
            isSucceed = ExportEndCheck();
            if (!isSucceed)
                return;

            Log.Console("Proto2CSBatches Sucess!");
        }

        /// <summary>
        /// 获取Proto文件夹下的有效文件路径
        /// </summary>
        private static bool GetFilePath()
        {
            //Proto文件夹下所有文件的路径
            string[] pAllFilePath = Directory.GetFiles(importPath);

            protoFilePathDic.Clear();
            //根据Proto的类型（Inner、Outer、Mongo）将其记录到字典的列表中
            //优先记录主Proto文件路径 到列表
            string tempFileName = null;
            for (int i = 0; i < pAllFilePath.Length; i++)
            {
                //当前文件的名字
                tempFileName = Path.GetFileNameWithoutExtension(pAllFilePath[i]);
                switch (tempFileName)
                {
                    case "InnerMessage":
                    case "MongoMessage":
                    case "OuterMessage":
                        //将Proto文件路径 以Proto类型名为key 记录到列表字典
                        if (protoFilePathDic.ContainsKey(tempFileName))
                        {
                            Log.Console($"只能存在一个：{tempFileName}.proto");
                            return false;
                        }
                        List<string> filePathList = new List<string> { pAllFilePath[i] };
                        protoFilePathDic.Add(tempFileName, filePathList);
                        break;
                    default:
                        continue;
                }
            }

            //遍历所有文件，根据Proto类型（Inner、Outer、Mongo）记录系统模块的文件路径
            for (int i = 0; i < pAllFilePath.Length; i++)
            {
                //剔除扩展名不是.proto的文件路径
                if (!Path.GetExtension(pAllFilePath[i]).Equals(".proto"))
                    continue;
                //当前文件的名字
                tempFileName = Path.GetFileNameWithoutExtension(pAllFilePath[i]);
                //剔除主Proto的文件路径
                if (protoFilePathDic.ContainsKey(tempFileName))
                    continue;

                //Proto文件名包含“_”则为某个系统的Proto文件
                if (tempFileName.Contains('_'))
                {
                    int length = tempFileName.IndexOf('_');
                    string messageStr = tempFileName.Substring(0, length);
                    //将Proto文件路径 以Proto类型名记录到列表
                    if (protoFilePathDic.ContainsKey(messageStr))
                    {
                        protoFilePathDic[messageStr].Add(pAllFilePath[i]);
                    }
                    else
                    {
                        Log.Console($"找不到主Proto文件：{messageStr}.proto");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 将所有系统的.proto合并到指定的主Proto中
        /// </summary>
        private static void BatchesAndExport(string ns, string protoTypeName, string outputPath, string opcodeClassName, int startOpcode, bool isCheckMsgName = true)
        {
            //将相同Proto类型的文件内容合并到一个字符串
            StringBuilder protoAllContentSb = new StringBuilder();
            int nextProtoStartLineNum = 1;
            foreach (var protoFilePath in protoFilePathDic[protoTypeName])
            {
                //逐行合并数据，同时在切换系统模块时记录好 系统名字和起始行号
                string[] allContent = File.ReadAllLines(protoFilePath);
                for (int i = 0; i < allContent.Length; i++)
                {
                    protoAllContentSb.Append(allContent[i]);
                    protoAllContentSb.Append("\n");
                }
                protoAllContentSb.Append("\n");
                //记录当前光标所在行号
                nextProtoStartLineNum += allContent.Length + 1;
                //合并完当前系统后，将其存入队列
                string protoFileName = Path.GetFileNameWithoutExtension(protoFilePath);
                waitForExProtoNameQue.Enqueue(protoFileName);
                waitForExLineNumQue.Enqueue(nextProtoStartLineNum);
            }

            //测试函数：检查合并结果（在Proto文件夹下）
            //TestBatches(protoTypeName, protoAllContentSb.ToString());

            //将合并后的Proto文件整体转换为cs文件导出
            msgOpcode.Clear();
            Ex(ns, protoTypeName, protoAllContentSb.ToString(), outputPath, opcodeClassName, startOpcode, isCheckMsgName);
            GenerateOpcode(ns, opcodeClassName, outputPath);
        }

        //输出三个主文本（用于测试，比对是否合并成功）
        private static void TestBatches(string protoTypeName, string content)
        {
            using FileStream txt = new FileStream("../Proto/" + protoTypeName + "Batches.proto", FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter sw = new StreamWriter(txt);
            string str = content;
            sw.Write(str);
        }

        /// <summary>
        /// Proto文件生成cs代码
        /// </summary>
        /// <param name="ns">命名空间</param>
        /// <param name="protoTypeName">proto主文件类型</param>
        /// <param name="protoContent">proto主文件内容</param>
        /// <param name="outputPath">输出路径</param>
        private static void Ex(string ns, string protoTypeName, string protoContent, string outputPath, string opcodeClassName, int startOpcode, bool isCheckMsgName = true)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            msgOpcode.Clear();
            string csPath = Path.Combine(outputPath, protoTypeName + ".cs");
            string s = protoContent;

            StringBuilder sb = new StringBuilder();
            sb.Append("using ET;\n");
            sb.Append("using ProtoBuf;\n");
            sb.Append("using System.Collections.Generic;\n");
            sb.Append($"namespace {ns}\n");
            sb.Append("{\n");

            bool isMsgStart = false;
            //标记该message是否写了头尾括号“{”和“}”
            bool isHaveBracketHead = false;
            bool isHaveBracketTail = true;
            int crtLineNum = 0;
            //Log.Console($"正在导出：{waitForExProtoNameQue.Peek()}");
            foreach (string line in s.Split('\n'))
            {
                crtLineNum++;
                //当前行是否已是下一系统模块的Proto内容
                if (crtLineNum >= waitForExLineNumQue.Peek())
                {
                    waitForExProtoNameQue.Dequeue();
                    waitForExLineNumQue.Dequeue();
                    //if (waitForExLineNumQue.Count > 0)
                    //{
                    //    Log.Console($"正在导出：{waitForExProtoNameQue.Peek()}");
                    //}
                    //else
                    //{
                    //    Log.Console($"{protoTypeName} 导出成功！");
                    //}
                }

                string newline = line.Trim();

                if (newline == "")
                {
                    continue;
                }

                if (newline.StartsWith("//ResponseType"))
                {
                    string responseType = line.Split(" ")[1].TrimEnd('\r', '\n');
                    sb.AppendLine($"\t[ResponseType(nameof({responseType}))]");

                    //记录该responseType的消息名，便于检查该responseType是否存在
                    if (!responseTypeDic.ContainsKey(responseType))
                        responseTypeDic.Add(responseType, waitForExProtoNameQue.Peek());

                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    if (newline.Contains("ResponseType"))
                    {
                        Log.Console($"{waitForExProtoNameQue.Peek()} 警告！可能为 {crtExMsgName} 的下一个消息 警告原因：注释中出现ResponseType，但该行无法被解析");
                    }

                    sb.Append($"{newline}\n");
                    continue;
                }

                if (newline.StartsWith("message"))
                {
                    //上一个消息是否有后花括号
                    if (!isHaveBracketTail)
                        Log.Console($"{waitForExProtoNameQue.Peek()}（该文件或其上一个文件）错误！消息名：{crtExMsgName} 错误原因：该消息缺少" + "“}”号");
                    isHaveBracketTail = false;

                    string parentClass = "";
                    isMsgStart = true;
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    crtExMsgName = msgName;
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }
                    else if (ss.Length != 1)
                    {
                        Log.Console($"{waitForExProtoNameQue.Peek()} 错误！消息名：{msgName} 错误原因：message行书写不规范，无法解析");
                    }

                    if (ss[0].Split(splitChars, StringSplitOptions.RemoveEmptyEntries).Length != 2)
                    {
                        Log.Console($"{waitForExProtoNameQue.Peek()} 错误！错误原因：message行书写不规范\n错误行内容：{newline}");
                    }

                    msgOpcode.Add(new OpcodeInfo() { Name = msgName, Opcode = ++startOpcode });

                    sb.Append($"\t[Message({opcodeClassName}.{msgName})]\n");
                    sb.Append($"\t[ProtoContract]\n");
                    sb.Append($"\tpublic partial class {msgName}: Object");
                    if (parentClass == "IActorMessage" || parentClass == "IActorRequest" || parentClass == "IActorResponse")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass == "IActorLocationMessage" || parentClass == "IActorLocationRequest" || parentClass == "IActorLocationResponse")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass == "IActorCenterMessage" || parentClass == "IActorCenterRequest" || parentClass == "IActorCenterResponse")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass == "IMessage" || parentClass == "IRequest" || parentClass == "IResponse")
                    {
                        sb.Append($", {parentClass}\n");
                    }
                    else if (parentClass != "")
                    {
                        sb.Append($", {parentClass}\n");
                        Log.Console($"{waitForExProtoNameQue.Peek()} 警告！消息名：{msgName} 警告原因：message行的消息类型注释无法识别");
                    }
                    else
                    {
                        sb.Append("\n");
                        //记录出来，以防它是消息类型而不是结构类（防止message行注释遗漏错误）
                        //Log.Console($"在{waitForExProtoNameQue.Peek()} 中解析到结构类：{msgName}");

                        if (isCheckMsgName)
                        {
                            //记录自定义结构类所在文件名（防止在多个文件中被定义）
                            if (structMsgDic.ContainsKey(msgName))
                                Log.Console($"重复定义结构体错误！结构体 {msgName} 在 {structMsgDic[msgName]} 中已被定义，但在 {waitForExProtoNameQue.Peek()} 中又被重复定义");
                            else
                                structMsgDic.Add(msgName, waitForExProtoNameQue.Peek());
                        }
                    }

                    if (isCheckMsgName)
                    {
                        //记录回复消息类名及其所在文件名（用于ResponseType对应消息类名的匹配检测）
                        if (parentClass.EndsWith("Response") && !responseMsgNameDic.ContainsKey(msgName))
                        {
                            responseMsgNameDic.Add(msgName, waitForExProtoNameQue.Peek());
                        }

                        //记录所有消息类名及其所在文件名（防止在多个文件中被定义）
                        if (allMsgPosDic.ContainsKey(msgName))
                            Log.Console($"重复定义消息类错误！消息类 {msgName} 在 {allMsgPosDic[msgName]} 中已被定义，但在 {waitForExProtoNameQue.Peek()} 中又被重复定义");
                        else
                            allMsgPosDic.Add(msgName, waitForExProtoNameQue.Peek());
                    }

                    continue;
                }

                if (isMsgStart)
                {
                    if (newline == "{")
                    {
                        isHaveBracketHead = true;
                        sb.Append("\t{\n");
                        continue;
                    }

                    if (newline == "}")
                    {
                        isMsgStart = false;

                        isHaveBracketTail = true;
                        //该消息是否有前花括号
                        if (!isHaveBracketHead)
                            Log.Console($"{waitForExProtoNameQue.Peek()} 错误！消息名：{crtExMsgName} 错误原因：该消息缺少" + "“{”号");
                        isHaveBracketHead = false;

                        sb.Append("\t}\n\n");
                        continue;
                    }

                    if (newline.Trim().StartsWith("//"))
                    {
                        sb.AppendLine(newline);
                        continue;
                    }

                    if (newline.Trim() != "" && newline != "}")
                    {
                        if (newline.StartsWith("repeated"))
                        {
                            Repeated(sb, ns, newline);
                        }
                        else
                        {
                            Members(sb, newline, true);
                        }
                    }
                }
            }

            sb.Append("}\n");
            using FileStream txt = new FileStream(csPath, FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter sw = new StreamWriter(txt);
            sw.Write(sb.ToString());
        }

        private static void GenerateOpcode(string ns, string outputFileName, string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {ns}");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic static partial class {outputFileName}");
            sb.AppendLine("\t{");
            foreach (OpcodeInfo info in msgOpcode)
            {
                sb.AppendLine($"\t\t public const ushort {info.Name} = {info.Opcode};");
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            string csPath = Path.Combine(outputPath, outputFileName + ".cs");

            using FileStream txt = new FileStream(csPath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(txt);
            sw.Write(sb.ToString());
        }

        private static void Repeated(StringBuilder sb, string ns, string newline)
        {
            try
            {
                int index = newline.IndexOf(";");
                if (index >= 0)
                    newline = newline.Remove(index);
                //标记是否写了“;”号
                bool isHaveEndSign = index >= 0;
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[1];
                type = ConvertType(type);
                string name = ss[2];
                if (ss.Length != 5)
                {
                    Log.Console($"{waitForExProtoNameQue.Peek()} 错误！消息名：{crtExMsgName} 错误原因：repeated行 {name} 字段书写不规范，类型为 {type}");
                }

                if (!isHaveEndSign)
                {
                    Log.Console($"{waitForExProtoNameQue.Peek()} 警告！消息名：{crtExMsgName} 警告原因：{name} 字段没有“;”号");
                }
                int n = int.Parse(ss[4]);

                sb.Append($"\t\t[ProtoMember({n})]\n");
                sb.Append($"\t\tpublic List<{type}> {name} = new List<{type}>();\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"repeated异常！请检查端口号书写是否规范：{waitForExProtoNameQue.Peek()} 消息名：{crtExMsgName}\n异常行内容：{newline}\n {e}");
            }
        }

        private static string ConvertType(string type)
        {
            string typeCs = "";
            switch (type)
            {
                case "int16":
                    typeCs = "short";
                    break;
                case "int32":
                    typeCs = "int";
                    break;
                case "bytes":
                    typeCs = "byte[]";
                    break;
                case "uint32":
                    typeCs = "uint";
                    break;
                case "long":
                    typeCs = "long";
                    break;
                case "int64":
                    typeCs = "long";
                    break;
                case "uint64":
                    typeCs = "ulong";
                    break;
                case "uint16":
                    typeCs = "ushort";
                    break;
                case "float":
                    typeCs = "float";
                    break;
                case "double":
                    typeCs = "double";
                    break;
                default:
                    typeCs = type;
                    //记录引用了该自定义结构类的消息名
                    if (!basicTypeList.Contains(type))
                    {
                        if (allUseStructMsgDic.ContainsKey(type))
                            allUseStructMsgDic[type].Add(crtExMsgName);
                        else
                            allUseStructMsgDic.Add(type, new List<string>() { crtExMsgName });
                    }
                    break;
            }

            return typeCs;
        }

        private static void Members(StringBuilder sb, string newline, bool isRequired)
        {
            try
            {
                int index = newline.IndexOf(";");
                if (index >= 0)
                    newline = newline.Remove(index);
                //标记是否写了“;”号
                bool isHaveEndSign = index >= 0;
                string[] ss = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                string type = ss[0];
                string typeCs = ConvertType(type);
                string name = ss[1];
                if (ss.Length != 4)
                {
                    Log.Console($"{waitForExProtoNameQue.Peek()} 错误！消息名：{crtExMsgName} 错误原因：{name} 字段书写不规范，类型为 {type}");
                }

                if (!isHaveEndSign)
                {
                    Log.Console($"{waitForExProtoNameQue.Peek()} 警告！消息名：{crtExMsgName} 警告原因：{name} 字段没有“;”号");
                }
                int n = int.Parse(ss[3]);

                sb.Append($"\t\t[ProtoMember({n})]\n");
                sb.Append($"\t\tpublic {typeCs} {name} {{ get; set; }}\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"某字段异常！请检查端口号书写是否规范：{waitForExProtoNameQue.Peek()} 消息名：{crtExMsgName}\n异常行内容：{newline}\n {e}");
            }
        }

        /// <summary>
        /// 导出后的全局检查
        /// </summary>
        /// <returns>是否无误</returns>
        private static bool ExportEndCheck()
        {
            bool isOk = true;
            try
            {
                //检查引用的自定义结构类是否存在
                foreach (var nowUseStructMsgPair in allUseStructMsgDic)
                {
                    string nowUseStructName = nowUseStructMsgPair.Key;
                    //若自定义结构类不存在
                    if (!structMsgDic.ContainsKey(nowUseStructName))
                    {
                        isOk = false;
                        foreach (var nowUseStructMsgName in nowUseStructMsgPair.Value)
                        {
                            string nowMsgfileName = allMsgPosDic[nowUseStructMsgName];
                            Log.Console($"找不到自定义结构类 {nowUseStructName} 错误！引用文件名：{nowMsgfileName} 消息类名：{nowUseStructMsgName}");
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Console("检测引用的自定义结构类是否存在时异常：\n " + e);
            }

            //检查发送消息类是否能找到响应它的回复消息类
            foreach (var responseTypePair in responseTypeDic)
            {
                string responseTypeName = responseTypePair.Key;
                //若全局都找不到和//ResponseType后接的消息类名 相同的 回复消息类名
                if (!responseMsgNameDic.ContainsKey(responseTypeName))
                {
                    isOk = false;
                    Log.Console($"找不到和//ResponseType {responseTypeName} 对应的回复消息类！该请求消息类所在文件名：{responseTypePair.Value}");
                }
            }

            //检查是否有回复消息类 没有其对应的发送消息类
            foreach (var responseMsgNamePair in responseMsgNameDic)
            {
                string responseMsgName = responseMsgNamePair.Key;

                if (!responseTypeDic.ContainsKey(responseMsgName))
                {
                    isOk = false;
                    Log.Console($"找不到和 {responseMsgName} 对应的请求消息类！该回复消息类所在文件名：{responseMsgNamePair.Value}");
                }
            }

            return isOk;
        }
    }
}
