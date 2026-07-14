using System;
using System.Threading;
using CommandLine;
using NLog;

namespace ET
{
    public enum OutputType
    {
        Server,
        Client,
    }
    internal static class Program
    {

        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error(e.ExceptionObject.ToString());
            };

            ETTask.ExceptionHandler += Log.Error;
            
            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
			
            try
            {		
                Game.EventSystem.Add(typeof(Game).Assembly);
				
                ProtobufHelper.Init();
                MongoRegister.Init();

                //输出到服务器/客户端
                //int index = args[0].IndexOf(':');
                //string outputTypeStr = args[0].Substring(index + 1);
                //OutputType outputType = (OutputType)Enum.Parse(typeof(OutputType), outputTypeStr);
                //args[0] = args[0].Replace(':' + outputTypeStr, "");

                // 命令行参数
                Options options = null;
                Parser.Default.ParseArguments<Options>(args)
                        .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
                        .WithParsed(o => { options = o; });

                Options.Instance = options;

                Log.ILog = new NLogger(Game.Options.AppType.ToString());
                LogManager.Configuration.Variables["appIdFormat"] = $"{Game.Options.Process:000000}";
				
                Log.Info($"server start........................ {Game.Scene.Id}");
				
                switch (Game.Options.AppType)
                {
                    case AppType.ExcelExporter:
                    {
                        Game.Options.Console = 1;
                        ExcelExporter.Export();
                        return 0;
                    }
                    case AppType.Proto2CSBatches:
                    {
                        Game.Options.Console = 1;
                        Proto2CSBatches.Export(OutputType.Client);
                        Proto2CSBatches.Export(OutputType.Server);
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Console(e.ToString());
            }
            return 1;
        }
    }
}