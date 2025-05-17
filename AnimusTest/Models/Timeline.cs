using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnimusTest.Models {

    public class Timeline
    {
        public List<Frame> Frames { get; set; } = new List<Frame>();
        public double duration = 10000; // playtime duration in ms
        public int fps = 24;

        public void AddTestData()
        {

        }

        public void DisplayTimeline(ListBox timelineList)
        {

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
        }

        public void UpdateTimeline(ListBox timelineList)
        {
            timelineList.Items.Clear();
        }


    }
}
