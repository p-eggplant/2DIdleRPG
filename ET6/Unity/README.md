# ET6 Game Framework — Unity Client

基于 **ET (EGameTale) V6 框架** 的 Unity 客户端项目，采用 **ECS 架构** 与 **ILRuntime 热更新方案**，使用 **Unity 2020.3.49f1c1** + **URP** 渲染管线。

---

## 目录结构

```
Unity/
├── Assets/                          # Unity 资源目录
│   ├── Bundles/                     # AssetBundle 源资源
│   │   ├── Code/                    # 打包的代码 DLL
│   │   ├── Config/                  # 游戏配置数据
│   │   ├── dialog/                  # 对话框 UI
│   │   │   ├── dlg_common/          # 通用对话框
│   │   │   ├── dlg_gm/             # GM 调试对话框
│   │   │   ├── dlg_login/          # 登录对话框
│   │   │   ├── dlg_package/        # 背包对话框
│   │   │   ├── dlg_reconnection/   # 断线重连对话框
│   │   │   ├── dlg_role/           # 角色对话框
│   │   │   └── dlg_updata/         # 更新对话框
│   │   ├── resources/              # 游戏资源
│   │   ├── sound/                  # 音频文件
│   │   └── UI/                     # 通用 UI 预制体
│   ├── Editor/                     # Unity 编辑器扩展 (Unity.Editor)
│   │   ├── AssetPostProcessor/     # 资源导入后处理
│   │   ├── BuildEditor/            # 构建管线编辑器
│   │   ├── ComponentViewEditor/    # ECS 组件监视器
│   │   │   ├── Entity/            # 实体视图编辑器
│   │   │   └── TypeDrawer/        # 类型绘制器
│   │   ├── Helper/                 # 编辑器工具辅助
│   │   ├── ILRuntimeEditor/        # ILRuntime 调试集成
│   │   ├── RecastNavDataExporter/  # NavMesh 数据导出
│   │   ├── ReferenceCollectorEditor/ # 引用收集器编辑器
│   │   ├── ServerCommandLineEditor/ # 服务器命令行工具
│   │   └── ToolEditor/             # 通用工具编辑器
│   ├── Hotfix/                     # 热更新代码桩 (Unity.Hotfix)
│   ├── HotfixView/                 # 热更新视图桩 (Unity.HotfixView)
│   ├── Model/                      # 模型层桩 (Unity.Model)
│   ├── ModelView/                  # 模型视图桩 (Unity.ModelView)
│   ├── Mono/                       # 启动引导层 (Unity.Mono)
│   │   ├── Core/                   # 核心基础设施
│   │   │   ├── Helper/            # 工具类 (Byte, Enum, File, Math, MD5, Network, Object, Process, Random, String, Zip)
│   │   │   ├── Log/               # 日志系统 (ILog, NLogger, FileLogger, UnityLogger)
│   │   │   ├── DoubleMap.cs       # 双向字典
│   │   │   ├── HashSetComponent.cs # HashSet 组件
│   │   │   ├── ListComponent.cs   # List 组件
│   │   │   ├── MonoPool.cs        # 对象池
│   │   │   ├── MultiMap.cs        # 多值字典系列
│   │   │   ├── Options.cs         # 命令行选项
│   │   │   ├── ThreadSynchronizationContext.cs # 线程同步上下文
│   │   │   ├── TimeHelper.cs / TimeInfo.cs     # 时间工具
│   │   │   └── WrapVector/Quaternion.cs        # 结构体包装
│   │   ├── Helper/                # Mono 层辅助
│   │   ├── ILRuntime/             # ILRuntime 绑定代码生成
│   │   ├── ImportPackage/         # 第三方包源码
│   │   │   ├── ProCamera2D/       # 2D 相机工具
│   │   │   ├── Splat Painter/     # 地形绘制
│   │   │   └── xasset/            # 资源管理系统
│   │   ├── Module/                # 网络模块
│   │   │   ├── Message/           # 消息协议
│   │   │   ├── Mongo/             # MongoDB 集成
│   │   │   ├── Network/           # KCP 网络层
│   │   │   └── NetworkTCP/        # TCP 网络层
│   │   ├── MonoBehaviour/         # Unity 生命周期脚本
│   │   ├── CodeLoader.cs          # 代码加载器
│   │   ├── Define.cs              # 编译定义
│   │   └── ...                    # 其他基础组件
│   ├── Plugins/                   # 原生插件
│   │   ├── Android/               # Android (ARM64, ARMv7)
│   │   ├── iOS/                   # iOS 占位
│   │   ├── MacOS/                 # macOS (libkcp.dylib, libRecastDll.dylib)
│   │   ├── x86/                   # Windows 32-bit (kcp.dll)
│   │   ├── x86_64/                # Windows 64-bit (kcp.dll, RecastDll.dll)
│   │   ├── CommandLine.dll        # 命令行解析
│   │   └── ICSharpCode.SharpZipLib.dll # ZIP 压缩
│   ├── Res/                       # 渲染资源
│   │   ├── Mat/                   # 材质
│   │   └── UniversalRenderPipelineAsset* # URP 管线配置
│   ├── Scenes/                    # 场景
│   │   └── Init.unity             # 启动场景
│   ├── ThirdParty/                # 第三方库源码 (Unity.ThirdParty)
│   │   ├── Demigiant/             # DOTween 动画库
│   │   ├── ETTask/                # ET 异步任务系统
│   │   ├── ILRuntime/             # C# IL 解释器（热更核心）
│   │   ├── LitJson/               # JSON 解析
│   │   ├── protobuf-net/          # Protocol Buffers 序列化
│   │   ├── ShareLib/              # 共享库 (KCP 协议、Recast 寻路)
│   │   ├── Spine/                 # 2D 骨骼动画
│   │   └── sqlitekit/             # SQLite 数据库
│   └── link.xml                   # IL2CPP 链接器规则
│
├── Codes/                          # 热更新源码（在 Assets 外部编译）
│   ├── Hotfix/                    # 热更新逻辑层 (客户端业务逻辑)
│   │   ├── Client/                # 客户端系统
│   │   │   ├── AccountSystem/     # 账号系统
│   │   │   ├── Common/            # 通用工具
│   │   │   ├── Login/             # 登录流程
│   │   │   └── PlayerSystem/      # 玩家系统
│   │   │       ├── PlayerBagComponentSystem/    # 背包
│   │   │       ├── PlayerDataComponentSystem/   # 角色数据
│   │   │       └── PlayerRoleUpComponentSystem/ # 角色升级
│   │   ├── Common/                # 共享通用代码
│   │   ├── Core/                  # 热更新核心框架
│   │   │   └── Scene/             # 场景管理
│   │   ├── Demo/                  # Demo 逻辑
│   │   │   ├── AI/                # AI 行为
│   │   │   ├── Login/             # 登录 Demo
│   │   │   ├── Move/              # 移动 Demo
│   │   │   ├── Scene/             # 场景 Demo
│   │   │   ├── Session/           # 网络会话 Demo
│   │   │   └── Unit/              # 游戏单位 Demo
│   │   └── Module/                # 模块实现
│   │       ├── AI/                # AI 模块
│   │       ├── Config/            # 配置模块
│   │       ├── GTools/            # GM 调试工具
│   │       ├── Message/           # 消息处理器
│   │       ├── Numeric/           # 数值属性系统
│   │       ├── Ping/              # Ping 模块
│   │       └── Recast/            # 寻路模块
│   │
│   ├── HotfixView/               # 热更新视图层 (UI 事件、Unity 绑定)
│   │   ├── Client/                # 客户端视图系统
│   │   │   ├── UIBase/            # 基础 UI
│   │   │   ├── UIDoTweenTest/     # 动画测试 UI
│   │   │   ├── UIExample/         # 示例 UI
│   │   │   ├── UIGm/              # GM 工具 UI
│   │   │   ├── UILogin/           # 登录 UI 处理
│   │   │   └── UIPackage/         # 背包 UI 处理
│   │   ├── Common/                # 通用视图工具
│   │   ├── Demo/                  # Demo 视图
│   │   └── Module/                # 视图模块
│   │       ├── Resource/          # 资源加载
│   │       └── UI/                # UI 系统 (UIEvent, UIComponent)
│   │
│   ├── Model/                     # 纯数据模型层 (无 UnityEngine 依赖)
│   │   ├── Client/                # 客户端数据模型
│   │   │   ├── Account/           # 账号数据
│   │   │   ├── Define/            # 客户端定义
│   │   │   ├── Login/             # 登录状态
│   │   │   └── Player/            # 玩家数据组件
│   │   ├── Core/                  # ECS 核心
│   │   │   ├── Entity/            # 实体系统 (Entity, Component)
│   │   │   ├── Event/             # 事件系统
│   │   │   ├── Object/            # 基类 (IDisposable, Object)
│   │   │   ├── Scene/             # 场景管理
│   │   │   └── Timer/             # 计时器系统
│   │   ├── Demo/                  # Demo 数据模型
│   │   ├── Generate/              # 自动生成代码
│   │   │   ├── Config/            # 配置类 (AI, Example, Material, Nerve, PlayerData...)
│   │   │   ├── ConfigPartial/     # 配置扩展
│   │   │   └── Message/           # 消息协议类
│   │   └── Module/                # 模块定义 (服务端+客户端共享)
│   │       ├── Actor/             # Actor 模型
│   │       ├── ActorLocation/     # Actor 定位
│   │       ├── AI/                # AI 定义
│   │       ├── Config/            # 配置系统接口
│   │       ├── CoroutineLock/     # 协程锁
│   │       ├── Message/           # 消息系统
│   │       ├── Numeric/           # 数值系统
│   │       ├── Ping/              # Ping 定义
│   │       └── Recast/            # 寻路定义
│   │
│   └── ModelView/                # 模型视图层 (引用 UnityEngine)
│       ├── Client/                # 客户端视图模型
│       │   ├── UIBase/            # 基础 UI 组件
│       │   ├── UIDoTweenTest/     # DOTween 测试
│       │   ├── UIExample/         # UI 示例
│       │   ├── UIGm/              # GM UI 组件
│       │   ├── UILogin/           # 登录 UI 组件
│       │   └── UIPackage/         # 背包 UI 组件
│       ├── Define/                # UI 定义
│       ├── Demo/                  # Demo 视图组件
│       ├── Module/                # 视图模块定义
│       │   └── UI/                # UI 系统定义
│       └── ShareDefine/           # 共享定义
│
├── Packages/                      # Unity Package Manager 配置
│   ├── manifest.json              # 包依赖清单
│   └── packages-lock.json         # 包锁定版本
│
├── ProjectSettings/               # Unity 项目设置
│   ├── ProjectVersion.txt         # Unity 版本: 2020.3.49f1c1
│   ├── ProjectSettings.asset      # 播放器设置 (IL2CPP, 脚本定义)
│   ├── EditorBuildSettings.asset  # 构建场景列表
│   └── ...
│
├── Library/                       # Unity 缓存（自动生成）
├── Logs/                          # Unity 日志
├── obj/                           # .NET 中间编译输出
├── Temp/                          # Unity 临时文件
├── UserSettings/                  # 编辑器用户偏好
├── .vs/                           # Visual Studio 配置
│
├── Unity.sln                      # Visual Studio 解决方案
├── *.csproj                       # .NET 项目文件（由 .asmdef 生成）
├── .gitignore                     # Git 忽略规则
├── .vsconfig                      # VS 组件配置
└── Unity.userprefs                # 编辑器偏好
```

---

## 架构设计

### ECS 四层架构

项目采用 ET 框架经典的**四层架构**，代码按程序集划分：

| 程序集 | 层 | 职责 | 依赖 |
|--------|-----|------|------|
| `Unity.ThirdParty` | 第三方库 | DOTween, ILRuntime, protobuf-net, Spine, LitJson, KCP, Recast 等 | 无 |
| `Unity.Mono` | 启动引导层 | Unity 生命周期管理、核心工具、网络层、代码加载、日志 | ThirdParty |
| `Unity.Model` | 纯数据模型层 | ECS 核心（Entity/Component/System）、消息协议、配置定义、数值系统 | Mono, ThirdParty |
| `Unity.ModelView` | 模型视图层 | 持有 UnityEngine 引用的数据组件，如 UI 组件定义、动画组件 | Model, Mono, ThirdParty |
| `Unity.Hotfix` | 热更新逻辑层 | 客户端业务系统（登录、背包、角色升级、AI 等） | Model, Mono, ThirdParty |
| `Unity.HotfixView` | 热更新视图层 | UI 事件处理、Unity 资源绑定、视图逻辑 | ModelView, Hotfix, Model, Mono, ThirdParty |
| `Unity.Editor` | 编辑器层 | 构建工具、资源后处理、组件监视器、寻路数据导出 | Mono, ThirdParty |

### 热更新机制

- **ILRuntime 模式**：通过 ILRuntime IL 解释器加载热更新 DLL，支持运行时热更
- **Mono 模式**：通过 `Assembly.Load` 直接加载 DLL，适合调试
- **Reload 模式**：开发迭代时热重载

热更新源码位于 `Codes/` 目录（在 Unity Assets 外部），编译为独立 DLL 后可由 `CodeLoader.cs` 加载。

---

## 技术栈

| 技术 | 用途 |
|------|------|
| **Unity 2020.3 LTS** | 游戏引擎 |
| **URP** | 通用渲染管线 |
| **ILRuntime** | C# IL 解释器 - 热更核心 |
| **DOTween / DOTweenPro** | 动画补间 |
| **Spine** | 2D 骨骼动画 |
| **protobuf-net** | Protocol Buffers 序列化 |
| **LitJson** | JSON 解析 |
| **KCP Protocol** | 可靠 UDP 网络传输 |
| **Recast Navigation** | 寻路系统 (RecastDLL) |
| **xasset** | 资源管理与 AssetBundle |
| **SQLite (sqlitekit)** | 本地数据库 |
| **NLog** | 日志系统 |
| **SharpZipLib** | ZIP 压缩解压 |
| **ProCamera2D** | 2D 相机工具 |

---

## 构建目标

- **Windows** (Standalone, IL2CPP)
- **Android** (min SDK 19, ARM64 + ARMv7)
- **iOS** (min iOS 11)
- **macOS**
