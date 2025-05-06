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

namespace AnimusTest.Views
{

    public partial class IllustratorWindow : Window
    {
        private SKBitmap bitmap;
        private SKCanvas drawingCanvas;
        private SKPaint paint;
        private bool isDrawing = false;
        private SKPoint previousPoint;
        private VectorProjectHistory projectHistory = new();

        private float scaleX;
        private float scaleY;


        public IllustratorWindow()
        {
            InitializeComponent();

            bitmap = new SKBitmap(1500, 820);
            drawingCanvas = new SKCanvas(bitmap);
            

            paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 1,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Stroke
            };

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
            if (bitmap.Width != e.Info.Width || bitmap.Height != e.Info.Height)
            {
                bitmap = new SKBitmap(e.Info.Width, e.Info.Height);
                drawingCanvas = new SKCanvas(bitmap);
            }

            e.Surface.Canvas.Clear(SKColors.White);
            e.Surface.Canvas.DrawBitmap(bitmap, 0, 0);
        }

        private void SkiaCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;
            previousPoint = GetScaledPoint(e.GetPosition(SkiaCanvas));


        }

        private void SkiaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            var currentPoint = GetScaledPoint(e.GetPosition(SkiaCanvas));
            drawingCanvas.DrawLine(previousPoint, currentPoint, paint);
            previousPoint = currentPoint;
            SkiaCanvas.InvalidateVisual();
        }

        private void SkiaCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
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






    }

    public static class ExtensionMethods
    {
        public static SKPoint ToSKPoint(this System.Windows.Point point)
        {
            return new SKPoint((float)point.X, (float)point.Y);
        }
    }


}
