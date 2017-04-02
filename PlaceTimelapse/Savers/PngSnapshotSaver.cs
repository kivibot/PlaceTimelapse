using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Savers
{
    public class PngSnapshotSaver : ISnapshotSaver
    {
        private readonly DirectoryInfo _directory;
        private readonly ILogger _logger;

        private int _counter;

        public PngSnapshotSaver(DirectoryInfo directory, ILogger baseLogger)
        {
            if (!directory.Exists)
            {
                directory.Create();
                directory.Refresh();
            }
            _directory = directory;
            _logger = baseLogger.ForContext<PngSnapshotSaver>();
            _counter = 0;
        }

        public void Save(Bitmap snapshot)
        {
            string filename = String.Format("{0}.png", _counter.ToString("D10"));
            string path = Path.Combine(_directory.FullName, filename);

            _logger.Debug("Saving current snapshot to file: {filename}", filename);

            snapshot.Save(path, ImageFormat.Png);

            _counter++;
        }
    }
}
