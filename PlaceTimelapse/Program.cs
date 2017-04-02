using PlaceTimelapse.ColorPalettes;
using PlaceTimelapse.Renderers;
using PlaceTimelapse.Savers;
using PlaceTimelapse.Sources;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .WriteTo.ColoredConsole()
                 .CreateLogger();

            try
            {
                IColorPalette palette = new DefaultColorPalette();
                DefaultPlaceMapRendererOptions rendererOptions = new DefaultPlaceMapRendererOptions()
                {
                    BaseColor = Color.FromArgb(255, 255, 255),
                    MapHeight = 1000,
                    MapWidth = 1000,
                    MarginSize = 0,
                    TileSize = 1
                };
                DefaultPlaceMapRenderer renderer = new DefaultPlaceMapRenderer(rendererOptions, palette);

                DirectoryInfo directory = new DirectoryInfo("images");
                ISnapshotSaver saver = new PngSnapshotSaver(directory, logger);

                string csvPath = "export.csv";
                CsvPlaceEventSource eventSourceA = new CsvPlaceEventSource(csvPath, logger);

                DirectoryInfo eventImageDirectoryB = new DirectoryInfo("input");
                ImagePlaceEventSource eventSourceB = new ImagePlaceEventSource(eventImageDirectoryB, new DefaultColorPalette(), logger);

                DirectoryInfo eventImageDirectoryC = new DirectoryInfo("input2");
                ImagePlaceEventSource eventSourceC = new ImagePlaceEventSource(eventImageDirectoryC, new DefaultColorPalette(), logger);

                CompositePlaceEventSource eventSourceABC = new CompositePlaceEventSource(eventSourceA, new CompositePlaceEventSource(eventSourceB, eventSourceC));

                RedundantFilteringPlaceEventSource eventSource = new RedundantFilteringPlaceEventSource(eventSourceABC, 1001, 1001);

                TimeSpan cycle = TimeSpan.FromSeconds(5); 
                TimelapseManager manager = new TimelapseManager(eventSource, renderer, saver, cycle, logger);

                manager.CreateTimelapse();

                eventSourceA.Dispose();
                renderer.Dispose();
            }
            catch (Exception ex)
            {
                logger.Fatal("Exception: {ex}", ex);
            }
            finally
            {
                ((IDisposable)logger).Dispose();
            }
        }
    }
}
