using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Direct2D1.Media;
using Avalonia.Direct2D1.Media.Imaging;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering;
using AvaloniaUI;

namespace Avalonia.NETCoreApp
{
    public class MainWindow : Window
    {
        private static MainWindow _window = new MainWindow();
        private StackPanel stackPanel;
        private Frontend frontend;
        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif
        }

        public MainWindow GetInstance()
        {
            return _window;
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            stackPanel = this.FindControl<StackPanel>("RenderView");
            stackPanel.PointerPressed += SetCellPointerPressed;
            
        }
        
        public void GreetButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(@"CLICK!" + sender + e);
        }

        public void StartBtn_Click(object sender, RoutedEventArgs s)
        {
            Frontend.GetInstance().Start();
        }

        public void StopBtn_Click(object sender, RoutedEventArgs s)
        {
            Frontend.GetInstance().Stop();
        }

        public void GenerateBtn_Click(object sender, RoutedEventArgs s)
        {
            Frontend.GetInstance().Generate();
        }

        public void SetCellPointerPressed(object sender, RoutedEventArgs e)
        {
            var le = e as Avalonia.Input.PointerEventArgs;
            var position = le.GetPosition(stackPanel);
            int x = (int) (position.X / 200);
            int y = (int) (position.Y / 200);
            Frontend.GetInstance().SetCell(x, y);
        }
        
    }
    
}