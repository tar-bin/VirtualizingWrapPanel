using System.Windows;
using System.Windows.Controls;

namespace VirtualizingWrapPanelTest
{
    public class TestListBox : ListBox
    {
        static TestListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TestListBox), new FrameworkPropertyMetadata(typeof(TestListBox)));
        }
    }
}
