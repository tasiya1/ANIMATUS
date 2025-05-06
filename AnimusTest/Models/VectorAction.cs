using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AnimusTest.Models
{

    public interface IAction
    {
        void Undo();
        void Redo();
        ActionType Type { get; }
    }

    public enum ActionType
    {
        Stroke,
        Fill,
        Move,
        Delete,
        Transform
        // 
    }



}
