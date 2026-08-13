# 跳过事件（Event Skip）

有些路线只想快速经过事件房，所以我给事件的第一页补了一个“跳过”选项。普通事件会直接离开；先古之民和涅奥事件则分别获得 200 和 100 金币，作为这两类节点的替代收益。

## 具体规则

- 普通事件：直接离开，不获得原事件奖励。
- 先古之民：跳过后获得 200 金币。
- 涅奥：跳过后获得 100 金币。
- 假商人等本来就有离开按钮的事件，会把按钮文字统一显示为“跳过”。
- 一旦选择了事件原有选项，后续页面不会再出现“跳过”，避免先拿奖励再离开。

MOD 不依赖 BaseLib，也不包含 PCK 资源。当前提供 Android v0.103.2 和 PC v0.107.1 构建。

## 安装

从 [GitHub Releases](https://github.com/Yummn/sts2-event-skip/releases) 下载对应版本的 ZIP，解压后把完整的 `EventSkip` 文件夹放进游戏的 `mods` 目录，并在启动器中启用。

## 从源码构建

项目使用 .NET/C#。准备好对应版本的游戏程序集后，在仓库根目录运行：

```powershell
dotnet build EventSkip.csproj -c Release
```

离线行为检查可以运行 `python tests/verify_event_skip_offline.py`。
