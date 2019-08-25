using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace BetsPlanUIAutomation.ControlFinder
{
    public class ControlViewFinder
    {
        /// <summary>
        /// 按层次深度 查找控件中第一个控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="depth">要查找的深度，控件中的控件...</param>
        /// <returns>null or found AutomationElement</returns>
        public static AutomationElement GetFirstChild(AutomationElement parent, int depth)
        {
            //接收参数
            AutomationElement element = parent;
            for (var i = 0; i < depth; i++)
            {
                //查找第一个子元素
                element = TreeWalker.ControlViewWalker.GetFirstChild(element);
                if (element == null) return null;
                //Console.WriteLine("AutomationId=" + element.Current.AutomationId);
            }
            return element;
        }

        /// <summary>
        /// 按索引查找 当前元素的兄弟元素
        /// </summary>
        /// <param name="element">当前元素</param>
        /// <param name="index">要查找的索引</param>
        /// <returns>null or found AutomationElement</returns>
        public static AutomationElement GetNextSibling(AutomationElement element, int index)
        {
            //接收元素
            AutomationElement siblingElement = element;
            for (var i = 0; i < index; i++)
            {
                //查找最近的兄弟元素
                siblingElement = TreeWalker.ControlViewWalker.GetNextSibling(siblingElement);
                if (siblingElement == null) return null;
                //Console.WriteLine("AutomationId=" + siblingElement.Current.AutomationId);
            }

            return siblingElement;
        }

        /// <summary>
        /// 根据进程名称
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns>null or found AutomationElement</returns>
        public static AutomationElement GetAutomationElementByProcessName(string processName)
        {
            //需要查找的元素
            AutomationElement rootElement = null;

            // 根据进程名称查询进程对象
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length == 0) return rootElement;

            //查询桌面中所有的窗口
            AutomationElementCollection desktopChildren = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);

            foreach (AutomationElement automationElement in desktopChildren)
            {
                //根据进程Id查找
                if (automationElement.Current.ProcessId == processes[0].Id)
                {
                    rootElement = automationElement;
                    break;
                }
            }

            return rootElement;
        }
    }
}
