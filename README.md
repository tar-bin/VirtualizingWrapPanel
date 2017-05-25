# VirtualizingWrapPanel
VirtualizingWrapPanel CodePlex Mirror
https://uhimaniavwp.codeplex.com/

```
<Window x:Class="WpfApplication1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vwp="clr-namespace:CodePlex.VirtualizingWrapPanel;assembly=VirtualizingWrapPanel"
        Title="MainWindow" Height="350" Width="525">
    <Grid>
        <ListBox>
            <ListBox.ItemsPanel>
                <ItemsPanelTemplate>
                    <vwp:VirtualizingWrapPanel />
                </ItemsPanelTemplate>
            </ListBox.ItemsPanel>
        </ListBox>
    </Grid>
</Window>
```

Microsoft Public License (Ms-PL)
