using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EPPTools.Utils
{
    /// <summary>
    /// Color工具集
    /// </summary>
    public class ColorUtils
    {
        /// <summary>
        /// 尝试根据字符串创建Color对象。字符串格式：[#]rr[ ]gg[ ]bb[ ][aa] [^]rr[ ]gg[ ]bb[ ][aa]  其中值取值可以为0-1、00-ff或0-255
        /// </summary>
        /// <param name="colorString">可能代表颜色信息的字符串</param>
        /// <returns>正确表示一个颜色信息则返回对应的Color对象，否则返回的Color信息均为0</returns>
        public static Color TryGetColor(string colorString)
        {
            if (!string.IsNullOrEmpty(colorString))
            {
                //所有可能的格式：[#]rr[ ]gg[ ]bb[ ][aa] [^]rr[ ]gg[ ]bb[ ][aa]  0-1、00-ff或0-255
                //包含空格 格式可能为带有空格的格式
                if (colorString.Contains(" "))
                {
                    string newColorString = string.Copy(colorString);
                    bool is0x = false;//是否是以#或^开头的十六进制形式
                    if (newColorString[0] == '#' || newColorString[0] == '^')
                    {
                        newColorString = newColorString.Substring(1);
                        is0x = true;
                    }
                    //去掉多余的空格并以此进行分割
                    Regex regex = new Regex(@"\s+");
                    string[] colorArray = regex.Split(newColorString);
                    if (colorArray.Length == 3)
                    {
                        return PieceColor(is0x, colorArray, false);
                    }
                    else if (colorArray.Length == 4)
                    {
                        return PieceColor(is0x, colorArray, true);
                    }
                }
                else
                {
                    //不含空格的格式。只能是 [#]rrggbb[aa] [^]rrggbb[aa] 且为十六进制格式
                    string newColorString = string.Copy(colorString);
                    string[] colorArray;
                    if (newColorString[0] == '#' || newColorString[0] == '^')
                    {
                        newColorString = newColorString.Substring(1);
                    }
                    if (newColorString.Length == 6)
                    {
                        colorArray = new string[3];
                        colorArray[0] = newColorString.Substring(0, 2);
                        colorArray[1] = newColorString.Substring(2, 2);
                        colorArray[2] = newColorString.Substring(4, 2);

                        return PieceColor(true, colorArray, false);
                    }
                    else if (newColorString.Length == 8)
                    {
                        colorArray = new string[4];
                        colorArray[0] = newColorString.Substring(0, 2);
                        colorArray[1] = newColorString.Substring(2, 2);
                        colorArray[2] = newColorString.Substring(4, 2);
                        colorArray[3] = newColorString.Substring(6, 2);

                        return PieceColor(true, colorArray, true);
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// 根据提供的数组进行颜色对象的创建
        /// </summary>
        /// <param name="is0x"></param>
        /// <param name="colorArray"></param>
        /// <param name="hasAlpha"></param>
        /// <returns></returns>
        private static Color PieceColor(bool is0x, string[] colorArray, bool hasAlpha)
        {
            //0-1 00-ff 0-255
            float r, g, b, a;
            //00-ff
            if (is0x)
            {
                r = int.Parse(colorArray[0], NumberStyles.HexNumber);
                g = int.Parse(colorArray[1], NumberStyles.HexNumber);
                b = int.Parse(colorArray[2], NumberStyles.HexNumber);
                if (hasAlpha)
                {
                    a = int.Parse(colorArray[3], NumberStyles.HexNumber);
                }
                else
                {
                    a = 255f;
                }

                return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
            //0-1
            if (colorArray[0].Contains(".") || colorArray[1].Contains(".") || colorArray[2].Contains("."))
            {
                r = float.Parse(colorArray[0]);
                g = float.Parse(colorArray[1]);
                b = float.Parse(colorArray[2]);
                if (hasAlpha)
                {
                    a = float.Parse(colorArray[3]);
                }
                else
                {
                    a = 1f;
                }

                return new Color(r, g, b, a);
            }
            //0-255
            r = int.Parse(colorArray[0]);
            g = int.Parse(colorArray[1]);
            b = int.Parse(colorArray[2]);
            if (hasAlpha)
            {
                a = int.Parse(colorArray[3]);
            }
            else
            {
                a = 255;
            }

            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
