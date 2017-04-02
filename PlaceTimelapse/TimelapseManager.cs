using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse
{
    public class TimelapseManager
    {
        private readonly IPlaceEventSource _eventSource;
        private readonly IPlaceMapRenderer _renderer;
        private readonly ISnapshotSaver _saver;
        private readonly TimeSpan _cycle;
        private readonly ILogger _logger;

        public TimelapseManager(IPlaceEventSource eventSource, IPlaceMapRenderer renderer, ISnapshotSaver saver, TimeSpan cycle, ILogger logger)
        {
            _eventSource = eventSource;
            _renderer = renderer;
            _saver = saver;
            _cycle = cycle;
            _logger = logger.ForContext<TimelapseManager>();
        }

        public void CreateTimelapse()
        {
            _logger.Information("Creating a timelapse.");

            DateTime lastSnapshotDate = DateTime.MinValue;
            
            foreach (PlaceEvent placeEvent in _eventSource.GetEvents())
            {
                if (lastSnapshotDate + _cycle < placeEvent.Date)
                {
                    CreateSnapshot();

                    lastSnapshotDate = placeEvent.Date;
                }

                _renderer.RenderEvent(placeEvent);
            }

            CreateSnapshot();

            _logger.Information("Done.");
        }

        private void CreateSnapshot()
        {
            _logger.Debug("Creating a snapshot.");

            Bitmap snapshot = _renderer.GetSnapshot();
            _saver.Save(snapshot);
        }

    }
}
