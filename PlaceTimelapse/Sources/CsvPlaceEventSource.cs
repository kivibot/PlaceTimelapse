using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Sources
{
    public class CsvPlaceEventSource : IPlaceEventSource, IDisposable
    {
        private static readonly DateTime _unixDate = new DateTime(1970, 1, 1);

        private readonly TextReader _reader;
        private readonly ILogger _logger;

        public CsvPlaceEventSource(string path, ILogger baseLogger)
            : this(new FileStream(path, FileMode.Open), baseLogger)
        {
        }

        public CsvPlaceEventSource(Stream stream, ILogger baseLogger)
        {
            _reader = new StreamReader(stream);
            _logger = baseLogger.ForContext<CsvPlaceEventSource>();
        }

        public IEnumerable<PlaceEvent> GetEvents()
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                _logger.Verbose("Parsing line: {line}", line);

                string[] tokens = line.Split(new string[] { "\",\"" }, StringSplitOptions.None)
                    .Select(token => token.Replace("\"", ""))
                    .ToArray();

                PlaceEvent placeEvent = new PlaceEvent()
                {
                    Id = long.Parse(tokens[0]),
                    X = int.Parse(tokens[1]),
                    Y = int.Parse(tokens[2]),
                    Username = tokens[3],
                    ColorId = int.Parse(tokens[4]),
                    Date = _unixDate.Add(TimeSpan.FromMilliseconds(ulong.Parse(tokens[5])))
                };

                yield return placeEvent;
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
