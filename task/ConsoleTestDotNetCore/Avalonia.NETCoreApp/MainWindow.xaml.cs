using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Avalonia.NETCoreApp
{
    public class MainWindow : Window
    {
        private static MainWindow _window = new MainWindow();
        private StackPanel StackPanel;
        private RenderControl RenderControl;
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

            RenderControl = this.FindControl<RenderControl>("RenderControl");
            
            StackPanel = this.FindControl<StackPanel>("RenderView");
            StackPanel.PointerPressed += SetCellPointerPressed;
            
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
        
        public void NextStepBtn_Click(object sender, RoutedEventArgs s)
        {
            Frontend.GetInstance().NextStep();
        }

        public void SetCellPointerPressed(object sender, RoutedEventArgs e)
        {
            var le = e as Avalonia.Input.PointerEventArgs;
            var position = le.GetPosition(StackPanel);
            int x = (int) (position.X / RenderControl.Resolution);
            int y = (int) (position.Y / RenderControl.Resolution);
            Frontend.GetInstance().SetCell(x, y);
        }
        
    }
    
}