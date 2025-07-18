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
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

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
        private float scaleDPI_X;
        private float scaleDPI_Y;

        private Color _selectedColor = Colors.Blue;
        private const int MaxColorsInHistory = 10;
        private List<Color> colorHistory = new();
        public float BrushSizeBinded { get; private set; }
        public bool ShowPreviousFrame { get; set; } = true;

        private DrawingMachine DM;
        private Project project;
        public AnimationMachine animator;
        private RenderMachine renderer;
        private FileController fileController;
        private SKSurface SurfaceCanvas;

        

        public IllustratorWindow()
        {
            InitializeComponent();

            SkiaCanvas.PaintSurface += SkiaCanvas_PaintSurface;
            var width = 1500;//(int)SkiaCanvas.ActualWidth;
            var height = 1000;//(int)SkiaCanvas.ActualHeight;

            project = new Project(width, height, 3);
            DM = new DrawingMachine(project, () => SkiaCanvas.InvalidateVisual());
            renderer = new RenderMachine(project);
            animator = new AnimationMachine(project, TimelineCanvas, () => SkiaCanvas.InvalidateVisual(), () => UpdateScale());
            fileController = new();

            BrushList.SelectedIndex = 0;// зразу встановлю дефолтний вибраний пензль
            //додати тестові фрейми
            for (int i = 0; i < 68; i++)
            {
                project.Frames.Add(new Models.Frame(width, height, 3));
            }
            /*MessageBox.Show("Actual size before load. \nWidth: " + SkiaCanvas.ActualWidth + "\nHeight: " + SkiaCanvas.ActualHeight + 
                "Size before load. \nWidth: " + SkiaCanvas.Width + "\nHeight: " + SkiaCanvas.Height);*/

            SkiaCanvas.Loaded += (s, e) =>
            {
                /*MessageBox.Show("Actual size after load. \nWidth: " + SkiaCanvas.ActualWidth + "\nHeight: " + SkiaCanvas.ActualHeight +
                "Size after load. \nWidth: " + SkiaCanvas.Width + "\nHeight: " + SkiaCanvas.Height);*/
                ActualCanvasSize.Text = $"Actual Canvas Size: \nWidth: {SkiaCanvas.ActualWidth}\nHeight: {SkiaCanvas.ActualHeight}";
                ProstoCanvasSize.Text = $"Canvas Size: \nWidth: {SkiaCanvas.Width}\nHeight: {SkiaCanvas.Height}";
            };
            //Loaded += (s, e) => UpdateScale();
            //SkiaCanvas.SizeChanged += (s, e) => UpdateScale(); // якщо розмір 

            RefreshLayerListUI();
            animator.RenderTimelineUI();
            animator.RegenerateFrameCache();

        }

        private void SkiaCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            if (ShowPreviousFrame && project.CurrentFrameIndex > 1)
            {
                if (animator.frameCache.Count > project.CurrentFrameIndex - 1)
                {
                    var prevBmp = animator.frameCache[project.CurrentFrameIndex - 1];
                    if (prevBmp != null && prevBmp.Height > 0 && prevBmp.Width > 0)
                    {
                        using var paint = new SKPaint { Color = SKColors.Red.WithAlpha(80) };
                        canvas.DrawBitmap(prevBmp, SKPoint.Empty, paint);
                    }
                }
            }


            var frame = project.CurrentFrame;
            foreach (var layer in frame.Layers)
            {
                if (!layer.IsVisible || layer.Bitmap == null) continue;
                canvas.DrawBitmap(layer.Bitmap, SKPoint.Empty);
            }

            scaleDPI_X = e.Info.Width / (float)SkiaCanvas.ActualWidth;
            scaleDPI_Y = e.Info.Height / (float)SkiaCanvas.ActualHeight;

            FromInfoSize.Text = $"From Info Size: \nWidth: {e.Info.Width}\nHeight: {e.Info.Height}";
        }

        private void SkiaCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //DM.isDrawing = true;
            previousPoint = GetScaledPoint(e.GetPosition(SkiaCanvas));

            DM.StartDrawing(previousPoint);
            LayerCanvasSize.Text = $"Current layer Canvas Size: \nWidth: {project.CurrentLayer.Bitmap.Width}\nHeight: {project.CurrentLayer.Bitmap.Height}";
        }

        private void SkiaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //_lastMousePos = e.GetPosition(SkiaCanvas);
            if (!DM.isDrawing) return;
            /*
            GlobalCoordinates.Text = $"Global Coordinates: \n{e.GetPosition(this)}";
            SkiaCanvasCoordinates.Text = $"Local Coordinates: \n{e.GetPosition(SkiaCanvas)}";
            ScaledCoordinates.Text = $"Scaled Coordinates: \n{GetScaledPoint(e.GetPosition(SkiaCanvas))}";
            */
            var currentPoint = GetScaledPoint(e.GetPosition(SkiaCanvas));
            DM.ContinueDrawing(currentPoint);
            SkiaCanvas.InvalidateVisual();
            Console.WriteLine(e.GetPosition(SkiaCanvas) + "- коорди скіканвасу\n" + currentPoint + "- коорди заскейлені\n\n");
        }

        private void SkiaCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DM.EndDrawing();
            SkiaCanvas.InvalidateVisual();
        }

        private SKPoint GetScaledPoint(Point p)
        {
            return (new SKPoint((float)p.X * scaleDPI_X, (float)p.Y * scaleDPI_Y));
        }


        public void UpdateScale()
        {

            var layer = project.CurrentLayer;
            if (layer?.Bitmap == null || SkiaCanvas.ActualWidth == 0 || SkiaCanvas.ActualHeight == 0)
                return;

            scaleX = (float)layer.Bitmap.Width / (float)SkiaCanvas.ActualWidth;
            scaleY = (float)layer.Bitmap.Height / (float)SkiaCanvas.ActualHeight;
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

            if (LayerList.SelectedIndex >= 0) DM.SetActiveLayer(LayerList.SelectedIndex);
        }

        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveLayerButton_Click(object sender, RoutedEventArgs e)
        {

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
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Animus Project File|*.animusproj",
                Title = "Save Project",
                FileName = "project"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                FileController.SaveProjectToProjectFile(project, path);
                MessageBox.Show("Project saved successfully!");
            }
            else
            {
                MessageBox.Show("Save operation was cancelled.");
            }
            
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Animus Project File|*.animusproj",
                Title = "Open Project"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                try
                {
                    project = FileController.LoadProjectFromProjectFile(path); // генерування параметрів вікна заново
                    project.Title = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    animator = new AnimationMachine(project, TimelineCanvas, () => SkiaCanvas.InvalidateVisual(), () => UpdateScale());
                    animator.RegenerateFrameCache();
                    DM = new DrawingMachine(project, () => SkiaCanvas.InvalidateVisual());
                    
                    renderer = new RenderMachine(project);
                    
                    SkiaCanvas.InvalidateVisual();
                    
                    RefreshLayerListUI();
                    animator.RenderTimelineUI();
                    MessageBox.Show("Project opened successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open project: {ex.Message}");
                }
            }
        }

        private void SaveProjectAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportProject_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (project.Frames.Count == 0)
            {
                MessageBox.Show("No frames to export.");
                return;
            }
            // Save the project as a video or image sequence
            fileController.ExportProject(project);
            MessageBox.Show("Project exported successfully!");
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
                var layer = project.CurrentFrame.Layers[i];

                CheckBox visibilityCheckBox = new CheckBox
                {
                    IsChecked = layer.IsVisible,
                    Margin = new Thickness(0, 0, 5, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                visibilityCheckBox.PreviewMouseRightButtonDown += (s, e) => e.Handled = true;
                visibilityCheckBox.PreviewMouseRightButtonUp += (s, e) => e.Handled = true;
                visibilityCheckBox.MouseRightButtonDown += (s, e) => e.Handled = true;
                visibilityCheckBox.MouseRightButtonUp += (s, e) => e.Handled = true;

                visibilityCheckBox.Checked += (s, e) =>
                {
                    layer.IsVisible = true;
                    SkiaCanvas.InvalidateVisual();
                };
                visibilityCheckBox.Unchecked += (s, e) =>
                {
                    layer.IsVisible = false;
                    SkiaCanvas.InvalidateVisual();
                };

                TextBlock text = new TextBlock
                {
                    Text = $"Layer {i+1}",
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center
                };

                StackPanel panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                panel.Children.Add(visibilityCheckBox);
                panel.Children.Add(text);

                ListBoxItem item = new ListBoxItem
                {
                    Content = panel,
                    Tag = layer
                };

                item.PreviewMouseLeftButtonDown += OnLayerItem_Click;

                LayerList.Items.Add(item);
            }
        }


        public void OnLayerItem_Click(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBoxItem item && item.Tag is Layer layer)
            {
                int index = project.CurrentFrame.Layers.IndexOf(layer);
                DM.SetActiveLayer(index);
                RefreshLayerListUI();
                //MessageBox.Show($"Selected layer: (Index: {index})");
            }

        }

        private void AddColorToHistory(Color newColor)
        {
            if (colorHistory.Count > 0 && colorHistory.Last() == newColor) //щоб не дублювати останній колір
                return;

            colorHistory.Add(newColor);

            if (colorHistory.Count > MaxColorsInHistory)
                colorHistory.RemoveAt(0);

            RefreshColorHistoryPanel();
        }

        private void RefreshColorHistoryPanel()
        {
            ColorHistoryPanel.Children.Clear();

            foreach (var color in colorHistory)
            {
                var rect = new Border
                {
                    Width = 20,
                    Height = 20,
                    Background = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B)),
                    Margin = new Thickness(2),
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.White,
                    Cursor = Cursors.Hand,
                    ToolTip = $"#{color.R:X2}{color.G:X2}{color.B:X2}"
                };

                rect.MouseLeftButtonDown += (s, e) =>
                {
                    _selectedColor = color;
                    ColorPicker.SelectedColor = Color.FromRgb(color.R, color.G, color.B);
                    DM.currentBrush.TintBrush(new SKColor(color.R, color.G, color.B));

                };

                ColorHistoryPanel.Children.Add(rect);
            }
        }

        public void PublishProject_ClickAsync(object sender, RoutedEventArgs e)
        {

            PublishProjectWindow publishProjectWindow = new(project);
            publishProjectWindow.Show();
            
        }

        public void BrushList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushList.SelectedIndex >= 0 && BrushList.SelectedIndex < DM.Brushes.Count)
            {
                DM.setBrush(BrushList.SelectedIndex, ColorPicker.SelectedColor, BrushSizeBinded);
            }
        }

        private void ColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                _selectedColor = e.NewValue.Value;

                DM.currentBrush.TintBrush(
                    new SKColor(_selectedColor.R, _selectedColor.G, _selectedColor.B, _selectedColor.A)
                );
                //ColorDisplay.Background = new SolidColorBrush(_selectedColor);
                
            }
        }

        private void ColorPicker_ColorSelected(object sender, RoutedEventArgs e) =>
            AddColorToHistory(_selectedColor);

        public void ShowOrHideAnimationPanel(object sender, RoutedEventArgs e)
        {
            if ((bool)AnimationPanelVisibilityCheckbox.IsChecked)
            {
                AnimationPanel.Visibility = Visibility.Visible;
            }
            else
            {
                AnimationPanel.Visibility = Visibility.Collapsed;
            }
        }

        public void ExportToImage_Click(object sender, RoutedEventArgs e)
        {
            //fileController.ExportToImage(project, SurfaceCanvas);
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Save Canvas as Image",
                FileName = "canvas_image"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                FileController.SaveCurrentCanvasToFile(RedrawOnCanvasSurface(), path);
            }
        }

        public void BrushSizeSlider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BrushSizeBinded = (float)e.NewValue;
            if (BrushSizeLabel!=null) BrushSizeLabel.Text = e.NewValue.ToString();
            if (DM != null)
            {
                DM.currentBrush.UpdateBrushSize(BrushSizeBinded);
                DM.Eraser.StrokeWidth = BrushSizeBinded;
            }

        }

        private SKSurface RedrawOnCanvasSurface()
        {
            var info = new SKImageInfo(1500, 1000);
            var surface = SKSurface.Create(info);
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            foreach (var layer in project.CurrentFrame.Layers)
            {
                if (!layer.IsVisible || layer.Bitmap == null) continue;

                using var paint = new SKPaint
                {
                    Color = SKColors.White.WithAlpha((byte)(layer.Opacity * 255)),
                    IsAntialias = true
                };

                canvas.DrawBitmap(layer.Bitmap, SKPoint.Empty, paint);
            }

            //canvas.Flush(); //може це воно сповільнювало відмальовку?

            return surface;
        }

        public void ExportToSeries_Click(object sender, RoutedEventArgs e)
        {
            FileController.ExportToSeriesOfImages(project, @"E:\Animatus\Output\" + project.Title);
        }
        
        public void ExportToVideo_Click(object sender, RoutedEventArgs e)
        {
            //fileController.ExportToVideo(project, SurfaceCanvas);
            /*
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "MP4 Video|*.mp4",
                Title = "Save Animation Movie",
                FileName = "animation_rendered"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                FileController.save(project, path);
            }*/
            var watch = System.Diagnostics.Stopwatch.StartNew();

            FileController.SaveProjectAsAnimationMovie(project);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show($"Export To Video. Time Elapsed: \nElapsed time: {elapsedMs} ms");
        }
    }

}
