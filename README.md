# EPPFramework
C#服务器+Unity客户端的简单增量式热更新框架。

*首次运行框架需要修改较多配置信息。*

注：

1. 如果未特殊标明，框架内的*.cfg文件内容均为**json**格式。
2. 目前没有区分资源服务器和逻辑服务器，需要自行修改。
3. 与一般通信消息协议不同，这个框架内消息格式为：'int32''int32''int32''string'。分别为：'消息总长度''消息处理器ID''消息处理器中方法ID''消息体'。
4. Unity版本为2019.3.15f1。

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

![服务器目录结构](https://www.qinghuixiang.com/File/Github/EPPF%E6%9C%8D%E5%8A%A1%E5%99%A8%E7%9B%AE%E5%BD%95%E7%BB%93%E6%9E%84.png)

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

#### 创建第一个Panel

1. 随便找一个Canvas，创建一个空物体（推荐锚点设置为按住alt点右下角的那个设置）
2. 修改名称为xxxPanel。注意以**Panel**结尾，这个很重要。
3. 转换为预制体。在`Assets/LuaFramework/Builder`文件夹下新建一个文件夹，将预制体拖拽到这个文件夹中。**这个文件夹名称将作为ab包的名称，其中的所有预制体在打包时会打包到对应的ab包内。**
4. （步骤5和6有编辑器扩展快速创建，详见‘创建Panel对应的Ctrl和View文件的扩展’）
5. 在`Assets/LuaFramework/Lua/Ctrl|View`文件夹创建对应的`xxxCtrl.lua`和`xxxPanel.lua`文件。
6. 在`Assets/LuaFramework/Lua/Managers/CtrlLuaManager.lua`文件内，添加三项内容：添加刚刚创建的Ctrl文件引用、CtrlNames表内添加对应的字段、函数`this.New()`中对`CtrlNames`对应字段赋值。
7. 在Lua代码中，使用`Panel.OpenPanel(CtrlNames.xxxCtrl);`即可创建一个Panel。

#### 创建Panel对应的Ctrl和View文件的扩展

点击菜单栏`EPP Tools/Create ToLua File`将打开创建文件的窗口。默认情况下，设置“AssetBundle名称”、**在Hierarchy窗口中选中要生成文件的Panel游戏物体**，再点击“从Lua模板创建”即可创建对应的Ctrl和Panel文件，并将在`CtrlLuaManager`内添加对应的引用。

若勾选了“添加Button事件监听”，则在Panel文件中添加Button游戏物体的引用且在Ctrl文件中添加Button的事件，Toggle同理。

“创建空Lua文件”将不写文件内容。

将对应的Panel游戏物体下的子物体拖拽到窗口中，则在创建文件时在Panel文件中添加对应物体的引用。

#### 如何打包资源

**因为资源包使用加密算法进行加密，所以必须使用框架内的打包工具或自行修改打包后的资源。**

点击菜单栏`EPP Tools/Create Assets Bundle`将打开生成ab包的扩展窗口。

其中，“资源打包范围”若选择为“Distributed Folder”则只会打包“资源根目录”指定的文件夹下的文件，每个文件夹作为一个ab包；其他选项打出来的包是整个工程中可以打包的资源均会打成资源包。“Lua代码根目录”是项目逻辑Lua文件的根目录。“ToLua代码根目录”为ToLua框架带的Lua代码文件目录。“打包资源”和“打包Lua代码”控制是否打包Res和Lua。压缩方式和目标平台根据项目自行选择，不同平台的ab包格式不同不能通用。

#### 如何打包场景

场景包为没有后缀名、不加密的普通ab包文件，即只要用Unity官方提供的ab包管理器即可。

点击菜单栏`Window/AssetsBundle Browser`（若没有此选项则到Package Manager中安装），其中含有名为`firstscene`、`loadingscene`和`gamescene`的三个包（若没有则右键创建，并将对应名称的场景文件拖拽到Configure右侧窗口），切换到`Build`选项卡，按需选择平台，填写ab包输出目录后点击Build即可。

构建完成的ab包存在与Assets同级目录下的执行目录内，将`firstscene`、`loadingscene`和`gamescene`拷贝到持久化目录或服务器上即可。

### 3.相关工具说明

##### 协议相关类生成工具

##### 热更新资源版本控制工具

### 4.其他