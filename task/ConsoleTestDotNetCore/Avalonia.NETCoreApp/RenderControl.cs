using System;
using System.Drawing.Printing;
using Automaton.core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Direct2D1.Media;
using Avalonia.Media;
using Avalonia.NETCoreApp;
using Avalonia.Rendering;

namespace AvaloniaUI
{
    public class RenderControl : Canvas
    {
        public double resolution;
        private Frontend frontend;
        private int rows;
        private int cols;
        private Cell[][] data;
        private int i = 0;
        public RenderControl(Rect bounds, double resolution) : this()
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
                base.Render(context);
                this.Background = Brushes.Aquamarine;
                this.Height = this.Parent.DesiredSize.Height; 
                this.Width = this.Parent.DesiredSize.Width;
                this.resolution = 50;
                cols = (int) (Height / resolution);
                rows = (int) (Width / resolution);
                frontend = new Frontend(cols, rows);
                frontend.Generate();
                data = frontend.GetNextGeneration();
                DrawCells(Brushes.Red, context);
                DrawGrid(Brushes.White, context);
                Console.WriteLine(i++);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DrawPixel(IBrush brush,Point point, DrawingContext context)
        {
            var pixel = new Rect(new Point(point.X * resolution, point.Y * resolution), new Size(1 * resolution, 1 * resolution));
            context.FillRectangle(brush, pixel);
        }

        public void DrawGrid(IBrush brush, DrawingContext context)
        {
            Console.WriteLine("DRAWING GRID");
            for (var i = 0; i < Width/resolution; i++)
            {
                for (var j = 0; j < (Height / resolution); j++)
                {
                    context.DrawLine(new Pen(brush), new Point(i*resolution, 0), new Point(i*resolution,Height));
                    context.DrawLine(new Pen(brush), new Point(0, j*resolution), new Point(Width,j*resolution));
                }
            }
        }

        public void DrawCells(IBrush brush, DrawingContext context)
        {
            Console.WriteLine("DRAWING CELLS");
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