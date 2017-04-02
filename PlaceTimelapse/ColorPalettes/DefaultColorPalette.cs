using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.ColorPalettes
{
    public class DefaultColorPalette : IColorPalette
    {
        private readonly Color[] _colors;

        public DefaultColorPalette()
        {
            _colors = new Color[]
            {
                Color.FromArgb(255, 255, 255),
                Color.FromArgb(228, 228, 228),
                Color.FromArgb(136, 136, 136),
                Color.FromArgb(34, 34, 34),
                Color.FromArgb(255, 167, 209),
                Color.FromArgb(229, 0, 0),
                Color.FromArgb(229, 149, 0),
                Color.FromArgb(160, 106, 66),
                Color.FromArgb(229, 217, 0),
                Color.FromArgb(148, 224, 68),
                Color.FromArgb(2, 190, 1),
                Color.FromArgb(0, 211, 221),
                Color.FromArgb(0, 131, 199),
                Color.FromArgb(0, 0, 234),
                Color.FromArgb(207, 110, 228),
                Color.FromArgb(130, 0, 128)
            };
        }

        public Color GetColor(int colorId)
        {
            return _colors[colorId];
        }
    }
}
