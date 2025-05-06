using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimusTest.Models;

namespace AnimusTest.Machines
{
    public class VectorProjectHistory : ProjectHistory
    {
        private Stack<IAction> undoStack = new();
        private Stack<IAction> redoStack = new();

        private int maxUndoCount = 50;

        public void Undo()
        {
            if (undoStack.Count == 0) return;

            var action = undoStack.Pop();
            action.Undo();
            redoStack.Push(action);
        }

        public void Redo()
        {
            if (redoStack.Count == 0) return;

            var action = redoStack.Pop();
            action.Redo();
            undoStack.Push(action);
        }

        public void AddToHistory(IAction action)
        {
            if (undoStack.Count >= maxUndoCount)
                undoStack.TrimExcess(); // або зробити shift (видалити найстаріший)

            undoStack.Push(action);
            redoStack.Clear(); // Після нової дії redo вже не актуальний
        }


        public bool CanUndo() => undoStack.Count > 0;
        public bool CanRedo() => redoStack.Count > 0;


        public void KeepBufferBounds()
        {
            while (undoStack.Count > maxUndoCount)
                undoStack.Pop();
        }

        public void SaveState()
        {

        }

        
    }
}
