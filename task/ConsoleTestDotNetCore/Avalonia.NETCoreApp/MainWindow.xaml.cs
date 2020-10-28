using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

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
            
            //Setting the valu
            var imageControl = this.FindControl<Image>("Cat");
            imageControl.Source = new Bitmap("//home/amir-kamolov/photo/cat.jpg");
            
            Console.WriteLine(imageControl.Source.Size);
            Console.WriteLine(imageControl.Source.Dpi);
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