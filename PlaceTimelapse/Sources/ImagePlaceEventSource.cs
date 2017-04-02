using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Sources
{
    public class ImagePlaceEventSource : IPlaceEventSource
    {
        private static readonly DateTime _unixDate = new DateTime(1970, 1, 1);

        private readonly DirectoryInfo _directory;
        private readonly IColorPalette _palette;
        private readonly ILogger _logger;

        public ImagePlaceEventSource(DirectoryInfo directory, IColorPalette palette, ILogger logger)
        {
            _directory = directory;
            _palette = palette;
            _logger = logger.ForContext<ImagePlaceEventSource>();
        }

        public IEnumerable<PlaceEvent> GetEvents()
        {
            Color[,] previousColors = null;

            foreach (FileInfo file in _directory.GetFiles())
            {
                _logger.Verbose("Parsing file: {file}", file);

                DateTime date = ParseDate(file.Name);

                using (Bitmap image = new Bitmap(file.FullName))
                {
                    BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    IntPtr ptr = data.Scan0;
                    byte[] rgbValues = new byte[image.Height * image.Width * 3];
                    Marshal.Copy(ptr, rgbValues, 0, rgbValues.Length);
                    image.UnlockBits(data);

                    Color[,] currentColors = new Color[image.Width, image.Height];

                    int width = image.Width;
                    int height = image.Height;

                    Task<List<PlaceEvent>>[] tasks = Enumerable.Range(0, width)
                       .Select(x => Task.Run(() => ProcessColumn(width, height, rgbValues, currentColors, previousColors, date, x).ToList()))
                       .ToArray();

                    Task.WaitAll(tasks);

                    IEnumerable < PlaceEvent > events = tasks.SelectMany(t => t.Result);

                    foreach (PlaceEvent placeEvent in events)
                        yield return placeEvent;

                    previousColors = currentColors;
                }
            }
        }

        private IEnumerable<PlaceEvent> ProcessColumn(int width, int height, byte[] rgbValues, Color[,] currentColors, Color[,] previousColors, DateTime date, int x)
        {
            for (int y = 0; y < height; y++)
            {
                int index = y * width + x;
                Color currentColor = Color.FromArgb(rgbValues[index * 3 + 2], rgbValues[index * 3 + 1], rgbValues[index * 3 + 0]);
                currentColors[x, y] = currentColor;
                if (previousColors == null || currentColor != previousColors[x, y])
                {
                    _logger.Verbose("Changed pixel: {x},{y]", x, y);

                    PlaceEvent placeEvent = new PlaceEvent()
                    {
                        X = x,
                        Y = y,
                        ColorId = _palette.GetColorId(currentColor),
                        Date = date
                    };

                    yield return placeEvent;
                }
            }
        }

        private DateTime ParseDate(string filename)
        {
            string datePart = filename.Split('.')[0];
            TimeSpan delta;
            if (datePart.Length == 10)
                delta = TimeSpan.FromSeconds(ulong.Parse(datePart));
            else
                delta = TimeSpan.FromMilliseconds(ulong.Parse(datePart));

            return _unixDate.Add(delta);
        }
    }
}
