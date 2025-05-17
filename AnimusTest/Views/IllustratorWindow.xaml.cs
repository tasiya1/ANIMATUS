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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimusTest.Controls;

namespace AnimusTest.Views
{

    public partial class IllustratorWindow : Window
    {
        
        //private bool isDrawing = false;
        private SKPoint previousPoint;
        private RasterProjectHistory projectHistory = new();


        private float scaleX;
        private float scaleY;
        private float scaleSpeed = 1.1f;

        private DrawingMachine DM;
        private Project project;
        private AnimationMachine animator;
        private RenderMachine renderer;

        public IllustratorWindow()
        {
            InitializeComponent();

            SkiaCanvas.PaintSurface += SkiaCanvas_PaintSurface; 

            project = new Project(1500, 820, 3);
            DM = new DrawingMachine(project);
            renderer = new RenderMachine(project);
            animator = new AnimationMachine(project, TimelineCanvas, () => SkiaCanvas.InvalidateVisual(), () => UpdateScale());


            //додати тестові фрейми
            for (int i = 0; i < 60; i++)
            {
                project.Frames.Add(new Models.Frame(1500, 820, 3));
            }


            Loaded += (s, e) => UpdateScale();
            SkiaCanvas.SizeChanged += (s, e) => UpdateScale(); // якщо розмір змінюється
            var st = new ScaleTransform();
            SkiaCanvas.RenderTransform = st;
            SkiaCanvas.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0)
                {
                    st.ScaleX *= scaleSpeed;
                    st.ScaleY *= scaleSpeed;
                }
                else
                {
                    st.ScaleX /= scaleSpeed;
                    st.ScaleY /= scaleSpeed;
                }
            };

            RefreshLayerListUI();
            animator.RenderTimelineUI();

        }

        private void SkiaCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            /*
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            foreach (var layer in DM.Layers)
            {
                if (!layer.IsVisible) continue;
                canvas.DrawBitmap(layer.Bitmap, SKPoint.Empty);
            }
            */


            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            var frame = project.CurrentFrame;

            foreach (var layer in frame.Layers)
            {
                if (!layer.IsVisible)
                    continue;

                var bmp = layer.Bitmap;

                using var paint = new SKPaint
                {
                    Color = SKColors.White.WithAlpha((byte)(layer.Opacity * 255)),
                    IsAntialias = true
                };

                canvas.DrawBitmap(bmp, SKPoint.Empty, paint);
            }
        }

        private void DisplayFrame(Models.Frame frame)
        {

            

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


        public void UpdateScale()
        {

            if (SkiaCanvas.ActualWidth == 0 || SkiaCanvas.ActualHeight == 0) return;

            scaleX = (float)project.CurrentLayer.Bitmap.Width / (float)SkiaCanvas.ActualWidth;
            scaleY = (float)project.CurrentLayer.Bitmap.Height / (float)SkiaCanvas.ActualHeight;
        }


        public void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            DM.ToggleEraser();
        }


        public void ToggleTimelineButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the timeline
            /*
            if (TimelineGrid.Visibility == Visibility.Visible)
            {
                TimelineGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                TimelineGrid.Visibility = Visibility.Visible;
            }
            */
        }

        public void ToggleStoryboardButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the visibility of the storyboard
            /*
            if (StoryboardGrid.Visibility == Visibility.Visible)
            {
                StoryboardGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                StoryboardGrid.Visibility = Visibility.Visible;
            }
            */
        }

        private void LayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
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
                

            }
            */
        }

        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Add a new layer to the project
            var newLayer = new RasterLayer(bitmap.Width, bitmap.Height);
            DM.Layers.Add(newLayer);
            // Update the UI to reflect the new layer
            // LayerList.Items.Add(newLayer.Name); // or whatever you use to display layers
            */
        }

        private void RemoveLayerButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Remove the selected layer from the project
            if (DM.Layers.Count > 0)
            {
                DM.Layers.RemoveAt(DM.Layers.Count - 1); // remove the last layer for example
                // Update the UI to reflect the removed layer
                // LayerList.Items.RemoveAt(LayerList.Items.Count - 1);
            }
            */
        }

        private void OnKeyframeUI_Click(object sender, MouseButtonEventArgs e)
        {
            /*
            if (sender is Polygon polygon && polygon.Tag is Models.Frame frame)
            {
                if (currentFrameUIEl != null)
                {
                    currentFrameUIEl.Stroke = diamondBorder;
                }
                currentFrameUIEl = polygon;
                currentFrameUIEl.Stroke = Brushes.Yellow;
                project.Frames.Find(frame);
                project. = ;
                currentLayer = currentFrame.Layers[0];
                DisplayFrame(currentFrame);
                //MessageBox.Show($"Selected frame: {currentFrame.title}");
            }
            */
            
        }





        private async void PlayAnimation_Button(object sender, RoutedEventArgs e)
        {
            animator.PlayAnimation();
        }

        public void updateToolColor()
        {

        }

        private void StopAnimation_Button(object sender, RoutedEventArgs e)
        {

        }

        private void PauseAnimation_Button(object sender, RoutedEventArgs e)
        {

        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow welcomeWindow = new WelcomeWindow();
            welcomeWindow.Show();
            this.Close();
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            /*
            fileController.SaveProjectToFile(timeline);
            MessageBox.Show("Successfully saved project!");
            */
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            /*
            timeline = fileController.OpenFile();
            if (timeline == null)
            {
                MessageBox.Show("Smth went wrong. Could not read file.");
                return;
            }
            MessageBox.Show("Successfully opened project!");
            currentFrame = timeline.Frames[0];
            RenderTimelineUI();
            */
        }

        private void TimelineCursor_MouseDown(object sender, MouseEventArgs e)
        {
            double mouseX = e.GetPosition(TimelineCanvas).X;
            animator.MoveTimelineCursor(mouseX);
        }


        private void TimelineCursor_MouseUp(object sender, MouseEventArgs e)
        {
            double mouseX = e.GetPosition(TimelineCanvas).X;
            animator.MoveTimelineCursor(mouseX);
        }

        private void TimelineCursor_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void TimelineCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }


        private void RefreshLayerListUI()
        {
            
            LayerList.Items.Clear();

            for (int i = 0; i < project.CurrentFrame.Layers.Count; i++)
            {

                ListBoxItem item = new ListBoxItem
                {
                    Content = $"Layer {i}",
                    Tag = project.CurrentLayer
                };
                item.MouseDown += OnLayerItem_Click;
                LayerList.Items.Add(item);
            }
            
        }

        public void OnLayerItem_Click (object sender, MouseButtonEventArgs e)
        {
            /*
            if (sender is ListBoxItem item && item.Tag is RasterLayer layer)
            {
                DM.SetActiveLayer(project.Frames[DM.activeFrameIndex].Layers.IndexOf(layer));
                RefreshLayerListUI();
            }
            */
        }



        public void DisplayTimeline(ListBox timelineList)
        {
            /*
            timelineList.Items.Clear();

            for (int i = 0; i < Frames.Count; i++)
            {

                ListBoxItem item = new ListBoxItem
                {

                    Content = $"Frame {i + 1}",
                    Tag = Frames[i]
                };

                timelineList.Items.Add(item);
            }
            */
        }

        public void UpdateTimeline(ListBox timelineList)
        {
            timelineList.Items.Clear();
        }



    }

}
