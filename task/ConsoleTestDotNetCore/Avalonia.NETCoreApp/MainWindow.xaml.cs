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

namespace Avalonia.NETCoreApp
{
    public class MainWindow : Window
    {
        private static MainWindow _window = new MainWindow();
        public MainWindow()
        {
            InitializeComponent();
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
            //Getting Controls references
            
            //Setting the value
            var imageControl = this.FindControl<Image>("Field");
            var canvas = this.FindControl<Canvas>("Canvas");
            imageControl.Source = new Bitmap("//home/amir-kamolov/photo/cat.jpg");
            Thread.Sleep(500);
            bool[,] dots = {{true,false},{false,true}};
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