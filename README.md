# Event Skip / 跳过事件

> 项目分类：个人项目 / 《杀戮尖塔 2》Mod / 小功能 Mod / 可直接使用

给事件初始选择页增加“跳过”选项，方便不想处理当前事件时直接离开。

## 功能

- 普通事件和先古之民事件的初始选择页增加“跳过”；
- 跳过普通事件：直接离开，不获得额外奖励；
- 跳过先古之民：获得 200 金币；
- 跳过涅奥：获得 100 金币；
- 假商人等自定义事件原有的离开按钮会显示为“跳过”；
- 一旦选择事件原本的选项，本事件后续页面不再显示“跳过”，避免先拿奖励再跳过；
- 不依赖 BaseLib，不含 PCK。

## 兼容版本

- Android：Slay the Spire 2 v0.103.2
- PC：Slay the Spire 2 v0.107.1

如 Release 中出现其他版本包，请按文件名选择与当前游戏版本一致的版本。

## 安装

1. 从 [Releases](https://github.com/Yummn/sts2-event-skip/releases) 下载对应平台的 ZIP。
2. 解压后，将 `EventSkip` 文件夹完整放入游戏 `mods/` 目录。
3. 在启动器中启用模组。

目录示例：

```text
mods/
└─ EventSkip/
   ├─ EventSkip.dll
   ├─ EventSkip.pdb
   ├─ EventSkip.json
   └─ README-EventSkip.md
```
