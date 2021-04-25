using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Billiard.Assistant
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        static KeyboardHook keyboardHook;

        int ra = 25;
        int index = 0;
        Point startMousePosition;
        Point endMousePosition;

        delegate void LogAppendDelegate(Color color, string text);

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            int ScreenWidth = SystemInformation.VirtualScreen.Width;
            int ScreenHeight = SystemInformation.VirtualScreen.Height;
            int x = ScreenWidth - Width - 20;
            int y = ScreenHeight / 3 - Height;
            Location = new Point(x, y);
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDownEvent += new KeyEventHandler(Form1_KeyDown);
            keyboardHook.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)ModifierKeys == (int)Keys.Alt)
            {
                switch (e.KeyValue)
                {
                    case (int)Keys.S:
                        startMousePosition = MousePosition;
                        index = 0;
                        Log($"已标记洞口坐标：{startMousePosition}", Color.Green);
                        break;
                    case (int)Keys.E:
                        endMousePosition = MousePosition;
                        index = 0;
                        Log($"已标记白球坐标：{endMousePosition}", Color.Green);
                        break;
                    case (int)Keys.F:
                        SetAimPos();
                        break;
                }
            }
        }

        private void SetAimPos()
        {
            if (index >= 4)
            {
                index = 1;
            }
            else
            {
                index++;
            }
            int x = 0;
            int y = 0;
            switch (index)
            {
                case 1:
                    x = (int)(endMousePosition.X + ra * ((endMousePosition.X - startMousePosition.X) / Math.Sqrt((endMousePosition.X - startMousePosition.X) * (endMousePosition.X - startMousePosition.X) + (endMousePosition.Y - startMousePosition.Y) * (endMousePosition.Y - startMousePosition.Y))));
                    y = (int)(endMousePosition.Y + ra * ((endMousePosition.Y - startMousePosition.Y) / Math.Sqrt((endMousePosition.X - startMousePosition.X) * (endMousePosition.X - startMousePosition.X) + (endMousePosition.Y - startMousePosition.Y) * (endMousePosition.Y - startMousePosition.Y))));
                    break;
                case 2:
                    x = (int)(endMousePosition.X - ra * ((-endMousePosition.X + startMousePosition.X) / Math.Sqrt((-endMousePosition.X + startMousePosition.X) * (-endMousePosition.X + startMousePosition.X) + (endMousePosition.Y - startMousePosition.Y) * (endMousePosition.Y - startMousePosition.Y))));
                    y = (int)(endMousePosition.Y + ra * ((endMousePosition.Y - startMousePosition.Y) / Math.Sqrt((-endMousePosition.X + startMousePosition.X) * (-endMousePosition.X + startMousePosition.X) + (endMousePosition.Y - startMousePosition.Y) * (endMousePosition.Y - startMousePosition.Y))));
                    break;
                case 3:
                    x = (int)(endMousePosition.X + ra * ((endMousePosition.X - startMousePosition.X) / Math.Sqrt((endMousePosition.X - startMousePosition.X) * (endMousePosition.X - startMousePosition.X) + (-endMousePosition.Y + startMousePosition.Y) * (-endMousePosition.Y + startMousePosition.Y))));
                    y = (int)(endMousePosition.Y - ra * ((-endMousePosition.Y + startMousePosition.Y) / Math.Sqrt((endMousePosition.X - startMousePosition.X) * (endMousePosition.X - startMousePosition.X) + (-endMousePosition.Y + startMousePosition.Y) * (-endMousePosition.Y + startMousePosition.Y))));
                    break;
                case 4:
                    x = (int)(endMousePosition.X - ra * ((-endMousePosition.X + startMousePosition.X) / Math.Sqrt((-endMousePosition.X + startMousePosition.X) * (-endMousePosition.X + startMousePosition.X) + (-endMousePosition.Y + startMousePosition.Y) * (-endMousePosition.Y + startMousePosition.Y))));
                    y = (int)(endMousePosition.Y - ra * ((-endMousePosition.Y + startMousePosition.Y) / Math.Sqrt((-endMousePosition.X + startMousePosition.X) * (-endMousePosition.X + startMousePosition.X) + (-endMousePosition.Y + startMousePosition.Y) * (-endMousePosition.Y + startMousePosition.Y))));
                    break;
            }
            SetCursorPos(x, y);
            Log($"【方案{index}】已设置瞄准坐标：{x}，{y}", Color.Green);
        }

        private void LogAppend(Color color, string text)
        {
            rtbConsole.SelectionColor = color;
            rtbConsole.AppendText(text);
            rtbConsole.AppendText("\n");
            rtbConsole.SelectionStart = rtbConsole.Text.Length;
            rtbConsole.ScrollToCaret();
        }

        public void Log(string text, Color color)
        {
            LogAppendDelegate la = LogAppend;
            rtbConsole.Invoke(la, color, DateTime.Now.ToString("HH:mm:ss ") + text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log("【作弊可耻，公平游戏！！！】", Color.Red);
            Log("【此项目仅限学习参考！！！】", Color.Red);
            Log("辅助器已开启", Color.Green);
        }
    }
}
