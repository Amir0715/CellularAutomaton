using System;
using System.Threading;
using System.Threading.Tasks;
using Automaton.core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Direct2D1.Media;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.NETCoreApp;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;

namespace AvaloniaUI
{
    public class RenderControl : Control
    {
        
        private int i = 0;

        public RenderControl(Rect bounds) : this()
        {
            this.Bounds = bounds;
        }

        public RenderControl() : base()
        {
        }
        
        public override void Render(DrawingContext context)
        {
            try
            {
                
                context.Custom(new RendrenOp(new Rect(0,0,1664,1016),10));
                
                // При передаче котекста в другие методы не рисуется, мне кажется 
                //DrawCells(Brushes.Red, context);
                //DrawGrid(Brushes.Black, ref context);
                
                // не совсем понятно чем отличается IvokeAsync и Post, мне кажется порядком вызова
                //Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
                Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
                base.Render(context);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        


        class RendrenOp : ICustomDrawOperation
        {

            private double resolution;
            private Frontend frontend;
            private int rows;
            private int cols;
            private Cell[][] data;
            private double Height;
            private double Width;
            public Rect Bounds { get; }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => false;
            public RendrenOp(Rect bounds, double resolution)
            {
                Bounds = bounds;
                this.resolution = resolution;
                Height = Bounds.Height;
                Width = Bounds.Width;
            }
            
            public void Render(IDrawingContextImpl context)
            {
                context.FillRectangle(Brushes.Gold, Bounds);
                cols = (int) (Height / resolution); 
                rows = (int) (Width / resolution);
                DrawPixel(Brushes.Red, new Point(10,10),context);
                
                //context.DrawLine(new Pen(Brushes.Blue), new Point(100, 100), new Point(100,500));
                
                frontend = new Frontend(cols, rows);
                frontend.Generate();
                data = frontend.GetNextGeneration();
                DrawCells(Brushes.Red, context);
                
            }

            public void Dispose()
            {
                
            }
            
            private void DrawPixel(IBrush brush,Point point, IDrawingContextImpl context)
            {
                var pixel = new Rect(new Point(point.X * resolution, point.Y * resolution), new Size(1 * resolution, 1 * resolution));
                context.FillRectangle(brush, pixel);
            }

            private void DrawGrid(IBrush brush, IDrawingContextImpl context)
            {
                
                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < cols; j++)
                    {
                        context.DrawLine(new Pen(brush), new Point(i*resolution, 0), new Point(i*resolution,Height));
                        context.DrawLine(new Pen(brush), new Point(0, j*resolution), new Point(Width,j*resolution));
                    }
                }
            }

            private void DrawCells(IBrush brush, IDrawingContextImpl context)
            {
                Console.WriteLine(@"DRAWING CELLS");
                Console.WriteLine(data.Length);
                Console.WriteLine(data[0].Length);
                for (var i = 0; i < data.Length; i++)
                {
                    for (var j = 0; j < data[0].Length; j++)
                    {
                        if (data[i][j].IsAlive)
                        {
                            DrawPixel(brush,new Point(j,i), context);
                        }
                    }
                }
            }
        }
        
        
    }
}