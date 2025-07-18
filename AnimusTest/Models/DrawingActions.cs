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

        public ActionType Type => ActionType.Stroke;

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    
    }
}
