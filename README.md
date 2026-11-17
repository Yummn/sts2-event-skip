# Event Skip / 跳过事件

<!-- MOD-DOWNLOAD-ZH-START -->
## 中文下载与安装（先看这里）

- **跳过事件**：请进入 [发布下载 / Releases](https://github.com/Yummn/sts2-event-skip/releases)，展开 **Assets**，选择对应平台和游戏版本的中文安装包。
- **不要下载** Code → Download ZIP 或 Source code (zip/tar.gz)：那是源码，不含可直接安装的编译产物。
- 手动上传的源码压缩包已撤下，今后只发布安装包、说明和验证资料；项目代码仍保留在仓库。

### 本轮对应安装包

- [手机 v0.113.0](https://github.com/Yummn/sts2-event-skip/releases/download/mobile-v113-pc-v111-20261117/EventSkip-0.1.0-Android-v113.0.zip)
- [电脑 v0.111.0（尚未实机回归）](https://github.com/Yummn/sts2-event-skip/releases/download/mobile-v113-pc-v111-20261117/EventSkip-0.1.0-PC-v111.0.zip)

以上只是本轮构建目标，其他游戏版本请查看相应历史发布；不能混装手机和电脑版。旧文档中的版本号、源码附件指引如与本节冲突，以本节及对应Release说明为准。

### 电脑版安装

1. 退出游戏，解压**电脑版安装包**。
2. 将整个 `EventSkip` 文件夹放到游戏根目录的 `mods` 文件夹中；没有 `mods` 就新建。
3. 重启游戏，在模组列表中启用。不要只把ZIP或源码目录放在游戏根目录。

```text
Slay the Spire 2/mods/EventSkip/EventSkip.dll
Slay the Spire 2/mods/EventSkip/EventSkip.json
```

手机版请使用对应游戏版本的启动器导入安装包，不要拿电脑版代替；保留原有存档和模组配置。

GitHub 自动生成的 Source code 链接不能单独删除，因此请认准上面的中文安装包链接。
<!-- MOD-DOWNLOAD-ZH-END -->

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
