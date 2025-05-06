using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using AnimusTest.Controls;
using AnimusTest.Machines;
using AnimusTest.Models;
using AnimusTest.Views;
using SkiaSharp;

namespace AnimusTest
{
    public partial class MainWindow : Window
    {
        private FileController fileController = new();

        private Stroke currentStroke; // для запису в дані шару
        private Polyline currentLine = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 2 }; // для малювання напряму на канвас

        private Tool currentTool = null;

        private bool isDrawing = false;
        private Timeline timeline = new Timeline();
        private Keyframe currentFrame;
        private Layer currentLayer;
        private Polygon currentFrameUIEl = null;
        private Polyline cursor = null;
        private VectorProjectHistory projectHistory = new();

        private List<Polygon> FrameFigurines { get; set; } = new List<Polygon>();

        public OnionSkin onionSkin = new();
        public bool showOnionSkin = true;

        private bool testFrames = true;
        // тут створити унікальний список шарів


        private Color chosenColor = Colors.Navy;

        public MainWindow() {
            //MessageBox.Show("Constructor called!");
            InitializeComponent();
            //MessageBox.Show($"Initial Size: {this.Width}x{this.Height}");

            timeline.AddTestData();
            //RenderFrame(0);

            // кілька тестових фреймів
            for (int i = 0; i < 60; i++)
            {
                timeline.Frames.Add(new Keyframe("Тернопіль"));
            }
            

            RenderTimeline();

            var st = new ScaleTransform();
            var textBox = new TextBox { Text = "Test" };
            DrawCanvas.RenderTransform = st;
            DrawCanvas.Children.Add(textBox);
            DrawCanvas.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0)
                {
                    st.ScaleX *= 2;
                    st.ScaleY *= 2;
                }
                else
                {
                    st.ScaleX /= 2;
                    st.ScaleY /= 2;
                }
            };

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
            // Для запобігання малювання в нікуди
            currentFrame ??= this.timeline.Frames[0];
            if (currentLayer == null)
            {
                currentLayer = new Layer();
                currentFrame.Layers.Add(currentLayer);
            }

            isDrawing = true;
            currentStroke = new Stroke {
                Color = Colors.Navy,
                Width = 2
            };

            currentLine = new Polyline () {
                Stroke = Brushes.Navy,
                StrokeThickness = 2
            };
            currentStroke.Points.Add(e.GetPosition(DrawCanvas));
            currentLine.Points.Add(e.GetPosition(DrawCanvas));

            //****
            //.Add(currentStroke); // тут має робитися запис штриха на канвас, і потім після закінчення штрих має записатися в поле strokes 
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            if (isDrawing) {
                currentStroke.Points.Add(e.GetPosition(DrawCanvas));

                DisplayFrame(currentFrame);

                currentLine.Points.Add(e.GetPosition(DrawCanvas));
                DrawCanvas.Children.Add(currentLine);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
            isDrawing = false;

            if (currentFrame == null) {
                MessageBox.Show("No current frame selected!");
                return;
            }

            /*
            if (currentLayer == null) {
                currentLayer = new Layer();
                currentFrame.Layers.Add(currentLayer);
            }
            */

            currentLayer.Strokes.Add(currentStroke);

            /***********************************************************************/
            var action = new DrawLineAction(currentStroke, currentLayer);
            projectHistory.AddToHistory(action);
            UndoButton.IsEnabled = true;
            /***********************************************************************/

            DisplayFrame(currentFrame);

            // ***** ЧОМУ Ж ТИ НЕ РЕНДЕРИШСЯ!??
        }


        static void Print_Test_Info () {
            Debug.WriteLine("");
        }

        private void RenderFrame(Keyframe frame) {
            
            DrawCanvas.Children.Clear();

            foreach (Layer layer in frame.Layers) {
                if (layer.Strokes.Count > 0) // TODO ДОДАТИ ПЕРЕВІРКУ НА РЕНДЕРИНГ ПУСТИХ ФРЕЙМІВ ШАРІВ - ЯКЩО КАДР/ШАР Є ПУСТИМ, ТО НА КАНВАСІ МАЄ ЗАЛИШАТИСЬ ОСТАННІЙ НЕПУСТИЙ КАДР
                {
                    foreach (Stroke stroke in layer.Strokes)
                    {

                        Polyline polyline = new Polyline
                        {

                            Stroke = new SolidColorBrush(stroke.Color),
                            StrokeThickness = stroke.Width
                        };

                        foreach (Point p in stroke.Points)
                        {

                            polyline.Points.Add(p);
                        }

                        DrawCanvas.Children.Add(polyline);
                    }
                }
            }
        }


        private void DisplayFrame(Keyframe frame)
        {
            DrawCanvas.Children.Clear();

            if (showOnionSkin)
            {
                if (onionSkin.showPrev)
                {
                    for (int i = 0; i < onionSkin.amountPrev; i++)
                    {

                        int prevIndex = timeline.Frames.IndexOf(frame) - 1;
                        if (prevIndex >= 0)
                        {
                            DrawOnionSkin(timeline.Frames[prevIndex], Colors.Red, 0.1);
                        }
                    }
                }

                if (onionSkin.showNext)
                {
                    for (int i = 0; i <= onionSkin.amountNext; i++)
                    {
                        int nextIndex = timeline.Frames.IndexOf(frame) + 1;
                        if (nextIndex < timeline.Frames.Count)
                        {
                            DrawOnionSkin(timeline.Frames[nextIndex], Colors.Green, 0.1);
                        }
                    }
                }
            }

            foreach (Layer layer in frame.Layers)
            {
                foreach (Stroke stroke in layer.Strokes)
                {
                    DrawStroke(stroke, Colors.Navy, 1.0);
                }
            }
        }

        private void DrawOnionSkin(Keyframe frame, Color color, double opacity)
        {
            foreach (Layer layer in frame.Layers)
            {
                foreach (Stroke stroke in layer.Strokes)
                {
                    DrawStroke(stroke, color, opacity);
                }
            }
        }

        private void DrawStroke(Stroke stroke, Color color, double opacity)
        {
            Polyline polyline = new Polyline
            {
                Stroke = new SolidColorBrush(Color.FromArgb((byte)(opacity * 255), color.R, color.G, color.B)),
                StrokeThickness = stroke.Width
            };

            foreach (Point p in stroke.Points)
            {
                polyline.Points.Add(p);
            }

            DrawCanvas.Children.Add(polyline);
        }



        private void Window_Loaded(object sender, RoutedEventArgs e) {

            RenderTimeline();

        }

        private void RenderTimeline()
        {
            FrameFigurines.Clear();
            TimelineCanvas.Children.Clear();
            int startOffset = 50;
            double frameWidth = 20;
            double timelineHeight = TimelineCanvas.Height;

            Polyline polyline = new Polyline
            {
                Points = new PointCollection { new Point(0, 25), new Point(TimelineCanvas.Width, 25) },
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };

            TimelineCanvas.Children.Add(polyline);

            polyline = new Polyline
            {
                Points = new PointCollection { new Point(startOffset, 0), new Point(startOffset, timelineHeight) },
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };

            TimelineCanvas.Children.Add(polyline);

            for (int i = 0; i < timeline.Frames.Count; i++)
            {
                Keyframe frame = timeline.Frames[i];

                if (frame.isKey)
                {
                    Polygon diamond = new Polygon
                    {
                        Points = new PointCollection
                {
                    new Point(0, 5),
                    new Point(5, 10),
                    new Point(0, 15),
                    new Point(-5, 10)
                },
                        Fill = Brushes.CadetBlue,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 1
                    };

                    Canvas.SetLeft(diamond, i * frameWidth + startOffset + frameWidth);
                    Canvas.SetTop(diamond, 50);

                    int frameIndex = i;
                    diamond.Tag = frame;
                    diamond.MouseDown += OnKeyframeClick;

                    TimelineCanvas.Children.Add(diamond);

                    FrameFigurines.Add(diamond);
                    
                }
            }
            for (int i = 0; i < ((TimelineCanvas.Width-startOffset) / 20); i++)
            {
                int computedX = i * 20;
                TextBlock textBlock = new TextBlock();
                textBlock.Foreground = Brushes.Gray;
                textBlock.Text = i.ToString();
                Canvas.SetLeft(textBlock, computedX - 5 + startOffset);
                Canvas.SetTop(textBlock, 3);

                polyline = new Polyline
                {
                    Points = new PointCollection { new Point(computedX + startOffset, 20), new Point(computedX + startOffset, 35) },
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };

                
                TimelineCanvas.Children.Add(polyline);
                TimelineCanvas.Children.Add(textBlock);
                
            }

            cursor ??= new Polyline
            {
                Points = new PointCollection { new Point(startOffset + frameWidth, 0), new Point(startOffset + frameWidth, TimelineCanvas.Height) },
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            TimelineCanvas.Children.Add(cursor);
        }


        private void OnKeyframeClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Polygon polygon && polygon.Tag is Keyframe frame)
            {
                if (currentFrameUIEl != null)
                {
                    currentFrameUIEl.Stroke = Brushes.Blue;
                }
                currentFrameUIEl = polygon;
                currentFrameUIEl.Stroke = Brushes.Yellow;
                currentFrame = frame;
                currentLayer = currentFrame.Layers[0];
                DisplayFrame(currentFrame);
                //MessageBox.Show($"Selected frame: {currentFrame.title}");
            }
        }
        private void LayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LayerList.SelectedIndex >= 0 && LayerList.SelectedIndex < currentFrame.Layers.Count)
            {

                currentLayer = currentFrame.Layers[LayerList.SelectedIndex];

                /*
                if (currentFrame.Layers.Count > 0)
                {
                    currentLayer = currentFrame.Layers[0]; // поки шо буде так
                }
                else
                {
                    currentLayer = new Layer();
                    currentFrame.Layers.Add(currentLayer);
                }
                */

            }
        }

        private void RefreshLayerListUI()
        {

            LayerList.Items.Clear();

            for (int i = 0; i < currentFrame.Layers.Count; i++)
            {

                ListBoxItem item = new ListBoxItem
                {
                    Content = $"Layer {i}",
                    Tag = currentFrame.Layers[i]
                };

                LayerList.Items.Add(item);
            }
        }

        private async void PlayAnimation(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < timeline.Frames.Count; i++)
            {
                //currentFrame = timeline.Frames[i];
                RenderFrame(timeline.Frames[i]);
                await Task.Delay(1000 / timeline.fps);
            }
            DisplayFrame(currentFrame);
        }


        private void StopAnimation(object sender, RoutedEventArgs e)
        {

        }

        private void PauseAnimation(object sender, RoutedEventArgs e) {
            
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow welcomeWindow = new WelcomeWindow();
            welcomeWindow.Show();
            this.Close();
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            fileController.SaveProjectToFile(timeline);
            MessageBox.Show("Successfully saved project!");
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            timeline = fileController.OpenFile();
            if (timeline == null)
            {
                MessageBox.Show("Smth went wrong. Could not read file.");
                return;
            }
            MessageBox.Show("Successfully opened project!");
            currentFrame = timeline.Frames[0];
            RenderTimeline();
        }

        private void TimelineCursor_MouseDown(object sender, MouseEventArgs e)
        {
            double mouseX = e.GetPosition(TimelineCanvas).X;
            MoveTimelineCursor(mouseX);
        }

        private void MoveTimelineCursor(double mouseX)
        {
            int startOffset = 70;

            int snappedFrame = (int)Math.Round((mouseX - startOffset) / 20.0);
            int newPosition = snappedFrame * 20;

            Canvas.SetLeft(cursor, newPosition);
            
            currentFrame = timeline.Frames[snappedFrame];
            currentLayer = currentFrame.Layers[0];

            DisplayFrame(currentFrame);

            SelectFrameOnUI();
        }

        private void SelectFrameOnUI()
        {
            if (currentFrameUIEl != null)
            {
                currentFrameUIEl.Stroke = Brushes.Blue;
            }

            for (int i = 0; i < FrameFigurines.Count; i++)
            {
                if (FrameFigurines[i].Tag == currentFrame)
                {
                    currentFrameUIEl = FrameFigurines[i];
                    currentFrameUIEl.Stroke = Brushes.Yellow;
                }
            }
        }

        private void TimelineCursor_MouseUp(object sender, MouseEventArgs e)
        {
            double mouseX = e.GetPosition(TimelineCanvas).X;
            MoveTimelineCursor(mouseX);
        }

        private void TimelineCursor_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void TimelineCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (projectHistory.CanUndo())
            {
                projectHistory.Undo();
                DisplayFrame(currentFrame);
                RedoButton.IsEnabled = true;
            }
            else
            {
                UndoButton.IsEnabled = false;
                MessageBox.Show("Nothing to undo.");
            }
            if (projectHistory.CanUndo())
            {
                UndoButton.IsEnabled = true;
            }
            else
            {
                UndoButton.IsEnabled = false;
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            
            if (projectHistory.CanRedo())
            {
                projectHistory.Redo();
                DisplayFrame(currentFrame);
                UndoButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Nothing to redo.");
                RedoButton.IsEnabled = false;
            }
            if (projectHistory.CanRedo())
            {
                RedoButton.IsEnabled = true;
            }
            else
            {
                RedoButton.IsEnabled = false;
            }

        }


    }
}

