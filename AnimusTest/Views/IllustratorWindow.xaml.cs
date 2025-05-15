using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Windows.Controls;
using AnimusTest.Machines;
using AnimusTest.Models;

namespace AnimusTest.Views
{

    public partial class IllustratorWindow : Window
    {
        
        private SKBitmap bitmap;
        private SKCanvas drawingCanvas;
        //private bool isDrawing = false;
        private SKPoint previousPoint;
        private RasterProjectHistory projectHistory = new();

        private DrawingMachine DM;
        

        private float scaleX;
        private float scaleY;


        public IllustratorWindow()
        {
            InitializeComponent();

            SkiaCanvas.PaintSurface += SkiaCanvas_PaintSurface;

            bitmap = new SKBitmap(1500, 820);
            drawingCanvas = new SKCanvas(bitmap);


            DM = new DrawingMachine(bitmap, drawingCanvas);

            /*
            paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 1,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Stroke
            };
            */
            Loaded += (s, e) => UpdateScale();
            SkiaCanvas.SizeChanged += (s, e) => UpdateScale(); // якщо розмір змінюється
            var st = new ScaleTransform();
            SkiaCanvas.RenderTransform = st;
            SkiaCanvas.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0)
                {
                    st.ScaleX *= 1.5;
                    st.ScaleY *= 1.5;
                }
                else
                {
                    st.ScaleX /= 1.5;
                    st.ScaleY /= 1.5;
                }
            };
            

        }

        private void SkiaCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            foreach (var layer in DM.Layers)
            {
                if (!layer.IsVisible) continue;
                canvas.DrawBitmap(layer.Bitmap, SKPoint.Empty);
            }
        }

        private void SkiaCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //DM.isDrawing = true;
            previousPoint = GetScaledPoint(e.GetPosition(SkiaCanvas));

            DM.StartDrawing(previousPoint);
        }

        private void SkiaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!DM.isDrawing) return;

            var currentPoint = GetScaledPoint(e.GetPosition(SkiaCanvas));
            DM.ContinueDrawing(currentPoint);
            SkiaCanvas.InvalidateVisual();
        }

        private void SkiaCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DM.EndDrawing();
            SkiaCanvas.InvalidateVisual();
        }

        private SKPoint GetScaledPoint(Point point)
        {
            return new SKPoint(
                (float)point.X * scaleX,
                (float)point.Y * scaleY
            );
        }


        private void UpdateScale()
        {
            if (SkiaCanvas.ActualWidth == 0 || SkiaCanvas.ActualHeight == 0) return;

            scaleX = (float)bitmap.Width / (float)SkiaCanvas.ActualWidth;
            scaleY = (float)bitmap.Height / (float)SkiaCanvas.ActualHeight;
        }


        public void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            DM.ToggleEraser();
        }



    }



    public static class ExtensionMethods
    {
        public static SKPoint ToSKPoint(this System.Windows.Point point)
        {
            return new SKPoint((float)point.X, (float)point.Y);
        }
    }


}
