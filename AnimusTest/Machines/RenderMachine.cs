using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimusTest.Models;
using SkiaSharp;

namespace AnimusTest.Machines
{
    public class RenderMachine
    {
        private readonly Project project;
        public RenderMachine(Project project) => this.project = project;

        public void Render(SKCanvas canvas)
        {
            /*
            canvas.Clear(SKColors.White);
            foreach (var layer in project.CurrentFrame.Layers)
            {
                if (!layer.IsVisible) continue;
                using var paint = new SKPaint { Color = SKColors.White.WithAlpha((byte)(layer.Opacity * 255)) };
                canvas.DrawBitmap(layer.Bitmap, 0, 0, paint);
            }
            */
        }

        // написати статичну функцію перетворення звичайних координат з вікна на координати для малювання на скіашарп канвасі
    }
}
