using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Renderers
{
    public class DefaultPlaceMapRenderer : IPlaceMapRenderer, IDisposable
    {
        private readonly int _marginSize;
        private readonly int _tileSize;
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Color _baseColor;
        private readonly IColorPalette _palette;

        private Bitmap _bitmap;
        private Graphics _graphics;

        public DefaultPlaceMapRenderer(DefaultPlaceMapRendererOptions options, IColorPalette palette)
        {
            _mapWidth = options.MapWidth;
            _mapHeight = options.MapHeight;
            _marginSize = options.MarginSize;
            _tileSize = options.TileSize;
            _baseColor = options.BaseColor;
            _palette = palette;

            _bitmap = new Bitmap(_mapWidth * (_tileSize + _marginSize), _mapHeight * (_tileSize + _marginSize));
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(_baseColor);
        }

        public Bitmap GetSnapshot()
        {
            return _bitmap;
        }

        public void RenderEvent(PlaceEvent placeEvent)
        {
            int bitmapX = placeEvent.X * (_tileSize + _marginSize);
            int bitmapY = placeEvent.Y * (_tileSize + _marginSize);

            Brush brush = new SolidBrush(_palette.GetColor(placeEvent.ColorId));

            _graphics.FillRectangle(brush, bitmapX, bitmapY, _tileSize, _tileSize);
        }

        public void Dispose()
        {
            _graphics.Dispose();
            _bitmap.Dispose();
        }

    }
}
