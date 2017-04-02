using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Renderers
{
    public class DefaultPlaceMapRendererOptions
    {
        public int MarginSize { get; set; }
        public int TileSize { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public Color BaseColor { get; set; }
    }
}
