using Avalonia.Media.Imaging;
using Avalonia.Controls;
using Image = Avalonia.Controls.Image;

namespace Avalonia.NETCoreApp
{
    public class Frontend
    {
        private Automaton.core.Automaton _automaton;
        private Bitmap _bitmap { get ; set; }
        private Image _image { get; set; }
        private MainWindow _mainWindow;

        Frontend()
        {
            _automaton = new Automaton.core.Automaton(200,300);
            _image = _mainWindow.GetInstance().FindControl<Image>("Cat");
            _image.Source = _bitmap;
        }
        
        
    }
}