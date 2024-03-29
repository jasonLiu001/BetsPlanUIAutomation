﻿using BetsPlanUIAutomation.ControlFinder;
using System;
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

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            this.UpdateMessage("监控已启动...");

            var winAutoFirstElement = this.GetWinAutoFirstElement();
            if (winAutoFirstElement == null)
            {
                this.UpdateMessage("未查找到真实盈亏字段的兄弟控件");
                return;
            }

            //模拟盈亏金额
            var mockWinMoneyValue = this.GetMockWinMoneyValue(winAutoFirstElement);

            this.UpdateMessage($"当前真实盈亏：{mockWinMoneyValue}");
        }

        /// <summary>
        /// 获取模拟盈利余额
        /// </summary>
        /// <param name="parent">包含模拟盈亏的父窗口</param>
        /// <returns></returns>
        private string GetMockWinMoneyValue(AutomationElement parent)
        {
            //模拟盈亏字段控件
            var winAutoControl = ControlViewFinder.GetNextSibling(parent, 15);
            if (winAutoControl == null)
            {
                this.UpdateMessage("未查找到包含真实盈亏字段的控件");
                return string.Empty;
            }

            return winAutoControl.Current.Name;
        }

        /// <summary>
        /// 查找模拟盈亏字段所在的同级字段
        /// </summary>
        /// <returns>null 或者 AutomationElement</returns>
        private AutomationElement GetWinAutoFirstElement()
        {
            //查找挂机软件窗口对象
            var rootElement = ControlViewFinder.GetAutomationElementByProcessName("星图官方挂机软件");
            if (rootElement == null)
            {
                this.UpdateMessage("未查找到星图官方挂机软件窗口");
                return null;
            }

            //包含 自动投注选项卡Tab 的父窗口
            var autoBetsFirstElement = ControlViewFinder.GetFirstChild(rootElement, 2);
            if (autoBetsFirstElement == null)
            {
                this.UpdateMessage("未查找到包含自动投注Tab选项卡所在的父窗口");
                return null;
            }

            //包含 所有选项卡 的父窗口
            var autoBetsElement = TreeWalker.ControlViewWalker.GetLastChild(autoBetsFirstElement);
            if (autoBetsElement == null)
            {
                this.UpdateMessage("未查找到包含所有Tab选项卡的父窗口");
                return null;
            }

            //包含 表格 最大盈利 值的父窗口
            var winAutoSiblingElement = ControlViewFinder.GetFirstChild(autoBetsElement, 6);
            if (winAutoSiblingElement == null)
            {
                this.UpdateMessage("未查找到包含表格及最大盈利数据的父窗口");
                return null;
            }

            //包含 模拟盈亏记录 父窗口
            var winAutoElement = TreeWalker.ControlViewWalker.GetLastChild(winAutoSiblingElement);
            if (winAutoElement == null)
            {
                this.UpdateMessage("未查找到包含最大盈利数据的父窗口");
                return null;
            }

            //模拟盈亏记录窗口中 第一个元素 兄弟控件
            var winAutoFirstElement = TreeWalker.ControlViewWalker.GetFirstChild(winAutoElement);
            return winAutoFirstElement;
        }

        /// <summary>
        /// 定义更新文本委托类型
        /// </summary>
        /// <param name="message">更新消息内容</param>
        public delegate void UpdateTextDelegate(string message);
        /// <summary>
        /// 更新消息文本
        /// </summary>
        /// <param name="message">消息文本</param>
        private void UpdateMessage(string message)
        {
            this.Invoke(new UpdateTextDelegate(UpdateMessageControlText), message);
        }
        /// <summary>
        /// 更新消息控件的文本
        /// </summary>
        /// <param name="message">消息的内容</param>
        private void UpdateMessageControlText(string message)
        {
            this.Txt_Message.AppendText(message + "\r\n");
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {

        }
    }
}
