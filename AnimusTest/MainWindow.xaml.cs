using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnimusTest.Models;

namespace AnimusTest
{
    public partial class MainWindow : Window
    {

        private Stroke currentStroke; // для запису в дані шару
        private Polyline currentLine = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 2 }; // для малювання напряму на канвас

        private bool isDrawing = false;
        private Timeline timeline = new Timeline();
        private Keyframe currentFrame;
        private Layer currentLayer;
        // тут створити унікальний список шарів


        private Color chosenColor = Colors.Navy;

        public MainWindow() {
            //MessageBox.Show("Constructor called!");
            InitializeComponent();
            //MessageBox.Show($"Initial Size: {this.Width}x{this.Height}");

            timeline.AddTestData(); // Додаємо тестові кадри
            //RenderFrame(0);
            timeline.Frames.Add(new Keyframe("TinkiWinki"));
            timeline.Frames.Add(new Keyframe("Dipsi"));
            timeline.Frames.Add(new Keyframe("LalaPo"));
            RefreshTimelineUI();


            timeline.DisplayTimeline(TimelineList);

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
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

                RenderFrame(currentFrame);

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

            if (currentLayer == null) {
                currentLayer = new Layer();
                currentFrame.Layers.Add(currentLayer);
            }

            currentLayer.Strokes.Add(currentStroke);
            RenderFrame(currentFrame);

            // ***** ЧОМУ Ж ТИ НЕ РЕНДЕРИШСЯ!??
        }


        static void Print_Test_Info () {
            Debug.WriteLine("");
        }

        private void RenderFrame(Keyframe frame) {
            
            DrawCanvas.Children.Clear();

            foreach (Layer layer in frame.Layers) {
                
                foreach (Stroke stroke in layer.Strokes) {

                    Polyline polyline = new Polyline {

                        Stroke = new SolidColorBrush(stroke.Color),
                        StrokeThickness = stroke.Width
                    };

                    foreach (Point p in stroke.Points) {

                        polyline.Points.Add(p);
                    }

                    DrawCanvas.Children.Add(polyline);
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) {

            for (int i = 0; i < timeline.Frames.Count; i++) {

                TimelineList.Items.Add($"Frame {i}: {timeline.Frames[i].title}");
            }
            TimelineList.SelectedIndex = 0; 
            //TimelineList.SelectedItems = timeline.Frames;

            RefreshLayerListUI();

        }

        private void TimelineList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
            if (TimelineList.SelectedIndex >= 0 && TimelineList.SelectedIndex < timeline.Frames.Count) {

                currentFrame = timeline.Frames[TimelineList.SelectedIndex];

                if (currentFrame.Layers.Count > 0) {
                    currentLayer = currentFrame.Layers[0]; // поки шо буде так
                } else {
                    currentLayer = new Layer();
                    currentFrame.Layers.Add(currentLayer);
                }

                RenderFrame(currentFrame);
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

        private void RefreshTimelineUI() {

            TimelineList.Items.Clear();

            for (int i = 0; i < timeline.Frames.Count; i++) {
                
                ListBoxItem item = new ListBoxItem {
                    Content = $"Frame {i}: {timeline.Frames[i].title}",
                    Tag = timeline.Frames[i]
                };

                TimelineList.Items.Add(item);
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
        }


        private void StopAnimation(object sender, RoutedEventArgs e)
        {

        }

        private void PauseAnimation(object sender, RoutedEventArgs e) {
            
        }

        private void DisplayOnionSkin()
        {

        }


    }
}

