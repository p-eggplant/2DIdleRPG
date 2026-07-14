# Git 版本管理指南

> 当前项目的文件分类与 Git 上传策略

---

## 一、必须上传 Git 的文件

### 1. 源代码

| 目录 | 内容 | 说明 |
|------|------|------|
| `ET6/Unity/Codes/` | 全部 `.cs` | 客户端核心逻辑（Model/ModelView/Hotfix/HotfixView） |
| `ET6/Unity/Assets/Mono/Core/` | 全部 `.cs` | 框架核心库（Entity、Event、Object、Timer、Helper 等） |
| `ET6/Unity/Assets/Mono/MonoBehaviour/` | 全部 `.cs` | Init、ReferenceCollector、UILayerScript等 |
| `ET6/Unity/Assets/Mono/Helper/` | 全部 `.cs` | PathHelper、AssetsBundleHelper 等 |
| `ET6/Unity/Assets/Mono/Module/` | 全部 `.cs` | 网络层（KChannel、KService、TChannel 等） |
| `ET6/Unity/Assets/Mono/ILRuntime/` | 全部 `.cs` | ILRuntime 适配层（**不包含 Generate/ 下的生成代码**） |
| `ET6/Unity/Assets/Editor/` | 全部 `.cs` | 编辑器扩展脚本 |
| `ET6/Server/` | 全部 `.cs` | 服务端逻辑 |
| `ET6/Robot/` | 全部 `.cs` | 机器人测试逻辑 |
| `ET6/Tools/` | 全部 `.cs` | 工具链代码 |
| `ET6/ThirdParty/` | 全部 `.cs` | 服务端第三方源码 |
| `ET6/Libs/Kcp/` | `.c` `.h` | KCP 协议 C 源码 |
| `ET6/Libs/RecastDll/` | `.cpp` `.h` `.cmake` | 寻路库 C++ 源码 |

### 2. 协议定义

| 目录 | 内容 | 说明 |
|------|------|------|
| `proto/` | 全部 `.proto` | 网络协议定义（13个文件） |

### 3. 配置表源文件

| 目录 | 内容 | 说明 |
|------|------|------|
| `Excel/` | 全部 `.xlsx` | 策划配置表源文件（25个文件） |

### 4. Unity 美术资源

| 目录 | 内容 | 说明 |
|------|------|------|
| `ET6/Unity/Assets/Bundles/UI/` | `.prefab` `.meta` | UI 预制体 |
| `ET6/Unity/Assets/Bundles/dialog/` | `.prefab` `.png` `.anim` `.controller` `.mat` `.meta` | UI 弹窗、动画、贴图（~250张PNG） |
| `ET6/Unity/Assets/Bundles/sound/` | `.MP3` `.meta` | 音效文件 |
| `ET6/Unity/Assets/Bundles/Config/` | `.bytes` `.meta` | 导出的配置二进制（从Excel生成，但需要随项目提交） |
| `ET6/Unity/Assets/Res/` | `.mat` `.asset` `.meta` | 渲染资源 |
| `ET6/Unity/Assets/Scenes/` | `.unity` `.meta` | 场景文件 |

### 5. Unity 项目配置

| 文件/目录 | 说明 |
|-----------|------|
| `ET6/Unity/ProjectSettings/` | 所有 `.asset` 文件、`.meta`、`boot.config`（27个设置文件） |
| `ET6/Unity/Packages/manifest.json` | Unity 包管理配置 |
| `ET6/Unity/Packages/packages-lock.json` | 包版本锁定文件 |
| `ET6/Unity/Assets/link.xml` | 链接器配置 |

### 6. 第三方库（需提交版本）

| 目录 | 内容 | 说明 |
|------|------|------|
| `ET6/Unity/Assets/ThirdParty/Demigiant/` | `.dll` + 部分源码 | DOTween 动画库 |
| `ET6/Unity/Assets/ThirdParty/Spine/` | `.cs` + `.meta` | Spine 骨骼动画运行时 |
| `ET6/Unity/Assets/ThirdParty/sqlitekit/` | 全部 `.cs` | SQLite 数据库 |
| `ET6/Unity/Assets/Mono/ImportPackage/` | `.cs` `.md` | xasset、ProCamera2D、Splat Painter |
| `ET6/Unity/Assets/Plugins/` | `.dll` (CommandLine、SharpZipLib) | 托管插件 |
| `ET6/Unity/Assets/Plugins/x86_64/ x86/ MacOS/ Android/` | `.dll` `.dylib` `.so` | 原生插件（kcp、RecastDll） |

### 7. 项目配置文件与文档

| 文件/目录 | 说明 |
|-----------|------|
| `README.md` | 项目说明 |
| `学习建议/` | 技术文档 |
| `ET6/.gitignore` | Git 忽略规则 |
| `ET6/.editorconfig` | 编辑器配置 |
| `ET6/.gitattributes` | Git 属性 |
| `ET6/Server.sln` | 服务端解决方案 |
| `ET6/Robot.sln` | 机器人解决方案 |
| `ET6/Unity/Unity.sln.DotSettings` | Rider IDE 设置 |
| `ET6/Directory.Build.props` | MSBuild 配置 |
| `ET6/LICENSE` | 许可证 |
| `导出Excel.bat`、`导出Proto.bat`、`启动服务器.bat` | 开发工具脚本 |

---

## 二、不要上传 Git 的文件（编译/构建生成）

### 1. 编译输出

| 目录/文件 | 说明 | 大小 |
|-----------|------|------|
| `ET6/Bin/` | 所有 `.dll` `.exe` `.pdb` `.json` `.xml` `.config` | ~30MB |
| `ET6/Unity/Library/ScriptAssemblies/` | Unity 编译的程序集 | ~50MB |
| `ET6/Unity/obj/` | 编译中间文件 | ~20MB |
| 所有 `*/obj/` 目录 | .NET 编译中间件 | 各个子项目 |
| 所有 `*/bin/` 目录 | .NET 编译输出 | 各个子项目 |
| `ET6/Unity/Assets/Bundles/Code/Code.dll.bytes` | 热更 DLL 二进制（**已在 .gitignore 中排除**） | 2MB |
| `ET6/Unity/Assets/Bundles/Code/Code.pdb.bytes` | 热更调试符号 | 1MB |

### 2. Unity 本地缓存

| 目录 | 说明 | 大小 |
|------|------|------|
| `ET6/Unity/Library/` | 整个目录（脚本缓存、包缓存、状态缓存、着色器缓存） | ~200MB+ |
| `ET6/Unity/Temp/` | 临时文件 | 不定 |
| `ET6/Unity/UserSettings/` | 编辑器用户偏好 | 很小 |

### 3. 日志

| 目录 | 说明 |
|------|------|
| `ET6/Logs/` | 服务端运行日志 |
| `ET6/Unity/Logs/` | Unity 运行日志 |

### 4. 发布产物

| 目录 | 说明 |
|------|------|
| `ET6/Release/` | 打包发布的 `.unity3d` AssetBundle 文件 |

### 5. IDE/编辑器临时文件

| 目录/文件 | 说明 |
|-----------|------|
| `.vs/` | Visual Studio 用户设置 |
| `.vscode/` | VS Code 设置 |
| `.idea/` | Rider 设置 |
| `*.suo` | VS 解决方案用户选项 |
| `*.user` | 用户配置文件 |

### 6. ILRuntime 自动生成的绑定代码

| 目录 | 说明 |
|------|------|
| `ET6/Unity/Assets/Mono/ILRuntime/Generate/` | **约190个 CLR 绑定文件**，通过 ILRuntime 工具自动生成 |

---

## 三、需要注意的特殊文件

### 1. Unity .meta 文件 **必须上传**

Unity 每个资源文件都有一个对应的 `.meta` 文件（存储 GUID），如果不上传 `.meta`，其他人拉取后资源引用会全部断裂。

**规则**：所有 `Assets/` 目录下的资源 `.meta` 都需要上传，`Library/` 和 `Temp/` 下的 `.meta` 类缓存不上传。

### 2. ILRuntime 绑定代码

`ET6/Unity/Assets/Mono/ILRuntime/Generate/` 下的约 190 个 `.cs` 文件是自动生成的 CLR 绑定代码。

- **多人协作**：建议上传（否则每个人都要重新生成）
- **单人开发**：可不上传（本地运行生成即可）

### 3. Excel 导出的 .bytes 文件

`ET6/Unity/Assets/Bundles/Config/` 下的 `.bytes` 是由 `Excel/` 中的 `.xlsx` 导出的。虽然理论上可以重新生成，但为方便协作，**建议上传**（Unity 打包时直接引用这些文件）。

### 4. 服务端 .bytes 配置文件

`ET6/Config/` 下的 `.bytes` 与 Unity Bundles 中的内容重复，是服务端运行时读取的配置。**建议上传**。

---

## 四、建议的 .gitignore 配置

> 当前项目已有的 `.gitignore` 基本正确，建议补充以下规则：

```gitignore
# === 编译输出 ===
Bin/
**/bin/
**/obj/
**/Debug/
**/Release/
*.dll
*.exe
*.pdb
*.deps.json
*.runtimeconfig.json
*.nupkg

# === Unity 缓存 ===
[Ll]ibrary/
[Tt]emp/
[Ll]ogs/
[Uu]serSettings/
[Ll]ocalLow/

# === IDE ===
.vs/
.vscode/
.idea/
*.suo
*.user
*.userprefs

# === 日志 ===
[Ll]ogs/
[Ll]og/

# === 发布产物 ===
[Pp]ublish/
**/[Pp]ackages/PackageCache/
**/[Pp]ackages/.*/

# === 系统 ===
*.DS_Store
Thumbs.db
```

> **注意**：不要直接在 `.gitignore` 中写 `**/obj/` 这样的全局规则，因为有些第三方源码（如 SQLite 的 src/）在源代码目录中。建议按项目具体目录范围来配。

---

## 五、初始化 Git 步骤

```bash
# 1. 在项目根目录初始化
cd D:\software\gongshi\Demo升级\project
git init

# 2. 添加 .gitignore
# （已存在 ET6/.gitignore，但建议在根目录也放一份）

# 3. 检查哪些文件会被追踪
git status

# 4. 确认无误后暂存
git add .

# 5. 首次提交
git commit -m "feat: 初始化 ET6 项目"

# 6. 关联远程仓库
git remote add origin https://github.com/你的用户名/你的仓库名.git

# 7. 推送
git push -u origin main
```

---

## 六、首次推送到 GitHub 的预估大小

| 类别 | 估算大小 |
|------|----------|
| C# 源码 (~1000 文件) | ~15 MB |
| Proto 定义 | < 1 MB |
| Excel 配置表 | < 1 MB |
| Unity 美术资源（预制体/贴图/动画） | ~55 MB |
| Unity 项目配置 | ~5 MB |
| 第三方库 | ~30 MB |
| 其他（脚本、文档、配置） | ~5 MB |
| **总计** | **~110 MB** |

> 这个大小对 Git 来说完全可接受。GitHub 免费仓库限制是 1GB，推荐不超过 5GB。
