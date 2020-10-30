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
        }
        
        public void GreetButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(@"CLICK!");
            
            Console.WriteLine(this.DesiredSize);
            var rd = this.FindControl<RenderControl>("RenderControl");
            //rd.Render();
            var c = new Canvas();
        }

        public void StartBtn_Click(object sender, RoutedEventArgs s)
        {
            
        }

        public void StopBtn_Click(object sender, RoutedEventArgs s)
        {
            
        }

        public void GenerateBtn_Click(object sender, RoutedEventArgs s)
        {
            
        }
        
    }
    
}