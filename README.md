# EPPFramework
增量式更新的前后端框架。

*首次运行框架需要修改较多配置信息。*



注：

1. 如果未特殊标明，框架内的***.cfg**文件内容均为**json**格式。
2. 目前没有区分资源服务器和逻辑服务器，需要自行修改。

### 运行服务器

1. 在构建目录`EPPFServer/EPPFramework/bin/Debug/net5.0/Config/Config.json`的文件为运行配置，各字段说明：

   | 字段名                   | 说明                               |
   | ------------------------ | ---------------------------------- |
   | ResVersion               | 客户端Res版本号。已经弃用          |
   | LuaVersion               | 客户端Lua版本号。已经弃用          |
   | DataBaseConnectionString | 数据库连接串                       |
   | ShowGameVersion          | 客户端显示的版本号                 |
   | LoadingTipsFilePath      | 加载时的随机文字的文件地址         |
   | DefaultAvatarPath        | 默认的玩家头像磁盘地址（完整路径） |
   | Command/Active           | 是否启用服务器控制台               |
   | Command/AutoOpenExe      | 服务器控制台是否自动开启           |
   | Command/CommandExePath   | 服务器控制台程序完整路径           |
   | Command/IP               | 服务器控制台监听的IP地址           |
   | Command/Port             | 服务器控制台监听的端口号           |

2. 按需要开启/关闭`Netowrk/ServerSocket.cs`文件头部的宏定义。`DEBUG_MOD`主要控制监听的IP地址，`DEV_MOD`主要为监听端口不同。做区分是为了同时运行开发版本和线上版本。

3. 修改`Netowrk/ServerSocket.cs`文件中，在非`DEBUG_MOD`下的`ipString`和`httpIPString`（如果不开http监听则不用填）。

4. 修改`Common/ServerUtil.cs`中`ServerFileRootPath`属性的返回值，值为服务器静态文件的根路径。服务器热更新有关的所有文件的根路径。

服务器修改相关端口完成。

在线上版本中，热更新需要一个固定的目录结构：



