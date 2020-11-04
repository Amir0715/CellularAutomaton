using System;
using Automaton.core;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;

namespace Avalonia.NETCoreApp
{
    public class RenderControl : Control
    {   
        
        private int Cols;
        private int Rows;
        private int Resolution;
        private RenderOperation _RenderOperation;
        private Frontend _Frontend;
        public RenderControl(Rect bounds) : this()
        {
            this.Bounds = bounds;
        }

        public RenderControl() : base()
        {
            Resolution = 300;
            Cols = (int) (1664/Resolution); 
            Rows = (int) (1016/Resolution);
            _Frontend = Frontend.GetInstance(cols: Cols, rows: Rows);
            _RenderOperation = new RenderOperation(new Rect(0, 0, 1664,1016),_Frontend,Cols,Rows, Resolution);
        }
        
        public override void Render(DrawingContext context)
        {
            try
            {
                this.Bounds = new Rect(new Point(0,0),new Size(1664, 1016));
                
                context.Custom(_RenderOperation);
                
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
        
        


        public class RenderOperation : ICustomDrawOperation
        {

            private double Resolution;
            private Frontend Frontend;
            private int Rows;
            private int Cols;
            private Cell[][] Data;
            private double Height;
            private double Width;


            private readonly IBrush BackgroundColor = Brushes.White;
            private readonly IBrush GridColor = Brushes.Black;
            private readonly IBrush CellColor = Brushes.Red;
            
            public Rect Bounds { get; }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => false;
            public RenderOperation(Rect bounds, Frontend frontend ,int cols, int rows,double resolution)
            {
                Bounds = bounds;
                this.Resolution = resolution;
                Height = Bounds.Height;
                Width = Bounds.Width;
                this.Cols = cols;
                this.Rows = rows;
                this.Frontend = frontend;
            }
            
            public void Render(IDrawingContextImpl context)
            {
                try
                {
                    context.FillRectangle(BackgroundColor, Bounds);
                    
                    Data = Frontend.GetNextGeneration();
                    
                    DrawCells(CellColor, context);
                    DrawGrid(GridColor, context);
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
                var pixel = new Rect(new Point(point.X * Resolution, point.Y * Resolution), new Size( Resolution, Resolution));
                context.FillRectangle(brush, pixel);
            }

            private void DrawGrid(IBrush brush, IDrawingContextImpl context)
            {
                for (var i = 0; i <= Cols; i++)
                {
                    for (var j = 0; j <= Rows; j++)
                    {
                        context.DrawLine(new ImmutablePen(brush), new Point(i*Resolution, 0), new Point(i*Resolution,Height));
                        context.DrawLine(new ImmutablePen(brush), new Point(0, j*Resolution), new Point(Width,j*Resolution));
                    }
                }
            }

            private void DrawCells(IBrush brush, IDrawingContextImpl context)
            {
                for (var i = 0; i < Data.Length; i++)
                {
                    for (var j = 0; j < Data[0].Length; j++)
                    {
                        if (Data[i][j].IsAlive)
                        {
                            DrawPixel(brush,new Point(i,j), context);
                        }
                    }
                }
            }
        }
        
        
        
    }
}