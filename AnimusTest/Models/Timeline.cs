﻿using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnimusTest.Models {

    public class Timeline {
        public List<Keyframe> Frames { get; set; } = new List<Keyframe>();

        public void AddTestData() {
            
            /*
            for (int i = 0; i < 3; i++) {
                var frame = new Keyframe { FrameNumber = i };
                var layer = new Layer { Name = $"Layer {i}" };

                Shape shape = i switch {
                    0 => new Polygon {
                        Points = new PointCollection {
                        new Point(10, 50), new Point(50, 10), new Point(90, 50)
                    },
                        Stroke = Brushes.Blue,
                        StrokeThickness = 2
                    }, // Трикутник

                    1 => new Ellipse {
                        Width = 40,
                        Height = 40,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2
                    }, // Коло

                    2 => new Rectangle {
                        Width = 40,
                        Height = 40,
                        Stroke = Brushes.Green,
                        StrokeThickness = 2
                    }, // Квадрат

                    _ => null
                };

                if (shape != null) {
                    Canvas.SetLeft(shape, 50);
                    Canvas.SetTop(shape, 50);
                    layer.Shapes.Add(shape);
                }

                frame.Layers.Add(layer);
                Frames.Add(frame);
            }
            */
        }

        public void DisplayTimeline(ListBox timelineList) {

            timelineList.Items.Clear();

            for (int i = 0; i < Frames.Count; i++) {

                ListBoxItem item = new ListBoxItem {

                    Content = $"Frame {i + 1}",
                    Tag = Frames[i] 
                };

                timelineList.Items.Add(item);
            }
        }

    }

}
