using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VirtualizingWrapPanelTest;

namespace VirtualizingWrapPanelTestRunner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new VirtualizingWrapPanelTest.VirtualizingWrapPanelTest().MeasureOverride_ArrangeOverride_ItemsPanelの場合_端までスクロールしている場合_Horizontal();
        }
    }
}
