﻿using PlaceTimelapse.ColorPalettes;
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
                CsvPlaceEventSource eventSource = new CsvPlaceEventSource(csvPath, logger);

                TimeSpan cycle = TimeSpan.FromMinutes(0.5);
                TimelapseManager manager = new TimelapseManager(eventSource, renderer, saver, cycle, logger);

                manager.CreateTimelapse();

                eventSource.Dispose();
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
