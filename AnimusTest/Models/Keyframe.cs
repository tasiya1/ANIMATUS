﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimusTest.Models {
    public class Keyframe {
        public int FrameNumber { get; set; }
        public string title;

        public bool isKey = false;
        public List<Layer> Layers { get; set; } = new List<Layer>();

        public Keyframe(string title) { 
            this.title = title;
        }

    }
}
