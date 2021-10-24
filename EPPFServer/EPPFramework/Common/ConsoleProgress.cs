using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPPFramework.Common
{
    public class ConsoleProgress
    {
        /// <summary>
        /// 进度条显示的类型
        /// </summary>
        public enum ProgressBarType
        {
            /// <summary>
            /// 字符
            /// </summary>
            Char,
            /// <summary>
            /// 颜色
            /// </summary>
            Color
        }

        public static void CPMain()
        {
            ConsoleProgress progress = new ConsoleProgress();
            Task.Run(() =>
            {
                int v = 0;
                while (v < 100)
                {
                    v++;

                    progress.Update(v / 100f, v.ToString());

                    Thread.Sleep(200);
                }
                progress.Update(1);
            });

            Console.ReadLine();
        }

        private float value = 0;
        /// <summary>
        /// 当前进度条的值
        /// </summary>
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                Update(value);
            }
        }

        /// <summary>
        /// 字符的最小数量
        /// </summary>
        public int MinCharAmount { get; set; }
        /// <summary>
        /// 字符的最大数量
        /// </summary>
        public int MaxCharAmount { get; set; }
        /// <summary>
        /// 进度条显示的字符
        /// </summary>
        public char ProgressChar { get; set; }
        /// <summary>
        /// 显示进度条的开始字符
        /// </summary>
        public char StartTag { get; set; }
        /// <summary>
        /// 显示进度条的结束字符
        /// </summary>
        public char EndTag { get; set; }

        private ProgressBarType barType = ProgressBarType.Char;
        /// <summary>
        /// 进度条类型
        /// </summary>
        public ProgressBarType BarType { get { return barType; } }
        private IDrawProgressBar drawProgressBar;

        /// <summary>
        /// 显示百分比
        /// </summary>
        public bool ShowPercentage { get; set; } = true;
        
        public ConsoleProgress()
        {
            this.MinCharAmount = 0;
            this.MaxCharAmount = 20;
            this.ProgressChar = '*';
            StartTag = '[';
            EndTag = ']';
            this.barType = ProgressBarType.Char;

            switch (this.barType)
            {
                case ProgressBarType.Char:
                    drawProgressBar = new CharProgressBar();
                    break;
                case ProgressBarType.Color:
                    drawProgressBar = new ColorProgressBar();
                    break;
                default:
                    Console.WriteLine("未知的类型：" + this.barType + "，将使用默认的字符类型显示");
                    drawProgressBar = new CharProgressBar();
                    break;
            }
        }

        /// <summary>
        /// 更新进度值
        /// </summary>
        /// <param name="value">进度值，0-1的值</param>
        /// <param name="msg">在进度条后面显示一个消息</param>
        public void Update(float value, string msg = null)
        {
            if(this.value != value)
            {
                this.value = value;
                
                int currentLineCursor = Console.CursorTop;//记录当前光标位置
                Console.SetCursorPosition(0, Console.CursorTop);//将光标至于当前行的开始位置
                Console.Write(new string(' ', Console.WindowWidth - 1));//用空格将当前行填满，相当于清除当前行
                Console.SetCursorPosition(0, Console.CursorTop);//将光标至于当前行的开始位置
                if (ShowPercentage)
                {
                    //进度百分比
                    Console.Write((value * 100).ToString("F6") + "%");
                }

                //绘制进度条
                drawProgressBar.DrawProgressBar(StartTag, EndTag, value, ProgressChar, MaxCharAmount, ConsoleColor.Red, ConsoleColor.Yellow);

                if (!string.IsNullOrEmpty(msg))
                {
                    Console.Write(msg);
                }
                //是否达到最大值
                if(this.Value >= 1)
                {
                    Console.WriteLine();
                }
                else
                {
                    Console.SetCursorPosition(0, currentLineCursor);//将光标恢复至开始时的位置
                }
            }
        }
    }

    public interface IDrawProgressBar
    {
        /// <summary>
        /// 绘制进度条
        /// </summary>
        /// <param name="value">进度的值。取值范围0-1</param>
        void DrawProgressBar(char startTag, char endTag, float value, char progressChar, int maxCharAmount, ConsoleColor foregroundColor, ConsoleColor backgroundColor);
    }

    public class CharProgressBar : IDrawProgressBar
    {
        public void DrawProgressBar(char startTag, char endTag, float value, char progressChar, int maxCharAmount, ConsoleColor fontColor, ConsoleColor backgroundColor)
        {
            int progressCharCount = (int)(value * maxCharAmount);
            Console.Write(startTag);
            Console.Write(new string(progressChar, progressCharCount));//用指定的字符填充指定的长度 相当于显示当前进度
            Console.Write(new string(' ', maxCharAmount - progressCharCount));
            Console.Write(endTag);
        }
    }

    public class ColorProgressBar : IDrawProgressBar
    {
        public void DrawProgressBar(char startTag, char endTag, float value, char progressChar, int maxCharAmount, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ConsoleColor backTempColor = Console.BackgroundColor;
            
            int progressCharCount = (int)(value * maxCharAmount);
            Console.Write(startTag);
            Console.BackgroundColor = foregroundColor;
            Console.Write(new string(' ', progressCharCount));//用指定的字符填充指定的长度 相当于显示当前进度
            Console.BackgroundColor = backgroundColor;
            Console.Write(new string(' ', maxCharAmount - progressCharCount));
            Console.BackgroundColor = backTempColor;
            Console.Write(endTag);
        }
    }
}
