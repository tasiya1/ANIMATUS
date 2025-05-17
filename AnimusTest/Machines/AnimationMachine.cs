using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using AnimusTest.Models;
using AnimusTest.Views;

namespace AnimusTest.Machines
{
    public class AnimationMachine
    {
        public Project project { get; }
        public Canvas TimelineCanvas { get; set; }
        private readonly Action requestRedraw;
        private readonly Action requestUpdateScale;

        private Polygon currentFrameUIEl = null;
        private Polyline cursor = null;
        private System.Windows.Media.Brush diamondFill = new SolidColorBrush(Color.FromRgb(168, 85, 247));
        private System.Windows.Media.Brush diamondBorder = new SolidColorBrush(Color.FromRgb(141, 54, 224));
        private int fps = 24;

        private List<Polygon> FrameFigurines { get; set; } = new List<Polygon>();


        public AnimationMachine(Project project, Canvas timelineCanvas, Action redraw, Action requestUpdateScale)
        {
            this.project = project;
            this.TimelineCanvas = timelineCanvas;
            this.requestRedraw = redraw; // буде подавати запит на перемальовування канвасу, коли користувач перемикає кадр
            this.requestUpdateScale = requestUpdateScale;
        }







        public void RenderTimelineUI()
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

            for (int i = 0; i < project.Frames.Count; i++)
            {
                Models.Frame frame = project.Frames[i];


                Polygon diamond = new Polygon
                {
                    Points = new PointCollection
                        {
                            new Point(0, 5),
                            new Point(5, 10),
                            new Point(0, 15),
                            new Point(-5, 10)
                        },
                    Fill = diamondFill,
                    Stroke = diamondBorder,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(diamond, i * frameWidth + startOffset + frameWidth);
                Canvas.SetTop(diamond, 50);

                int frameIndex = i;
                diamond.Tag = frame;
                diamond.MouseDown += OnKeyframeUI_Click;

                TimelineCanvas.Children.Add(diamond);

                FrameFigurines.Add(diamond);


            }
            for (int i = 0; i < ((TimelineCanvas.Width - startOffset) / 20); i++)
            {
                int computedX = i * 20;
                if (i % 5 == 0)
                {

                    TextBlock textBlock = new TextBlock();
                    textBlock.Foreground = Brushes.Gray;
                    textBlock.Text = i.ToString();
                    Canvas.SetLeft(textBlock, computedX - 5 + startOffset);
                    Canvas.SetTop(textBlock, 3);

                    TimelineCanvas.Children.Add(textBlock);
                }


                polyline = new Polyline
                {
                    Points = new PointCollection { new Point(computedX + startOffset, 20), new Point(computedX + startOffset, 35) },

                    Stroke = (i % 5 == 0) ? Brushes.Gray : Brushes.DarkGray,
                    StrokeThickness = 1
                };


                TimelineCanvas.Children.Add(polyline);


            }

            cursor ??= new Polyline
            {
                Points = new PointCollection { new Point(startOffset + frameWidth, 0), new Point(startOffset + frameWidth, TimelineCanvas.Height) },
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
            TimelineCanvas.Children.Add(cursor);

        }


        private void OnKeyframeUI_Click(object sender, MouseButtonEventArgs e)
        {
            
            if (sender is Polygon polygon && polygon.Tag is Models.Frame frame)
            {
                if (currentFrameUIEl != null)
                {
                    currentFrameUIEl.Stroke = diamondBorder;
                }
                currentFrameUIEl = polygon;
                currentFrameUIEl.Stroke = Brushes.Yellow;
                
                project.CurrentFrameIndex = project.Frames.IndexOf(frame);
                DisplayFrame(project.CurrentFrame);
                requestUpdateScale?.Invoke();

                //MessageBox.Show($"Selected frame: {currentFrame.title}");
            }
            

        }


        public async void PlayAnimation()
        {
            
            for (int i = 0; i < project.Frames.Count; i++)
            {
                //currentFrame = timeline.Frames[i];
                project.CurrentFrameIndex = i;
                Canvas.SetLeft(cursor, i * 20);
                RenderFrame(project.Frames[i]);
                await Task.Delay(1000 / fps);
            }
            DisplayFrame(project.CurrentFrame);
            
        }

        private void DisplayFrame(object currentFrame)
        {
            //throw new NotImplementedException();
            requestRedraw?.Invoke();
        }

        private void RenderFrame(Models.Frame frame)
        {
            //throw new NotImplementedException();
            requestRedraw?.Invoke();
        }

        private void StopAnimation()
        {

        }

        private void PauseAnimation()
        {

        }


        private void TimelineCursor_MouseDown()
        {
            /*
            double mouseX = e.GetPosition(TimelineCanvas).X;
            MoveTimelineCursor(mouseX);
            */
        }

        public void MoveTimelineCursor(double mouseX)
        {
            
            int startOffset = 70;

            int snappedFrame = (int)Math.Round((mouseX - startOffset) / 20.0);
            int newPosition = snappedFrame * 20;

            Canvas.SetLeft(cursor, newPosition);

            project.CurrentFrameIndex = snappedFrame;

            DisplayFrame(project.CurrentFrame);

            SelectFrameOnUI();
            
        }

        private void SelectFrameOnUI()
        {
            
            if (currentFrameUIEl != null)
            {
                currentFrameUIEl.Stroke = diamondBorder;
            }

            for (int i = 0; i < FrameFigurines.Count; i++)
            {
                if (FrameFigurines[i].Tag == project.CurrentFrame)
                {
                    currentFrameUIEl = FrameFigurines[i];
                    currentFrameUIEl.Stroke = Brushes.Yellow;
                }
            }
            

        }

        private void TimelineCursor_MouseUp()
        {
            /*
            double mouseX = e.GetPosition(TimelineCanvas).X;
            MoveTimelineCursor(mouseX);
            */
        }

        private void TimelineCursor_MouseMove()
        {

        }

        private void TimelineCanvas_MouseUp()
        {

        }



    }
}
