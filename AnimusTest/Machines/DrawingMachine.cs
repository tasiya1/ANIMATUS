using AnimusTest.Models;
using SkiaSharp;

namespace AnimusTest.Machines
{
    public class DrawingMachine
    {
        public bool isErasing { get; set; } = false;
        public bool isDrawing { get; set; } = false;
        public Models.Brush currentBrush;
        public SKPaint Eraser;
        public List<Models.Brush> Brushes = new List<Models.Brush>();
        private SKBitmap bitmap;
        private SKCanvas drawingCanvas;

        private SKPoint previousPoint;

        public SKCanvas activeCanvas { get; set; } = null;
        //public int activeFrameIndex = 0;

        private readonly Project project;
        private Action requestRedraw;

        //public int ActiveLayerIndex = 0;

        public DrawingMachine(Project project, Action requestRedraw)
        {


            Brushes.Add(new Models.Pencil());
            Brushes.Add(new Models.Marker());
            Brushes.Add(new Models.Charcoal());
            Brushes.Add(new Models.Acryl());
            Brushes.Add(new Models.Sponge());
            Brushes.Add(new Models.Airbrush());
            currentBrush = Brushes[0];
            Eraser = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                StrokeWidth = 20,
                Style = SKPaintStyle.Stroke,
                BlendMode = SKBlendMode.Clear,
                StrokeCap = SKStrokeCap.Round
            };
            


            // В МАЙБУТНЬОМУ (було б добре) Створити загальний клас Tool, від якого успадковуються Figure, Brush etc., а від них - відповідно підвиди інструментів(Figure: Rectangle, Line, Ellipse, Brush: Pen, Oil, Chalk...)
            this.project = project;
            this.requestRedraw = requestRedraw;
            //DrawTest();
        }


        public void DrawTest()
        {
            SetActiveLayer(1);
            //StartDrawing(new SKPoint(0, 0));
            //ContinueDrawing(new SKPoint(300, 500));
            //EndDrawing();

            activeCanvas = new SKCanvas(project.CurrentLayer.Bitmap);
            activeCanvas.DrawCircle(1490, 990, 5, new SKPaint { Color = SKColors.Blue });
            activeCanvas?.Dispose();
            activeCanvas = null;

            SetActiveLayer(0);
        }

        public void SetActiveLayer(int index)
        {
            if (index < project.CurrentFrame.Layers.Count)
                project.CurrentLayerIndex = index;
        }


        public void ToggleEraser()
        {
            if (isErasing) {
                //currentBrush.eraser.BlendMode = SKBlendMode.SrcOver;
                isErasing = false;
            } else
            {
                //currentBrush.eraser.BlendMode = SKBlendMode.Clear;
                isErasing = true;
            }
        }

        public void StartDrawing(SKPoint point)
        {
            isDrawing = true;
            previousPoint = point;

            var layer = project.CurrentLayer;
            activeCanvas = new SKCanvas(layer.Bitmap);
        }

        public void ContinueDrawing(SKPoint point)
        {
            if (!isDrawing || activeCanvas == null) return;

            if (!isErasing)
                currentBrush.Draw(previousPoint, point, activeCanvas);
            else activeCanvas.DrawLine(previousPoint, point, Eraser);
                previousPoint = point;
            this.requestRedraw?.Invoke();
        }

        public void EndDrawing() {
            project.CurrentFrame.isDirty = true;
            isDrawing = false;
            activeCanvas?.Dispose();
            activeCanvas = null;
            this.requestRedraw?.Invoke();
        }


        public void setBrush(int index, System.Windows.Media.Color? selectedColor, float brushSizeBinded)
        {
           currentBrush = Brushes[index]; 
            currentBrush.TintBrush(
                selectedColor.HasValue
                ? new SKColor(selectedColor.Value.R, selectedColor.Value.G, selectedColor.Value.B, selectedColor.Value.A)
                : SKColors.Black
            );
            currentBrush.brushSize = brushSizeBinded;
        }

    }

}
