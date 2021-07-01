using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RegexUtil
{
    /// <summary>
    /// 使用正则表达式验证字符串是否匹配
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool IsMatch(string input, string pattern)
    {
        return Regex.IsMatch(input, pattern);
    }
}
