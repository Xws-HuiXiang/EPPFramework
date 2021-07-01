using System.Collections.Generic;

/// <summary>
/// 热更新版本号的json数据类
/// </summary>
public class HotfixVersionData
{
    public int ResVersion { get; set; }
    public int LuaVersion { get; set; }
}

/// <summary>
/// 服务器pb文件列表的json数据类
/// </summary>
public class PbFileListData
{
    public List<string> URLList { get; set; }
}