using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimusTest.Models
{
    public class Project
    {
        public string Title { get; set; } = "New Project " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        public string Description { get; set; } = "No description";
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        public int Width { get; set; }
        public int Height { get; set; }
        public List<Frame> Frames { get; } = new();
        public int CurrentFrameIndex { get; set; }
        public Frame CurrentFrame => Frames[CurrentFrameIndex];

        public int CurrentLayerIndex { get; set; } = 0;
        public Layer CurrentLayer => CurrentFrame.Layers[CurrentLayerIndex];

        public bool isSaved { get; set; } = false;

        //public Bitmap Thumbnail { get; set; }

        public Project(int width, int height, int layerCountPerFrame)
        {
            this.Title = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            Width = width;
            Height = height;
            AddFrame(layerCountPerFrame);
        }

        public Project() {
            this.Title = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        public void AddFrame(int layerCount) {
            Frames.Add(new Frame(Width, Height, layerCount));
            CurrentFrameIndex = Frames.Count - 1;
        }

        public void RemoveFrame(int index) {
            if (index >= 0 && index < Frames.Count) Frames.RemoveAt(index);
            CurrentFrameIndex = Math.Clamp(CurrentFrameIndex, 0, Frames.Count - 1);
        }
    }
}
