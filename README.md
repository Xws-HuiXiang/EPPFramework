# EPPFramework
增量式更新的前后端框架。

*首次运行框架需要修改较多配置信息。*

注：

1. 如果未特殊标明，框架内的*.cfg文件内容均为**json**格式。
2. 目前没有区分资源服务器和逻辑服务器，需要自行修改。
3. 与一般通信消息协议不同，这个框架内消息格式为：'int32''int32''int32''string'。分别为：'消息总长度''消息处理器ID''消息处理器中方法ID''消息体'。

### 1.运行服务器

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

   *不需要的字段自行删除蛤~*

2. 按需要开启/关闭`Netowrk/ServerSocket.cs`文件头部的宏定义。`DEBUG_MOD`主要控制监听的IP地址，`DEV_MOD`主要为监听端口不同。做区分是为了同时运行开发版本和线上版本。

3. 修改`Netowrk/ServerSocket.cs`文件中，在非`DEBUG_MOD`下的`ipString`和`httpIPString`（如果不开http监听则不用填）。

4. 修改`Common/ServerUtil.cs`中`ServerFileRootPath`属性的返回值，值为服务器静态文件的根路径。服务器热更新有关的所有文件的根路径。

服务器修改相关端口完成。

客户端与服务器通信使用DH算法交换密钥，公钥与密钥的定义在`ServerSocket.cs`文件内，字段名称：`publicKey`和`secretKey`。

在线上版本中检测热更新时，需要一个固定的目录结构：

Root:
├─Development
│  ├─Android
│  │  ├─Latest
│  │  │  └─Lua2.zip
│  │  │  └─Res1.zip
│  │  ├─Lua
│  │  │  └─Lua1.zip
│  │  │  └─Lua2.zip
│  │  ├─Res
│  │  │  └─Res1.zip
│  │  └─Scene
│  │      ├─FirstScene
│  │      │  └─1
│  │      ├─GameScene
│  │      │  └─1
│  │      └─LoadingScene
│  │          └─1
│  ├─iOS
│  │  ├─Latest
│  │  ├─Lua
│  │  ├─Res
│  │  └─Scene
│  │      ├─FirstScene
│  │      │  └─1
│  │      ├─GameScene
│  │      │  └─1
│  │      └─LoadingScene
│  │          └─1
│  ├─MacOS
│  │  ├─Latest
│  │  ├─Lua
│  │  ├─Res
│  │  └─Scene
│  │      ├─FirstScene
│  │      │  └─1
│  │      ├─GameScene
│  │      │  └─1
│  │      └─LoadingScene
│  │          └─1
│  ├─Protos
│  └─Windows
│      ├─Latest
│      ├─Lua
│      ├─Res
│      └─Scene
│          ├─FirstScene
│          │  └─1
│          ├─GameScene
│          │  └─1
│          └─LoadingScene
│              └─1
└─Release
        与Development目录结构相同

在对应平台名称的目录中，“Lua”、“Res”存放热更新的压缩包文件，名称为“Lua+热更新版本.zip"，Res同理。**注意一定要是zip压缩包，若要换格式自行修改压缩/解压缩相关库。**”Latest“文件夹中存放第一次启动游戏时下载的资源，命名与Res和Lua文件夹相同。

### 2.运行客户端

客户端目前主要为UI框架，3D部分正在计划开发。UI框架中以Panel为单元，框架内管理各种Panel。如：LoginPanel、MainPagePanel、GamePanel等等。

客户端绝大部分设置在`Assets/Scripts/AppConst.cs`文件中。在`Assets/Scripts/Managers/NetworkManager.cs`文件中定义公钥，字段名称：`publicKey`。

AppConst主要字段的解释（代码内也有详细注释）：

| 字段名               | 说明                                                   |
| -------------------- | ------------------------------------------------------ |
| ROOT_URL             | 各种网络路径的根网址。比如https://xxx.com/File         |
| devMode              | 是否为开发模式。控制日志输出和热更新资源目录           |
| loadLoaclAssetBundle | 是否加载工程目录下的资源。为true则实例化工程中的预制体 |
| useLocalIP           | 是否使用本地IP                                         |
| useDevPort           | 是否使用开发模式端口                                   |
| adsTestMod           | UnityAds的广告投放是否启用开发模式                     |
| loadProjectScene     | 是否加载工程目录中的场景                               |
| zipPassword          | Res.zip和Lua.zip压缩包密码                             |
| abPackageKey         | ab包的AES解压缩密钥                                    |
| encryptionFileSuffix | 打包资源时的后缀名。可以随意修改                       |

修改`AppConst.cs`中的属性：`IP`和`HttpIP`为自己的IP地址。

添加新的平台支持时，在`GetPlatformName()`方法中添加对应的平台名称，在服务器提供相同的名称目录即可实现热更新资源。

其余方法和属性看看就明白了:)

将公钥设置与服务器相同后，启动服务器，再启动客户端，即可看到服务器输出”有客户端连接“的日志。

#### 如何打包资源

#### 如何打包场景

#### 创建第一个Panel

### 3.相关工具说明

##### 协议相关类生成工具

##### 热更新资源版本控制工具