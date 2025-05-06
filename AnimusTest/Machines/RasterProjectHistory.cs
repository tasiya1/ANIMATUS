using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace AnimusTest.Machines
{
    public class RasterProjectHistory : ProjectHistory
    {

        public List<SKBitmap> UndoBuffer = new();
        public List<SKBitmap> RedoBuffer = new();

        /*
         
        
        ДУЖЕ ХОРОША ІДЕЯ: ЗБЕРІГАТИ В БУФЕРІ НЕ ВЕСЬ БІТМАП, А ЛИШЕ ТУ ЙОГО ЧАСТИНКУ, ЯКА БУЛА ЗМІНЕНА!!!!!
          

         */

        public void Undo()
        {
            // Implement undo logic for raster graphics
        }
        public void Redo()
        {
            // Implement redo logic for raster graphics
        }
        public void AddToHistory()
        {
            // Implement logic to add current state to history
        }

        public void KeepBufferBounds()
        {

        }

        public void SaveState()
        {

        }
    }
}
