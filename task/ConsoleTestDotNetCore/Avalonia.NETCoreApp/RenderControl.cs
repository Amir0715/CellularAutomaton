using System;
using System.Threading;
using System.Threading.Tasks;
using Automaton.core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Direct2D1.Media;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.NETCoreApp;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;

namespace AvaloniaUI
{
    public class RenderControl : Control
    {   
        
        private int cols;
        private int rows;
        private int resolution;

        public RenderControl(Rect bounds) : this()
        {
            this.Bounds = bounds;
        }

        public RenderControl() : base()
        {
            resolution = 200;
            cols = (int) (1664/resolution); 
            rows = (int) (1016/resolution);
            frontend = Frontend.GetInstance(cols: cols, rows: rows);
            rendrenOp = new RendrenOp(new Rect(0, 0, 1664,1016),frontend,cols,rows, resolution);
        }
        
        public override void Render(DrawingContext context)
        {
            try
            {
                
                context.Custom(rendrenOp);
                
                // При передаче котекста в другие методы не рисуется, мне кажется 
                //DrawCells(Brushes.Red, context);
                //DrawGrid(Brushes.Black, ref context);
                
                // не совсем понятно чем отличается IvokeAsync и Post, мне кажется порядком вызова
                Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
                //Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
                base.Render(context);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        


        public class RendrenOp : ICustomDrawOperation
        {

            private double resolution;
            private Frontend frontend;
            private int rows;
            private int cols;
            private Cell[][] data;
            private double height;
            private double width;


            private readonly IBrush backgroundColor = Brushes.White;
            private readonly IBrush gridColor = Brushes.Black;
            private readonly IBrush cellColor = Brushes.Red;
            
            public Rect Bounds { get; }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => false;
            public RendrenOp(Rect bounds, Frontend frontend ,int cols, int rows,double resolution)
            {
                Bounds = bounds;
                this.resolution = resolution;
                height = Bounds.Height;
                width = Bounds.Width;
                this.cols = cols;
                this.rows = rows;
                this.frontend = frontend;
            }
            
            public void Render(IDrawingContextImpl context)
            {
                try
                {
                    context.FillRectangle(backgroundColor, Bounds);
                    
                    data = frontend.GetNextGeneration();
                    
                    DrawCells(cellColor, context);
                    DrawGrid(gridColor, context);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public void Dispose()
            {
            }
            
            private void DrawPixel(IBrush brush,Point point, IDrawingContextImpl context)
            {
                var pixel = new Rect(new Point(point.X * resolution, point.Y * resolution), new Size( resolution, resolution));
                context.FillRectangle(brush, pixel);
            }

            private void DrawGrid(IBrush brush, IDrawingContextImpl context)
            {
                for (var i = 0; i <= cols; i++)
                {
                    for (var j = 0; j <= rows; j++)
                    {
                        context.DrawLine(new ImmutablePen(brush), new Point(i*resolution, 0), new Point(i*resolution,height));
                        context.DrawLine(new ImmutablePen(brush), new Point(0, j*resolution), new Point(width,j*resolution));
                    }
                }
            }

            private void DrawCells(IBrush brush, IDrawingContextImpl context)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    for (var j = 0; j < data[0].Length; j++)
                    {
                        if (data[i][j].IsAlive)
                        {
                            DrawPixel(brush,new Point(i,j), context);
                        }
                    }
                }
            }
        }

        private RendrenOp rendrenOp;
        private Frontend frontend;
    }
}