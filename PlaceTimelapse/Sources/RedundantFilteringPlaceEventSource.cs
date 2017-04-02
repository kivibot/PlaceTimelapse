using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Sources
{
    public class RedundantFilteringPlaceEventSource : IPlaceEventSource
    {
        private readonly IPlaceEventSource _source;
        private readonly int[,] _colorIds;

        public RedundantFilteringPlaceEventSource(IPlaceEventSource source, int width, int height)
        {
            _source = source;
            _colorIds = new int[width, height];
        }
        
        public IEnumerable<PlaceEvent> GetEvents()
        {
            foreach (PlaceEvent placeEvent in _source.GetEvents())
            {
                if (_colorIds[placeEvent.X, placeEvent.Y] == placeEvent.ColorId)
                    continue;
                _colorIds[placeEvent.X, placeEvent.Y] = placeEvent.ColorId;
                yield return placeEvent;
            }
        }
    }
}
