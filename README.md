# ET6 Game Server Project

> 基于 ET6 框架的游戏服务端项目

```
project/
├── 导出Excel.bat                        # Excel配置导出脚本
├── 导出Proto.bat                        # Proto协议导出脚本
├── 启动服务器.bat                        # 服务器启动脚本
│
├── ET6/                                 # ET6框架核心目录
│   ├── Bin/                             # 编译输出与运行时依赖
│   │   ├── Server.exe                   # 服务端入口
│   │   ├── Tools.exe                    # 工具入口(Excel/Proto导出)
│   │   ├── CommandLine.dll
│   │   ├── DnsClient.dll
│   │   ├── EPPlus.dll                   # Excel解析库
│   │   ├── ICSharpCode.SharpZipLib.dll
│   │   ├── kcp.dll / libkcp.dylib       # KCP网络库
│   │   ├── MongoDB.Bson.dll              # MongoDB数据库驱动
│   │   ├── MongoDB.Driver.dll
│   │   ├── MongoDB.Driver.Core.dll
│   │   ├── MongoDB.Driver.Legacy.dll
│   │   ├── NLog.dll / NLog.config        # 日志组件
│   │   ├── protobuf-net.dll             # Protobuf序列化
│   │   ├── RecastDll.dll / libRecastDll.dylib  # Recast寻路库
│   │   ├── Microsoft.CodeAnalysis.dll   # Roslyn编译分析
│   │   ├── Microsoft.CodeAnalysis.CSharp.dll
│   │   ├── net6.0/                      # .NET 6运行时库
│   │   └── runtimes/                    # 跨平台原生库
│   │       ├── linux/native/libmongocrypt.so
│   │       ├── osx/native/libmongocrypt.dylib
│   │       ├── unix/lib/netcoreapp3.0/ / netstandard2.0/
│   │       └── win/lib/netcoreapp3.0/ / netstandard2.0/
│   │           └── win/native/libzstd.dll, mongocrypt.dll, snappy32.dll, snappy64.dll
│   │
│   ├── Config/                          # 服务器配置文件(二进制.bytes格式)
│   │   ├── AIConfigCategory.bytes         # AI配置
│   │   ├── ExampleConfigCategory.bytes    # 示例配置
│   │   ├── GameServerInfoConfigCategory.bytes
│   │   ├── MaterialConfigCategory.bytes   # 材料配置
│   │   ├── NerveConfigCategory.bytes      # 神经/天赋配置
│   │   ├── NerveLayerConfigCategory.bytes
│   │   ├── NerveMouldConfigCategory.bytes
│   │   ├── NerveTierConfigCategory.bytes
│   │   ├── PlayerDataConfigCategory.bytes
│   │   ├── PlayerPropertyConfigCategory.bytes
│   │   ├── RoleLevelUpConfigCategory.bytes
│   │   ├── StartMachineConfigCategory.bytes
│   │   ├── StartProcessConfigCategory.bytes
│   │   ├── StartSceneConfigCategory.bytes
│   │   ├── StartZoneConfigCategory.bytes
│   │   ├── TranslateCategory.bytes       # 翻译配置
│   │   ├── TranslateProcCategory.bytes
│   │   ├── UnitConfigCategory.bytes      # 单位配置
│   │   ├── Recast/Map1, Map2             # Recast寻路网格数据
│   │   └── StartConfig/                  # 多环境启动配置
│   │       ├── Localhost/                # 本地开发环境
│   │       └── Release/                  # 正式发布环境
│   │
│   ├── Libs/                            # 原生库源码
│   │   ├── Kcp/ikcp.c, ikcp.h           # KCP协议C源码
│   │   └── RecastDll/                   # Recast寻路库(C++)
│   │       ├── CMakeLists.txt
│   │       ├── make_*.sh / make_*.bat    # 跨平台编译脚本
│   │       ├── Include/InvokeHelper.h
│   │       └── Source/InvokeHelper.cpp
│   │
│   ├── Logs/                            # 服务器运行日志
│   │   └── Server.*.Debug/Info/Warn/Error.log
│   │
│   ├── Release/PC/StreamingAssets/      # PC客户端发布资源
│   │   ├── code.unity3d                  # 热更代码包
│   │   ├── config.unity3d                # 配置包
│   │   ├── map1.unity3d / map2.unity3d  # 地图资源
│   │   ├── shader                        # 着色器
│   │   ├── uihelp.unity3d               # UI-Help界面
│   │   ├── uilobby.unity3d              # UI-大厅
│   │   ├── uilogin.unity3d              # UI-登录
│   │   └── unit.unity3d                  # 单位模型
│   │
│   ├── Server/                          # 服务端C#项目
│   │   ├── App/                         # 服务端入口程序
│   │   │   ├── Program.cs               # 主入口
│   │   │   ├── Server.App.csproj
│   │   │   ├── App.config / NLog.config
│   │   │   └── Properties/
│   │   ├── Hotfix/                      # 热更新层(业务逻辑)
│   │   │   ├── AppStart_Init.cs         # 初始化入口
│   │   │   ├── Demo/                    # 示例玩法模块
│   │   │   │   ├── AOI/                 # AOI(AOIEntity, AOIManager, Cell)
│   │   │   │   ├── Move/                # 移动(MoveHelper, PathFindHelper)
│   │   │   │   ├── Scene/               # 场景切换(TransferMap, SceneFactory)
│   │   │   │   ├── Session/             # 会话分发
│   │   │   │   ├── Transfer/            # 传输(TransferHelper)
│   │   │   │   └── Unit/                # 单位管理(UnitSystem, UnitFactory)
│   │   │   ├── Module/                  # 通用模块
│   │   │   │   ├── Actor/               # Actor消息分发
│   │   │   │   ├── ActorLocation/       # Actor定位
│   │   │   │   ├── Console/             # 控制台命令
│   │   │   │   ├── DB/                  # 数据库(DBComponent, DBManager)
│   │   │   │   ├── Handler/             # 通用消息处理(C2G_Ping, C2M_Reload)
│   │   │   │   ├── Http/                # HTTP服务
│   │   │   │   └── MessageInner/        # 内部网管通信(NetInnerComponent)
│   │   │   └── Server/                  # 具体服务器进程逻辑
│   │   │       ├── CacheServer/         # 缓存服务器(UnitCache)
│   │   │       ├── GameServer/          # 游戏逻辑服务器
│   │   │       │   ├── Common/          # 公共(MessageHelper)
│   │   │       │   └── PlayerSystem/    # 玩家系统
│   │   │       │       ├── PlayerMgrComponentSystem
│   │   │       │       ├── PlayerBagComponentSystem/    # 背包
│   │   │       │       ├── PlayerDataComponentSystem/   # 数据
│   │   │       │       ├── PlayerExampleComponentSystem/# 示例
│   │   │       │       ├── PlayerOssComponentSystem/    # OSS运营
│   │   │       │       ├── PlayerPropertyComponentSystem/# 属性
│   │   │       │       └── PlayerRoleUpComponent/       # 角色升级
│   │   │       ├── GateServer/          # 网关服务器
│   │   │       ├── LoginServer/         # 登录服务器
│   │   │       └── Watcher/             # 守护进程
│   │   └── Model/                       # 模型层(数据结构与配置定义)
│   │       ├── Demo/                    # 示例模型(EPlayer, AOI, Unit, Scene)
│   │       ├── Generate/                # 自动生成代码
│   │       │   ├── Config/              # Excel配置对应的C#类
│   │       │   │   ├── AIConfig.cs, ExampleConfig.cs, ...
│   │       │   │   ├── NerveConfig.cs, MaterialConfig.cs
│   │       │   │   ├── PlayerDataConfig.cs, PlayerPropertyConfig.cs
│   │       │   │   ├── StartMachineConfig.cs, StartProcessConfig.cs
│   │       │   │   ├── StartSceneConfig.cs, StartZoneConfig.cs
│   │       │   │   └── UnitConfig.cs, Translate.cs
│   │       │   ├── ConfigPartial/       # 配置分部类(扩展方法)
│   │       │   └── Message/             # 协议消息类(Inner/Outer/Mongo)
│   │       ├── Module/                  # 通用模块定义
│   │       │   ├── ActorLocation/       # Actor定位模型
│   │       │   ├── DB/                  # 数据库模型(MongoDB)
│   │       │   ├── Http/                # HTTP模型
│   │       │   ├── NetworkTCP/          # TCP网络模型
│   │       │   ├── Console/             # 控制台模型
│   │       │   └── Base/                # 基础(DllHelper, MongoHelper)
│   │       └── Server/                  # 服务器进程模型定义
│   │           ├── CacheServer/         # (UnitCacheInit, UnitCacheManager, UnitCacheNode)
│   │           ├── GameServer/Player/   # 玩家相关组件(Player, PlayerBag, PlayerData, ...)
│   │           ├── GateServer/          # GateSessionKeyComponent
│   │           ├── LoginServer/         # 登录(Account, GenerateId)
│   │           └── Watcher/             # WatcherComponent
│   │
│   ├── Robot/                           # 机器人(压力测试/自动化测试)项目
│   │   ├── App/Program.cs               # 机器人入口
│   │   ├── Hotfix/                      # 机器人热更新层
│   │   │   ├── AppStart_Init.cs
│   │   │   └── Module/RobotCase/        # 测试用例
│   │   │       ├── RobotCaseComponentSystem.cs
│   │   │       ├── RobotCaseDispatcherComponentSystem.cs
│   │   │       └── RobotCaseSystem.cs
│   │   └── Model/                       # 机器人模型层
│   │       ├── Base/DllHelper.cs
│   │       ├── Module/RobotCase/        # (IRobotCase, RobotCase, RobotCaseComponent)
│   │       └── Robot/                   # (RobotManagerComponent, RobotCaseType)
│   │
│   ├── ThirdParty/                      # 第三方库源码项目
│   │   ├── ETTask/ETTask.csproj         # 异步任务框架
│   │   ├── protobuf-net/protobuf-net.csproj  # Protobuf序列化
│   │   ├── ShareLib/ShareLib.csproj     # 共享库
│   │   └── UnityEngine/                 # Unity数学库(Matrix/Vectors/Quaternion)
│   │
│   ├── Tools/                           # 开发工具项目
│   │   ├── Analyzer/                    # Roslyn代码分析器
│   │   │   ├── Analyzer.csproj
│   │   │   ├── Analyzer/                # 分析规则(Entity/ETTask/ClassDeclaration...)
│   │   │   ├── CodeFixer/               # 代码修复器
│   │   │   ├── Config/                  # 分析器配置
│   │   │   └── Extension/               # 扩展方法
│   │   ├── App/                         # 工具应用
│   │   │   ├── Program.cs               # 工具入口
│   │   │   ├── Apps/                    # 工具实现
│   │   │   │   ├── ExcelExporter/       # Excel导出器(ExcelExporter.cs)
│   │   │   │   ├── Proto2CS/            # Proto转C#(Proto2CS.cs)
│   │   │   │   └── Proto2CSBatches/     # 批量Proto转C#
│   │   │   └── Base/                    # (空)
│   │   ├── Config/                      # IDE配置(Eclipse/Rider/Unity/VS)
│   │   ├── cwRsync/                     # Rsync同步工具(Windows移植版)
│   │   │   ├── rsync.exe, ssh.exe, ssh-keygen.exe
│   │   │   └── Config/                  # rsync配置文件
│   │   └── RecastNavExportor/           # Recast寻路网格导出工具
│   │       ├── RecastDemo.exe, SDL2.dll
│   │       ├── DroidSans.ttf
│   │       ├── solo_navmesh.bin
│   │       └── Meshes/                  # 示例网格文件(.obj)
│   │
│   └── Unity/                           # Unity客户端项目
│       ├── Unity.sln                    # Unity解决方案文件
│       ├── Assets/                      # Unity资源目录
│       │   ├── Bundles/                 # 资源包(Bundles)
│       │   │   ├── Code/                # 热更代码包(code.dll)
│       │   │   ├── Config/              # 配置资源包
│       │   │   ├── dialog/              # 对话UI包
│       │   │   │   ├── dlg_common/
│       │   │   │   ├── dlg_gm/
│       │   │   │   ├── dlg_login/
│       │   │   │   ├── dlg_package/
│       │   │   │   ├── dlg_reconnection/
│       │   │   │   ├── dlg_role/
│       │   │   │   └── dlg_updata/
│       │   │   ├── resources/           # (空)
│       │   │   ├── sound/               # 音效(sound_updata_open.MP3)
│       │   │   └── UI/                  # UI预制体(Gm_*, UIHelp, UILobby)
│       │   ├── Editor/                  # Unity编辑器扩展
│       │   │   ├── AssetPostProcessor/
│       │   │   ├── BuildEditor/
│       │   │   ├── ComponentViewEditor/
│       │   │   ├── Helper/
│       │   │   ├── ILRuntimeEditor/
│       │   │   ├── RecastNavDataExporter/
│       │   │   ├── ReferenceCollectorEditor/
│       │   │   ├── ServerCommandLineEditor/
│       │   │   └── ToolEditor/
│       │   ├── Hotfix/                  # 客户端热更代码(ILRuntime)
│       │   ├── HotfixView/              # 客户端热更UI代码
│       │   ├── Model/                   # 客户端模型层
│       │   ├── ModelView/               # 客户端UI模型层
│       │   ├── Mono/                    # Unity Mono层(不热更)
│       │   │   ├── CodeLoader.cs        # 热更加载器
│       │   │   ├── Define.cs, Init.cs
│       │   │   ├── Core/                # 核心库(DoubleMap, MultiMap, ObjectPool...)
│       │   │   │   └── Helper/          # 工具类(Byte, Enum, File, Math, MD5...)
│       │   │   │   └── Log/             # 日志系统
│       │   │   ├── Helper/              # Mono层工具
│       │   │   ├── ILRuntime/           # ILRuntime适配
│       │   │   │   ├── Generate/        # CLR绑定生成代码
│       │   │   │   └── ValueTypeBinders/ # 值类型绑定(Quaternion, Vector2/3)
│       │   │   ├── ImportPackage/       # 导入的第三方插件
│       │   │   │   ├── ProCamera2D/
│       │   │   │   ├── Splat Painter/
│       │   │   │   └── xasset/
│       │   │   ├── Module/              # 核心模块
│       │   │   │   ├── Network/         # 网络(KChannel, KService, Circularbuffer)
│       │   │   │   ├── NetworkTCP/      # TCP网络
│       │   │   │   ├── Mongo/           # MongoDB Bson属性
│       │   │   │   └── Message/         # RPC异常
│       │   │   └── MonoBehaviour/       # MonoBehaviour脚本
│       │   │       ├── CanvasConfig.cs, ComponentView.cs
│       │   │       ├── GizmosDebug.cs, Init.cs
│       │   │       ├── ReferenceCollector.cs, UILayerScript.cs
│       │   ├── Plugins/                 # 原生插件库
│       │   │   ├── CommandLine.dll, ICSharpCode.SharpZipLib.dll
│       │   │   ├── Android/libs/arm64-v8a, armeabi-v7a, x86/
│       │   │   ├── iOS/                 # (空)
│       │   │   ├── MacOS/libkcp.dylib, libRecastDll.dylib
│       │   │   ├── x86_64/kcp.dll, RecastDll.dll
│       │   │   └── x86/kcp.dll
│       │   ├── Res/                     # 渲染资源(URP配置, Material)
│       │   ├── Scenes/Init.unity        # 初始场景
│       │   └── ThirdParty/              # 客户端第三方库
│       │       ├── Demigiant/            # DoTween动画库
│       │       ├── ETTask/              # 异步任务
│       │       ├── ILRuntime/           # ILRuntime热更框架
│       │       ├── LitJson/             # JSON库
│       │       ├── protobuf-net/        # Protobuf序列化
│       │       ├── ShareLib/            # 共享库
│       │       ├── Spine/               # Spine骨骼动画
│       │       └── sqlitekit/           # SQLite数据库
│       ├── Codes/                       # 客户端代码
│       │   ├── Hotfix/                  # (Client, Common, Core, Demo, Module)
│       │   ├── HotfixView/              # (Client, Common, Demo, Module)
│       │   │   └── AppStart_Init.cs
│       │   ├── Model/                   # (Client, Core, Demo, Generate, Module)
│       │   └── ModelView/               # (Client, Define, Demo, Module, ShareDefine)
│       ├── Library/                     # Unity引擎库缓存
│       ├── Logs/                        # Unity日志
│       └── Packages/manifest.json, packages-lock.json
│
├── Excel/                               # Excel配置表源文件
│   ├── AI.xlsx                          # AI配置
│   ├── Example.xlsx                     # 示例配置
│   ├── GameServerInfoConfig.xlsx
│   ├── material.xlsx                    # 材料
│   ├── Nerve.xlsx / NerveLayer.xlsx    # 神经/天赋
│   ├── NerveMould.xlsx / NerveTier.xlsx
│   ├── Player.xlsx / PlayerProperty.xlsx
│   ├── RoleLevelUp.xlsx                # 角色升级
│   ├── StartConfig.xlsx                # 服务器启动配置
│   ├── Translate.xlsx / TranslateProc.xlsx
│   └── Unit.xlsx                       # 单位
│
└── proto/                               # Protocol Buffers协议定义文件
    └── (proto文件)
```

---

## 项目架构

### 核心框架
- **ET6 (Entity-Component Framework)** : 基于ECS(实体-组件-系统)的服务端框架，结合 `.NET 6.0`

### 服务器架构 (分布式多进程)
| 服务器类型 | 职责 |
|---|---|
| **LoginServer** | 玩家登录、账号验证、ID生成 |
| **GateServer** | 网关：负责客户端连接管理与会话密钥 |
| **GameServer** | 游戏逻辑：玩家数据、背包、属性、角色升级 |
| **CacheServer** | 缓存：Unit数据缓存 |
| **Watcher** | 守护进程：监控服务健康状态 |

### 客户端架构
- **Unity** + **ILRuntime** 热更新
- 逻辑层分 Model(不热更) / Hotfix(热更)
- UI层通过 `Bundles/dialog/` 动态加载预制体

### 关键技术栈
| 技术 | 用途 |
|---|---|
| **MongoDB** | 数据库(玩家数据持久化) |
| **Protobuf** | 网络协议序列化 |
| **KCP** | 可靠UDP传输协议 |
| **Recast** | 寻路网格导航(NavMesh) |
| **Roslyn** | 代码分析器与编译时检查 |
| **ILRuntime** | Unity客户端热更新方案 |
| **NLog** | 日志系统 |
| **EPPlus** | Excel配置读取与导出 |

### 开发工作流
1. 在 `Excel/` 目录编写配置表
2. 执行 `导出Excel.bat` → 生成 `Config/*.bytes` + `Generate/Config/*.cs`
3. 在 `proto/` 目录定义网络协议
4. 执行 `导出Proto.bat` → 生成 `Generate/Message/*.cs`
5. 执行 `启动服务器.bat` 启动服务器
