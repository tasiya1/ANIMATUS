using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Shapes;
using System.Windows.Ink;

namespace AnimusTest.Models
{
    public class DrawLineAction : IAction
    {
        private Stroke stroke;
        private Layer layer;

        public DrawLineAction(Stroke stroke, Layer layer)
        {
            this.stroke = stroke;
            this.layer = layer;
        }

        public void Undo() => layer.Remove(stroke);
        public void Redo() => layer.Add(stroke);
        public ActionType Type => ActionType.Stroke;


    }

    public class RemoveLineAction : IAction
    {
        private Stroke stroke;
        private Layer layer;

        public RemoveLineAction(Stroke stroke, Layer layer)
        {
            this.stroke = stroke;
            this.layer = layer;
        }

        public ActionType Type => ActionType.Delete;

        public void Undo() => layer.Add(stroke);
        public void Redo() => layer.Remove(stroke);
    }
}
