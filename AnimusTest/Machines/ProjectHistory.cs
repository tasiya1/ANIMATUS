using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AnimusTest.Machines
{
    public interface ProjectHistory
    {

        public void Undo() { }
        public void Redo() { }

        public void AddToHistory() { }

        public void KeepBufferBounds() { }

        public void SaveState() { }
    }
}
