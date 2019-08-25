using BetsPlanUIAutomation.ControlFinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;

namespace BetsPlanUIAutomation
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            this.updateMessage(string.Format("监控已启动..."));
            //查找挂机软件窗口对象
            var rootElement = ControlViewFinder.GetAutomationElementByProcessName("星图官方挂机软件");
            if (rootElement == null)
            {
                this.updateMessage("未查找到星图官方挂机软件窗口");
                return;
            }

            //查找自动投注选项卡Tab窗口
            var autoBetsFirstElement = ControlViewFinder.GetFirstChild(rootElement, 2);
            if (autoBetsFirstElement == null)
            {
                this.updateMessage("未查找到自动投注Tab窗口");
                return;
            }

            //自动投注Tab 兄弟窗口
            var autoBetsSiblingElement = TreeWalker.ControlViewWalker.GetFirstChild(autoBetsFirstElement);
            if (autoBetsSiblingElement == null) return;

            //自动投注Tab 窗口
            var autoBetsElement = TreeWalker.ControlViewWalker.GetNextSibling(autoBetsSiblingElement);
            if (autoBetsElement == null) return;

            //真实盈亏记录 兄弟窗口
            var winAutoSiblingElement = ControlViewFinder.GetFirstChild(autoBetsElement, 7);
            if (winAutoSiblingElement == null) return;

            //真实盈亏记录 窗口
            var winAutoElement = ControlViewFinder.GetNextSibling(winAutoSiblingElement, 3);
            if (winAutoElement == null) return;

            //真实盈亏记录窗口中 第一个元素
            var winAutoFirstElement = TreeWalker.ControlViewWalker.GetFirstChild(winAutoElement);
            if (winAutoFirstElement == null) return;

            //真实盈亏字段控件
            var winAutoControl = ControlViewFinder.GetNextSibling(winAutoFirstElement, 15);
            if (winAutoControl == null) return;
        }

        /// <summary>
        /// 定义更新文本委托类型
        /// </summary>
        /// <param name="message">更新消息内容</param>
        public delegate void updateTextDelegate(string message);
        /// <summary>
        /// 更新消息文本
        /// </summary>
        /// <param name="message">消息文本</param>
        private void updateMessage(string message)
        {
            this.Invoke(new updateTextDelegate(updateMessageControlText), message);
        }
        /// <summary>
        /// 更新消息控件的文本
        /// </summary>
        /// <param name="message">消息的内容</param>
        private void updateMessageControlText(string message)
        {
            this.txt_message.AppendText(message+"\r\n");
        }
    }
}
