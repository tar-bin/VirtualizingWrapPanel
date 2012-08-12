using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

using NUnit.Framework;

using Uhi.Libraries.Test;

using CodePlex.VirtualizingWrapPanel;

namespace VirtualizingWrapPanelTest
{
    [TestFixture]
    public class VirtualizingWrapPanelTest
    {
        #region ItemSize

        #region ItemWidth

        #region 正常テスト
        [Test, STAThread]
        public void ItemWidth_正常テスト()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemWidth.Is(double.NaN);

            panel.ItemWidth = 100;
            panel.ItemWidth.Is(100);
        }
        #endregion
        
        #region ゼロ
        [Test, STAThread]
        public void ItemWidth_ゼロ()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemWidth = 0;
            panel.ItemWidth.Is(0);
        }
        #endregion
        
        #region 最大値
        [Test, STAThread]
        public void ItemWidth_最大値()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemWidth = double.MaxValue;
            panel.ItemWidth.Is(double.MaxValue);
        }
        #endregion

        #region マイナス値
        [Test, STAThread, ExpectedException(typeof(ArgumentException))]
        public void ItemWidth_マイナス値()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemWidth = -1;
        }
        #endregion

        #region 無限大
        [Test, STAThread, ExpectedException(typeof(ArgumentException))]
        public void ItemWidth_無限大()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemWidth = double.PositiveInfinity;
        }
        #endregion

        #endregion

        #region ItemHeight

        #region 正常テスト
        [Test, STAThread]
        public void ItemHeight_正常テスト()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemHeight.Is(double.NaN);

            panel.ItemHeight = 100;
            panel.ItemHeight.Is(100);
        }
        #endregion

        #region ゼロ
        [Test, STAThread]
        public void ItemHeight_ゼロ()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemHeight = 0;
            panel.ItemHeight.Is(0);
        }
        #endregion

        #region 最大値
        [Test, STAThread]
        public void ItemHeight_最大値()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemHeight = double.MaxValue;
            panel.ItemHeight.Is(double.MaxValue);
        }
        #endregion

        #region マイナス値
        [Test, STAThread, ExpectedException(typeof(ArgumentException))]
        public void ItemHeight_マイナス値()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemHeight = -1;
        }
        #endregion

        #region 無限大
        [Test, STAThread, ExpectedException(typeof(ArgumentException))]
        public void ItemHeight_無限大()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemHeight = double.PositiveInfinity;
        }
        #endregion

        #endregion

        #endregion

        #region Orientation

        #region 正常テスト
        [Test, STAThread]
        public void Orientation_正常テスト()
        {
            var panel = new VirtualizingWrapPanel();
            panel.Orientation.Is(Orientation.Horizontal);

            panel.Orientation = Orientation.Vertical;
            panel.Orientation.Is(Orientation.Vertical);
        }
        #endregion

        #endregion

        #region Helper Methods

        #region FindItemHostPanel

        private static Panel FindItemsHostPanel(ItemsControl itemsControl)
        {
            return Find(itemsControl.ItemContainerGenerator, itemsControl);
        }

        private static Panel Find(IItemContainerGenerator generator, DependencyObject control)
        {
            var count = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(control, i);
                if (IsItemsHostPanel(generator, child))
                    return child as Panel;

                var panel = Find(generator, child);
                if (panel != null)
                    return panel;
            }
            return null;
        }

        private static bool IsItemsHostPanel(IItemContainerGenerator generator, DependencyObject target)
        {
            var panel = target as Panel;
            return panel != null && panel.IsItemsHost && generator == generator.GetItemContainerGeneratorForPanel(panel);
        }

        #endregion

        #endregion

        #region MesureOverride, ArrangeOverride

        #region 単独で使用された場合

        #region Horizontalの場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_Horizontalの場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            
            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 100));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 100, 50));
            containerLayouts[2].Is(new Rect(0, 50, 100, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 50));
        }
        #endregion

        #region Verticalの場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_Verticalの場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(100, 150));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(0, 50, 100, 50));
            containerLayouts[2].Is(new Rect(0, 100, 100, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 50));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 100));
        }
        #endregion

        #region ItemWidthを指定している場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_ItemWidthを指定している場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.ItemWidth = 80;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(160, 100));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 80, 50));
            containerLayouts[1].Is(new Rect(80, 0, 80, 50));
            containerLayouts[2].Is(new Rect(0, 50, 80, 50));
            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(80, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 50));
        }
        #endregion

        #region ItemHeightを指定している場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_ItemHeightを指定している場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            r1.VerticalAlignment = VerticalAlignment.Top;
            r2.VerticalAlignment = VerticalAlignment.Top;
            r3.VerticalAlignment = VerticalAlignment.Top;

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.ItemHeight = 80;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 160));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 80));
            containerLayouts[1].Is(new Rect(100, 0, 100, 80));
            containerLayouts[2].Is(new Rect(0, 80, 100, 80));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 80));
        }
        #endregion

        #region スクロールしている場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_スクロールしている場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            //panel.Width = 200;
            //panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);

            var scroll = new ScrollViewer();
            scroll.Width = 200;
            scroll.Height = 200;
            scroll.Content = panel;
            scroll.ScrollToEnd();

            var window = new Window();
            window.Content = scroll;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(150, 500));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200 - SystemParameters.VerticalScrollBarWidth, double.PositiveInfinity));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 150, 100));
            containerLayouts[1].Is(new Rect(0, 100, 150, 100));
            containerLayouts[2].Is(new Rect(0, 200, 150, 100));
            containerLayouts[3].Is(new Rect(0, 300, 150, 100));
            containerLayouts[4].Is(new Rect(0, 400, 150, 100));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(5);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 100));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 200));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 300));

            var r5Offset = (Vector)r5.AsDynamic().VisualOffset;
            r5Offset.Is(new Vector(0, 400));
        }
        #endregion

        #region 子要素の合計サイズがパネルより小さい場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_子要素の合計サイズがパネルより小さい場合()
        {
            var r1 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(160, 160));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 80, 80));
            containerLayouts[1].Is(new Rect(80, 0, 80, 80));
            containerLayouts[2].Is(new Rect(0, 80, 80, 80));
            containerLayouts[3].Is(new Rect(80, 80, 80, 80));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(80, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 80));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(80, 80));
        }
        #endregion

        #region 子要素の合計サイズとパネルのサイズが同じ場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_子要素の合計サイズとパネルのサイズが同じ場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 200));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 100, 100));
            containerLayouts[1].Is(new Rect(100, 0, 100, 100));
            containerLayouts[2].Is(new Rect(0, 100, 100, 100));
            containerLayouts[3].Is(new Rect(100, 100, 100, 100));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 100));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(100, 100));
        }
        #endregion

        #region 子要素の合計サイズがパネルより大きい場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_子要素の合計サイズがパネルより大きい場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(150, 600));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 150, 150));
            containerLayouts[1].Is(new Rect(0, 150, 150, 150));
            containerLayouts[2].Is(new Rect(0, 300, 150, 150));
            containerLayouts[3].Is(new Rect(0, 450, 150, 150));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 150));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 300));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 450));
        }
        #endregion

        #region 子要素単体のサイズとパネルのサイズが同じ場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_子要素単体のサイズとパネルのサイズが同じ場合()
        {
            var r1 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 200, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 300));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 50, 50));
            containerLayouts[1].Is(new Rect(0, 50, 200, 200));
            containerLayouts[2].Is(new Rect(0, 250, 50, 50));
            containerLayouts[3].Is(new Rect(50, 250, 50, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 50));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 250));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(50, 250));
        }
        #endregion

        #region 子要素単体のサイズがパネルより大きい場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_子要素単体のサイズがパネルより大きい場合()
        {
            var r1 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 300, Height = 300, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(300, 400));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 50, 50));
            containerLayouts[1].Is(new Rect(0, 50, 300, 300));
            containerLayouts[2].Is(new Rect(0, 350, 50, 50));
            containerLayouts[3].Is(new Rect(50, 350, 50, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 50));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 350));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(50, 350));
        }
        #endregion

        #region 子要素のサイズがまちまちな場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_子要素のサイズがまちまちな場合()
        {
            var r1 = new Rectangle() { Width = 30, Height = 30, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 70, Height = 70, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 90, Height = 90, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(150, 160));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 30, 30));
            containerLayouts[1].Is(new Rect(30, 0, 50, 50));
            containerLayouts[2].Is(new Rect(80, 0, 70, 70));
            containerLayouts[3].Is(new Rect(0, 70, 90, 90));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(30, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(80, 0));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 70));
        }
        #endregion

        #region Viewportが長方形の場合_Horizontal
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_Viewportが長方形の場合_Horizontal()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 50, Height = 120, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 80, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 20, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 70, Fill = new SolidColorBrush(Colors.Azure) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 100;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(180, 270));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 100));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 50, 120));
            containerLayouts[2].Is(new Rect(0, 120, 150, 80));
            containerLayouts[3].Is(new Rect(0, 200, 80, 20));
            containerLayouts[4].Is(new Rect(80, 200, 100, 70));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(5);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 120));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 200));

            var r5Offset = (Vector)r5.AsDynamic().VisualOffset;
            r5Offset.Is(new Vector(80, 200));
        }
        #endregion

        #region Viewportが長方形の場合_Vertical
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_単独で使用された場合_Viewportが長方形の場合_Vertical()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 50, Height = 120, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 80, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 20, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 70, Fill = new SolidColorBrush(Colors.Azure) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 100;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(400, 120));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 100));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 50, 120));
            containerLayouts[2].Is(new Rect(150, 0, 150, 80));
            containerLayouts[3].Is(new Rect(150, 80, 80, 20));
            containerLayouts[4].Is(new Rect(300, 0, 100, 70));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(5);

            var r1Offset = (Vector)r1.AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)r2.AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)r3.AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(150, 0));

            var r4Offset = (Vector)r4.AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(150, 80));

            var r5Offset = (Vector)r5.AsDynamic().VisualOffset;
            r5Offset.Is(new Vector(300, 0));
        }
        #endregion

        #endregion

        #region ItemsPanelの場合

        #region Horizontalの場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_Horizontalの場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 100));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 100, 50));
            containerLayouts[2].Is(new Rect(0, 50, 100, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 50));
        }
        #endregion

        #region Verticalの場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_Verticalの場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Vertical;

            window.UpdateLayout();

            window.Close();

            var panelType = panel.GetType();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(100, 150));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(0, 50, 100, 50));
            containerLayouts[2].Is(new Rect(0, 100, 100, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 50));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 100));
        }
        #endregion

        #region ItemWidthを指定している場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_ItemWidthを指定している場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;
            panel.ItemWidth = 80;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(160, 100));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 80, 50));
            containerLayouts[1].Is(new Rect(80, 0, 80, 50));
            containerLayouts[2].Is(new Rect(0, 50, 80, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(80, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 50));
        }
        #endregion

        #region ItemHeightを指定している場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_ItemHeightを指定している場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            r1.VerticalAlignment = VerticalAlignment.Top;
            r2.VerticalAlignment = VerticalAlignment.Top;
            r3.VerticalAlignment = VerticalAlignment.Top;

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;
            panel.ItemHeight = 80;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 160));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 80));
            containerLayouts[1].Is(new Rect(100, 0, 100, 80));
            containerLayouts[2].Is(new Rect(0, 80, 100, 80));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 80));
        }
        #endregion

        #region 端までスクロールしている場合_Horizontal
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_端までスクロールしている場合_Horizontal()
        {
            var r1 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };

            var itemsControl = new TestListBox();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);
            
            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;
            panel.ScrollOwner.ScrollToEnd();
            
            window.UpdateLayout();
            
            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(150, 500));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200 - SystemParameters.VerticalScrollBarWidth, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 150, 100));
            containerLayouts[1].Is(new Rect(0, 100, 150, 100));
            containerLayouts[2].Is(new Rect(0, 200, 150, 100));
            containerLayouts[3].Is(new Rect(0, 300, 150, 100));
            containerLayouts[4].Is(new Rect(0, 400, 150, 100));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var generator = itemsControl.ItemContainerGenerator;

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 200 - panel.ScrollOwner.ScrollableHeight));

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 300 - panel.ScrollOwner.ScrollableHeight));

            var r5Offset = (Vector)generator.ContainerFromItem(r5).AsDynamic().VisualOffset;
            r5Offset.Is(new Vector(0, 400 - panel.ScrollOwner.ScrollableHeight));
        }
        #endregion

        #region 途中までスクロールしている場合_Horizontal
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_途中までスクロールしている場合_Horizontal()
        {
            var r1 = new Rectangle() { Width = 20, Height = 80, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 80, Height = 20, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 300, Height = 200, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 200, Height = 200, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.Azure) };

            var itemsControl = new TestListBox();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var offset = 200;
            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;
            panel.SetVerticalOffset(offset);

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(300, 630));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200 - SystemParameters.VerticalScrollBarWidth, 200 - SystemParameters.HorizontalScrollBarHeight));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 20, 80));
            containerLayouts[1].Is(new Rect(20, 0, 80, 20));
            containerLayouts[2].Is(new Rect(0, 80, 300, 200));
            containerLayouts[3].Is(new Rect(0, 280, 200, 200));
            containerLayouts[4].Is(new Rect(0, 480, 100, 150));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 80 - offset));

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 280 - offset));
        }
        #endregion

        #region 端までスクロールしている場合_Vertical
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_端までスクロールしている場合_Vertical()
        {
            var r1 = new Rectangle() { Width = 300, Height = 300, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 200, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 10, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 80, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 200, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };

            var itemsControl = new TestListBox();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Vertical;

            window.UpdateLayout();

            var offset = 50;
            panel.ScrollOwner.ScrollToRightEnd();
            panel.SetVerticalOffset(offset);

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(800, 300));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200 - SystemParameters.VerticalScrollBarWidth, 200 - SystemParameters.HorizontalScrollBarHeight));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 300, 300));
            containerLayouts[1].Is(new Rect(300, 0, 200, 200));
            containerLayouts[2].Is(new Rect(500, 0, 10, 50));
            containerLayouts[3].Is(new Rect(500, 50, 100, 80));
            containerLayouts[4].Is(new Rect(600, 0, 200, 100));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(1);

            var generator = itemsControl.ItemContainerGenerator;

            var r5Offset = (Vector)generator.ContainerFromItem(r5).AsDynamic().VisualOffset;
            r5Offset.Is(new Vector(600 - panel.ScrollOwner.ScrollableWidth, 0 - offset));
        }
        #endregion

        #region 途中までスクロールしている場合_Vertical
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_途中までスクロールしている場合_Vertical()
        {
            var r1 = new Rectangle() { Width = 300, Height = 300, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 200, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 10, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 80, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 200, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };

            var itemsControl = new TestListBox();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);
            
            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            
            var offset = 550;
            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Vertical;

            window.UpdateLayout();

            panel.SetHorizontalOffset(offset);

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(800, 300));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200 - SystemParameters.VerticalScrollBarWidth, 200 - SystemParameters.HorizontalScrollBarHeight));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 300, 300));
            containerLayouts[1].Is(new Rect(300, 0, 200, 200));
            containerLayouts[2].Is(new Rect(500, 0, 10, 50));
            containerLayouts[3].Is(new Rect(500, 50, 100, 80));
            containerLayouts[4].Is(new Rect(600, 0, 200, 100));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(500 - offset, 50));

            var r5Offset = (Vector)generator.ContainerFromItem(r5).AsDynamic().VisualOffset;
            r5Offset.Is(new Vector(600 - offset, 0));
        }
        #endregion

        #region DataTemplateを指定している場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_DataTemplateを指定している場合()
        {
            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var panelTemplate = new ItemsPanelTemplate();
            panelTemplate.VisualTree = panelFactory;

            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.WidthProperty, 180.0);
            borderFactory.SetValue(Border.HeightProperty, 150.0);
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            borderFactory.SetValue(Border.BorderBrushProperty, new SolidColorBrush(Colors.Black));

            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = borderFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = panelTemplate;
            itemsControl.ItemTemplate = dataTemplate;
            itemsControl.ItemsSource = new string[] { "a", "b", "c", "d" };

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl);

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(180, 600));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 180, 150));
            containerLayouts[1].Is(new Rect(0, 150, 180, 150));
            containerLayouts[2].Is(new Rect(0, 300, 180, 150));
            containerLayouts[3].Is(new Rect(0, 450, 180, 150));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromIndex(0).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromIndex(1).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 150));
        }
        #endregion

        #region 子要素の合計サイズがパネルより小さい場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_子要素の合計サイズがパネルより小さい場合()
        {
            var r1 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 80, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;
            
            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(160, 160));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 80, 80));
            containerLayouts[1].Is(new Rect(80, 0, 80, 80));
            containerLayouts[2].Is(new Rect(0, 80, 80, 80));
            containerLayouts[3].Is(new Rect(80, 80, 80, 80));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(80, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 80));

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(80, 80));
        }
        #endregion

        #region 子要素の合計サイズとパネルのサイズが同じ場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_子要素の合計サイズとパネルのサイズが同じ場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 200));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 100, 100));
            containerLayouts[1].Is(new Rect(100, 0, 100, 100));
            containerLayouts[2].Is(new Rect(0, 100, 100, 100));
            containerLayouts[3].Is(new Rect(100, 100, 100, 100));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(0, 100));

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(100, 100));
        }
        #endregion

        #region 子要素の合計サイズがパネルより大きい場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_子要素の合計サイズがパネルより大きい場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(150, 600));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 150, 150));
            containerLayouts[1].Is(new Rect(0, 150, 150, 150));
            containerLayouts[2].Is(new Rect(0, 300, 150, 150));
            containerLayouts[3].Is(new Rect(0, 450, 150, 150));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 150));
        }
        #endregion

        #region 子要素単体のサイズとパネルのサイズが同じ場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_子要素単体のサイズとパネルのサイズが同じ場合()
        {
            var r1 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 200, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(200, 300));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 50, 50));
            containerLayouts[1].Is(new Rect(0, 50, 200, 200));
            containerLayouts[2].Is(new Rect(0, 250, 50, 50));
            containerLayouts[3].Is(new Rect(50, 250, 50, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 50));
        }
        #endregion

        #region 子要素単体のサイズがパネルより大きい場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_子要素単体のサイズがパネルより大きい場合()
        {
            var r1 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 300, Height = 300, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(300, 400));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 50, 50));
            containerLayouts[1].Is(new Rect(0, 50, 300, 300));
            containerLayouts[2].Is(new Rect(0, 350, 50, 50));
            containerLayouts[3].Is(new Rect(50, 350, 50, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;
            
            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(0, 50));
        }
        #endregion

        #region 子要素のサイズがまちまちな場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_子要素のサイズがまちまちな場合()
        {
            var r1 = new Rectangle() { Width = 30, Height = 30, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 50, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 70, Height = 70, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 90, Height = 90, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(150, 160));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 200));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(4);
            containerLayouts[0].Is(new Rect(0, 0, 30, 30));
            containerLayouts[1].Is(new Rect(30, 0, 50, 50));
            containerLayouts[2].Is(new Rect(80, 0, 70, 70));
            containerLayouts[3].Is(new Rect(0, 70, 90, 90));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(30, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(80, 0));

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(0, 70));
        }
        #endregion

        #region availableSizeが無制限の場合
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_availableSizeが無制限の場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.Aqua) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(300, 50));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(itemsControl.ActualWidth, itemsControl.ActualHeight));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(3);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 100, 50));
            containerLayouts[2].Is(new Rect(200, 0, 100, 50));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(3);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(200, 0));
        }
        #endregion

        #region Viewportが長方形の場合_Horizontal
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_Viewportが長方形の場合_Horizontal()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 50, Height = 120, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 80, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 20, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 70, Fill = new SolidColorBrush(Colors.Azure) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 100;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Horizontal;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(180, 270));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 100));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 50, 120));
            containerLayouts[2].Is(new Rect(0, 120, 150, 80));
            containerLayouts[3].Is(new Rect(0, 200, 80, 20));
            containerLayouts[4].Is(new Rect(80, 200, 100, 70));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(2);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));
        }
        #endregion

        #region Viewportが長方形の場合_Vertical
        [Test, STAThread]
        public void MeasureOverride_ArrangeOverride_ItemsPanelの場合_Viewportが長方形の場合_Vertical()
        {
            var r1 = new Rectangle() { Width = 100, Height = 50, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 50, Height = 120, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 80, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 20, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 70, Fill = new SolidColorBrush(Colors.Azure) };

            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 100;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.Orientation = Orientation.Vertical;

            window.UpdateLayout();

            window.Close();

            var extent = (Size)panel.AsDynamic().extent;
            extent.Is(new Size(400, 120));

            var viewport = (Size)panel.AsDynamic().viewport;
            viewport.Is(new Size(200, 100));

            var containerLayouts = panel.AsDynamic().containerLayouts as Dictionary<int, Rect>;
            containerLayouts.Count.Is(5);
            containerLayouts[0].Is(new Rect(0, 0, 100, 50));
            containerLayouts[1].Is(new Rect(100, 0, 50, 120));
            containerLayouts[2].Is(new Rect(150, 0, 150, 80));
            containerLayouts[3].Is(new Rect(150, 80, 80, 20));
            containerLayouts[4].Is(new Rect(300, 0, 100, 70));

            var internalChildren = panel.AsDynamic().InternalChildren as UIElementCollection;
            internalChildren.Count.Is(4);

            var generator = itemsControl.ItemContainerGenerator;

            var r1Offset = (Vector)generator.ContainerFromItem(r1).AsDynamic().VisualOffset;
            r1Offset.Is(new Vector(0, 0));

            var r2Offset = (Vector)generator.ContainerFromItem(r2).AsDynamic().VisualOffset;
            r2Offset.Is(new Vector(100, 0));

            var r3Offset = (Vector)generator.ContainerFromItem(r3).AsDynamic().VisualOffset;
            r3Offset.Is(new Vector(150, 0));

            var r4Offset = (Vector)generator.ContainerFromItem(r4).AsDynamic().VisualOffset;
            r4Offset.Is(new Vector(150, 80));
        }
        #endregion

        #endregion

        #endregion

        #region ContainerSizeForIndex

        #region 単独で使用された場合

        #region UIElementの場合
        [Test, STAThread]
        public void ContainerSizeForIndex_単独で使用された場合_UIElementの場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.Children.Add(new UIElement());

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(0, 0));
        }
        #endregion

        #region FrameworkElementの場合
        [Test, STAThread]
        public void ContainerSizeForIndex_単独で使用された場合_FrameworkElementの場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.Children.Add(new FrameworkElement() { Width = 50, Height = 100 });

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(50, 100));
        }
        #endregion

        #region indexが無効な場合
        [Test, STAThread]
        public void ContainerSizeForIndex_単独で使用された場合_indexが無効な場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.Children.Add(new UIElement());

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(1);
            size.Is(new Size(16, 16));
        }
        #endregion

        #endregion

        #region ItemsPanelの場合

        #region UIElementの場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemsPanelの場合_UIElementの場合()
        {
            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = template;

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            itemsControl.Items.Add(new UIElement());

            var panel = FindItemsHostPanel(itemsControl);

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(0, 0));
        }
        #endregion

        #region UIElementでない場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemsPanelの場合_UIElementでない場合()
        {
            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = template;
            
            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            itemsControl.Items.Add("foo");

            var panel = FindItemsHostPanel(itemsControl);

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(16, 16));
        }
        #endregion

        #region FrameworkElementの場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemsPanelの場合_FrameworkElementの場合()
        {
            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = template;
            
            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            itemsControl.Items.Add(new FrameworkElement() { Width = 50, Height = 100 });

            var panel = FindItemsHostPanel(itemsControl);

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(50, 100));
        }
        #endregion

        #region indexが無効な場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemsPanelの場合_indexが無効な場合()
        {
            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = template;

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            itemsControl.Items.Add(new UIElement());

            var panel = FindItemsHostPanel(itemsControl);

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(1);
            size.Is(new Size(16, 16));
        }
        #endregion

        #region DataTemplateを指定している場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemsPanelの場合_DataTemplateを指定している場合()
        {
            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var panelTemplate = new ItemsPanelTemplate();
            panelTemplate.VisualTree = panelFactory;

            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.WidthProperty, 100.0);
            borderFactory.SetValue(Border.HeightProperty, 200.0);

            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = borderFactory;

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = panelTemplate;
            itemsControl.ItemTemplate = dataTemplate;
            itemsControl.ItemsSource = new string[] { "a" };

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl);

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(100, 200));
        }
        #endregion

        #endregion

        #region prevSizeのテスト
        [Test, STAThread]
        public void ContainerSizeForIndex_prevSizeのテスト()
        {
            var template = new ItemsPanelTemplate();
            template.VisualTree = new FrameworkElementFactory(typeof(VirtualizingWrapPanel), "panel");

            var itemsControl = new ItemsControl();
            itemsControl.ItemsPanel = template;

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            itemsControl.Items.Add(new FrameworkElement() { Width = 50, Height = 100 });
            itemsControl.Items.Add("foo");

            var panel = FindItemsHostPanel(itemsControl);

            var size0 = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size0.Is(new Size(50, 100));

            var size1 = (Size)panel.AsDynamic().ContainerSizeForIndex(1);
            size1.Is(new Size(50, 100));
        }
        #endregion

        #region ItemWidthを指定している場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemWidthを指定している場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemWidth = 80;
            panel.Children.Add(new FrameworkElement() { Width = 50, Height = 100 });

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(80, 100));
        }
        #endregion

        #region ItemHeightを指定している場合
        [Test, STAThread]
        public void ContainerSizeForIndex_ItemHeightを指定している場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.ItemHeight = 80;
            panel.Children.Add(new FrameworkElement() { Width = 50, Height = 100 });

            var size = (Size)panel.AsDynamic().ContainerSizeForIndex(0);
            size.Is(new Size(50, 80));
        }
        #endregion

        #endregion

        #region IScrollInfo Members

        #region Extent

        #region 正常ケース
        [Test, STAThread]
        public void Extent_正常ケース()
        {
            var size = new Size(100, 100);
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = size;
            panel.ExtentWidth.Is(100);
            panel.ExtentHeight.Is(100);
        }
        #endregion

        #endregion

        #region Viewport

        #region 正常ケース
        [Test, STAThread]
        public void Viewport_正常ケース()
        {
            var size = new Size(100, 100);
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().viewport = size;
            panel.ViewportWidth.Is(100);
            panel.ViewportHeight.Is(100);
        }
        #endregion

        #endregion

        #region Offset

        #region 正常ケース
        [Test, STAThread]
        public void Offset_正常ケース()
        {
            var point = new Point(100, 100);
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().offset = point;
            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(100);
        }
        #endregion

        #endregion

        #region ScrollOwner

        #region 正常ケース
        [Test, STAThread]
        public void ScrollOwner_正常ケース()
        {
            var scroll = new ScrollViewer();
            var panel = new VirtualizingWrapPanel();
            panel.ScrollOwner = scroll;
            panel.ScrollOwner.Is(scroll);
        }
        #endregion

        #endregion

        #region CanHorizontalScroll

        #region 正常ケース
        [Test, STAThread]
        public void CanHorizontalScroll_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.CanHorizontallyScroll = true;
            panel.CanHorizontallyScroll.Is(true);
        }
        #endregion

        #endregion

        #region CanVerticalScroll

        #region 正常ケース
        [Test, STAThread]
        public void CanVerticalScroll_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.CanVerticallyScroll = true;
            panel.CanVerticallyScroll.Is(true);
        }
        #endregion

        #endregion

        #region LineUp

        #region 正常ケース
        [Test, STAThread]
        public void LineUp_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(50, 50);

            panel.LineUp();

            panel.HorizontalOffset.Is(50);
            panel.VerticalOffset.Is(50 - SystemParameters.ScrollHeight);
        }
        #endregion

        #endregion

        #region LineDown

        #region 正常ケース
        [Test, STAThread]
        public void LineDown_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(50, 50);

            panel.LineDown();

            panel.HorizontalOffset.Is(50);
            panel.VerticalOffset.Is(50 + SystemParameters.ScrollHeight);
        }
        #endregion

        #endregion

        #region LineLeft

        #region 正常ケース
        [Test, STAThread]
        public void LineLeft_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(50, 50);

            panel.LineLeft();

            panel.HorizontalOffset.Is(50 - SystemParameters.ScrollWidth);
            panel.VerticalOffset.Is(50);
        }
        #endregion

        #endregion

        #region LineRight

        #region 正常ケース
        [Test, STAThread]
        public void LineRight_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(50, 50);

            panel.LineRight();

            panel.HorizontalOffset.Is(50 + SystemParameters.ScrollWidth);
            panel.VerticalOffset.Is(50);
        }
        #endregion

        #endregion

        #region PageUp

        #region 正常ケース
        [Test, STAThread]
        public void PageUp_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.PageUp();

            panel.HorizontalOffset.Is(200);
            panel.VerticalOffset.Is(100);
        }
        #endregion

        #endregion

        #region PageDown

        #region 正常ケース
        [Test, STAThread]
        public void PageDown_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.PageDown();

            panel.HorizontalOffset.Is(200);
            panel.VerticalOffset.Is(300);
        }
        #endregion

        #endregion

        #region PageLeft

        #region 正常ケース
        [Test, STAThread]
        public void PageLeft_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.PageLeft();

            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(200);
        }
        #endregion

        #endregion

        #region PageRight

        #region 正常ケース
        [Test, STAThread]
        public void PageRight_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.PageRight();

            panel.HorizontalOffset.Is(300);
            panel.VerticalOffset.Is(200);
        }
        #endregion

        #endregion

        #region MouseWheelUp

        #region 正常ケース
        [Test, STAThread]
        public void MouseWheelUp_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.MouseWheelUp();

            panel.HorizontalOffset.Is(200);
            panel.VerticalOffset.Is(200 - SystemParameters.ScrollHeight * SystemParameters.WheelScrollLines);
        }
        #endregion

        #endregion

        #region MouseWheelDown

        #region 正常ケース
        [Test, STAThread]
        public void MouseWheelDown_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.MouseWheelDown();

            panel.HorizontalOffset.Is(200);
            panel.VerticalOffset.Is(200 + SystemParameters.ScrollHeight * SystemParameters.WheelScrollLines);
        }
        #endregion

        #endregion

        #region MouseWheelLeft

        #region 正常ケース
        [Test, STAThread]
        public void MouseWheelLeft_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.MouseWheelLeft();

            panel.HorizontalOffset.Is(200 - SystemParameters.ScrollWidth * SystemParameters.WheelScrollLines);
            panel.VerticalOffset.Is(200);
        }
        #endregion

        #endregion

        #region MouseWheelRight

        #region 正常ケース
        [Test, STAThread]
        public void MouseWheelRight_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(400, 400);
            panel.AsDynamic().viewport = new Size(100, 100);
            panel.AsDynamic().offset = new Point(200, 200);

            panel.MouseWheelRight();

            panel.HorizontalOffset.Is(200 + SystemParameters.ScrollWidth * SystemParameters.WheelScrollLines);
            panel.VerticalOffset.Is(200);
        }
        #endregion

        #endregion

        #region MakeVisible

        #region 単独で使用された場合

        #region Horizontalの場合

        #region 上に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_上に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetVerticalOffset(50);

            var ret = panel.MakeVisible(r2, default(Rect));
            ret.Is(new Rect(100, 0, 100, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 下に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_下に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetVerticalOffset(50);

            var ret = panel.MakeVisible(r6, default(Rect));
            ret.Is(new Rect(100, 200, 100, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(100);
        }
        #endregion

        #region 左に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_左に切れている場合()
        {
            var r1 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            
            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetHorizontalOffset(100);

            var ret = panel.MakeVisible(r2, default(Rect));
            ret.Is(new Rect(0, 100, 200, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 右に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_右に切れている場合()
        {
            var r1 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetVerticalOffset(100);

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(new Rect(0, 200, 200, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(100);
        }
        #endregion

        #region 表示範囲内で切れていない場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_表示範囲内で切れていない場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetVerticalOffset(50);

            var ret = panel.MakeVisible(r4, default(Rect));
            ret.Is(new Rect(100, 100, 100, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(50);
        }
        #endregion

        #region 表示範囲にない場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_表示範囲にない場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };
            var r7 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Bisque) };
            var r8 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Black) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);
            panel.Children.Add(r7);
            panel.Children.Add(r8);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var ret = panel.MakeVisible(r8, default(Rect));
            ret.Is(new Rect(100, 300, 100, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(200);
        }
        #endregion

        #region 子要素でない場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Horizontalの場合_子要素でない場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            
            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            
            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var ret = panel.MakeVisible(r4, default(Rect));
            ret.Is(Rect.Empty);

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #endregion

        #region Verticalの場合

        #region 上に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_上に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };
            
            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            
            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetVerticalOffset(100);

            var ret = panel.MakeVisible(r2, default(Rect));
            ret.Is(new Rect(100, 0, 100, 200));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 下に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_下に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetHorizontalOffset(100);

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(new Rect(200, 0, 100, 200));

            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 左に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_左に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetHorizontalOffset(50);

            var ret = panel.MakeVisible(r2, default(Rect));
            ret.Is(new Rect(0, 100, 100, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 右に切れている場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_右に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetHorizontalOffset(50);

            var ret = panel.MakeVisible(r6, default(Rect));
            ret.Is(new Rect(200, 100, 100, 100));

            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 表示範囲内で切れていない場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_表示範囲内で切れていない場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            panel.SetHorizontalOffset(50);

            var ret = panel.MakeVisible(r4, default(Rect));
            ret.Is(new Rect(100, 100, 100, 100));

            panel.HorizontalOffset.Is(50);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 表示範囲にない場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_表示範囲にない場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Beige) };
            var r7 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Bisque) };
            var r8 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Black) };

            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            panel.Children.Add(r4);
            panel.Children.Add(r5);
            panel.Children.Add(r6);
            panel.Children.Add(r7);
            panel.Children.Add(r8);

            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var ret = panel.MakeVisible(r8, default(Rect));
            ret.Is(new Rect(300, 100, 100, 100));

            panel.HorizontalOffset.Is(200);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 子要素でない場合
        [Test, STAThread]
        public void MakeVisible_単独で使用された場合_Verticalの場合_子要素でない場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.Aquamarine) };
            
            var panel = new VirtualizingWrapPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Width = 200;
            panel.Height = 200;
            panel.Children.Add(r1);
            panel.Children.Add(r2);
            panel.Children.Add(r3);
            
            var window = new Window();
            window.Content = panel;

            window.Show();
            window.Close();

            var ret = panel.MakeVisible(r4, default(Rect));
            ret.Is(Rect.Empty);

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #endregion

        #endregion

        #region ItemsPanelの場合

        #region Horizontalの場合

        #region 上に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_上に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetVerticalOffset(50);

            var ret = panel.MakeVisible(r2, default(Rect));
            ret.Is(new Rect(100, 0, 100, 200));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 下に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_下に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetVerticalOffset(50);

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(new Rect(0, 200, 100, 200));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(200);
        }
        #endregion

        #region 左に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_左に切れている場合()
        {
            var r1 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 300, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 300, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetHorizontalOffset(200);

            var ret = panel.MakeVisible(r1, default(Rect));
            ret.Is(new Rect(0, 0, 200, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 右に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_右に切れている場合()
        {
            var r1 = new Rectangle() { Width = 300, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 300, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 300, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            
            var ret = panel.MakeVisible(r1, default(Rect));
            ret.Is(new Rect(0, 0, 200, 100));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 表示範囲内で切れていない場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_表示範囲内で切れていない場合()
        {
            var r1 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.Beige) };
            var r7 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.Bisque) };
            var r8 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.Black) };
            var r9 = new Rectangle() { Width = 60, Height = 60, Fill = new SolidColorBrush(Colors.BlanchedAlmond) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);
            itemsControl.Items.Add(r6);
            itemsControl.Items.Add(r7);
            itemsControl.Items.Add(r8);
            itemsControl.Items.Add(r9);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;

            var ret = panel.MakeVisible(r5, default(Rect));
            ret.Is(new Rect(60, 60, 60, 60));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 表示範囲にない場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_表示範囲にない場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetVerticalOffset(400);

            window.UpdateLayout();
            window.Close();

            var ret = panel.MakeVisible(r1, default(Rect));
            ret.Is(Rect.Empty);

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(400);
        }
        #endregion

        #region 子要素でない場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Horizontalの場合_子要素でない場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 150, Height = 400, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Horizontal);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            
            var ret = panel.MakeVisible(r4, default(Rect));
            ret.Is(Rect.Empty);

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #endregion

        #region Verticalの場合

        #region 上に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_上に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetHorizontalOffset(100);
            panel.SetVerticalOffset(100);

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(new Rect(200, 0, 100, 200));

            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 下に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_下に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetHorizontalOffset(100);

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(new Rect(200, 0, 100, 200));

            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 左に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_左に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetHorizontalOffset(50);

            var ret = panel.MakeVisible(r1, default(Rect));
            ret.Is(new Rect(0, 0, 100, 150));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 右に切れている場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_右に切れている場合()
        {
            var r1 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 100, Height = 150, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            panel.SetHorizontalOffset(50);

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(new Rect(200, 0, 100, 150));

            panel.HorizontalOffset.Is(100);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 表示範囲内で切れていない場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_表示範囲内で切れていない場合()
        {
            var r1 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.Aquamarine) };
            var r5 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.Azure) };
            var r6 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.Beige) };
            var r7 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.Bisque) };
            var r8 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.Black) };
            var r9 = new Rectangle() { Width = 80, Height = 60, Fill = new SolidColorBrush(Colors.BlanchedAlmond) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);
            itemsControl.Items.Add(r4);
            itemsControl.Items.Add(r5);
            itemsControl.Items.Add(r6);
            itemsControl.Items.Add(r7);
            itemsControl.Items.Add(r8);
            itemsControl.Items.Add(r9);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;

            var ret = panel.MakeVisible(r5, default(Rect));
            ret.Is(new Rect(80, 60, 80, 60));

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 表示範囲にない場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_表示範囲にない場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 150, Fill = new SolidColorBrush(Colors.Aqua) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;
            
            window.UpdateLayout();
            window.Close();

            var ret = panel.MakeVisible(r3, default(Rect));
            ret.Is(Rect.Empty);

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region 子要素でない場合
        [Test, STAThread]
        public void MakeVisible_ItemsPanelの場合_Verticalの場合_子要素でない場合()
        {
            var r1 = new Rectangle() { Width = 150, Height = 100, Fill = new SolidColorBrush(Colors.AliceBlue) };
            var r2 = new Rectangle() { Width = 150, Height = 200, Fill = new SolidColorBrush(Colors.AntiqueWhite) };
            var r3 = new Rectangle() { Width = 150, Height = 300, Fill = new SolidColorBrush(Colors.Aqua) };
            var r4 = new Rectangle() { Width = 150, Height = 400, Fill = new SolidColorBrush(Colors.Aquamarine) };

            var panelFactory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));
            panelFactory.SetValue(VirtualizingWrapPanel.OrientationProperty, Orientation.Vertical);

            var template = new ItemsPanelTemplate();
            template.VisualTree = panelFactory;

            var itemsControl = new ItemsControl();
            itemsControl.Width = 200;
            itemsControl.Height = 200;
            itemsControl.ItemsPanel = template;
            itemsControl.Items.Add(r1);
            itemsControl.Items.Add(r2);
            itemsControl.Items.Add(r3);

            var window = new Window();
            window.Content = itemsControl;

            window.Show();
            window.Close();

            var panel = FindItemsHostPanel(itemsControl) as VirtualizingWrapPanel;

            var ret = panel.MakeVisible(r4, default(Rect));
            ret.Is(Rect.Empty);

            panel.HorizontalOffset.Is(0);
            panel.VerticalOffset.Is(0);
        }
        #endregion

        #endregion

        #endregion

        #endregion

        #region SetHorizontalOffset

        #region 正常ケース
        [Test, STAThread]
        public void SetHorizontalOffset_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);

            panel.SetHorizontalOffset(50);

            panel.HorizontalOffset.Is(50);
        }
        #endregion

        #region マイナス値を指定した場合
        [Test, STAThread]
        public void SetHorizontalOffset_マイナス値を指定した場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);

            panel.SetHorizontalOffset(-50);

            panel.HorizontalOffset.Is(0);
        }
        #endregion

        #region ViewportがExtentより大きい場合
        [Test, STAThread]
        public void SetHorizontalOffset_ViewportがExtentより大きい場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(100, 100);
            panel.AsDynamic().viewport = new Size(200, 200);

            panel.SetHorizontalOffset(50);

            panel.HorizontalOffset.Is(0);
        }
        #endregion

        #region Extentの端より大きい値を指定した場合
        [Test, STAThread]
        public void SetHorizontalOffset_Extentの端より大きい値を指定した場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);

            panel.SetHorizontalOffset(150);

            panel.HorizontalOffset.Is(100);
        }
        #endregion

        #endregion

        #region SetVerticalOffset

        #region 正常ケース
        [Test, STAThread]
        public void SetVerticalOffset_正常ケース()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);

            panel.SetVerticalOffset(50);

            panel.VerticalOffset.Is(50);
        }
        #endregion

        #region マイナス値を指定した場合
        [Test, STAThread]
        public void SetVerticalOffset_マイナス値を指定した場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);

            panel.SetVerticalOffset(-50);

            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region ViewportがExtentより大きい場合
        [Test, STAThread]
        public void SetVerticalOffset_ViewportがExtentより大きい場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(100, 100);
            panel.AsDynamic().viewport = new Size(200, 200);

            panel.SetVerticalOffset(50);

            panel.VerticalOffset.Is(0);
        }
        #endregion

        #region Extentの端より大きい値を指定した場合
        [Test, STAThread]
        public void SetVerticalOffset_Extentの端より大きい値を指定した場合()
        {
            var panel = new VirtualizingWrapPanel();
            panel.AsDynamic().extent = new Size(200, 200);
            panel.AsDynamic().viewport = new Size(100, 100);

            panel.SetVerticalOffset(150);

            panel.VerticalOffset.Is(100);
        }
        #endregion

        #endregion

        #endregion
    }
}
