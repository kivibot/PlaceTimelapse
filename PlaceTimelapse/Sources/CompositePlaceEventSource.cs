using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse.Sources
{
    public class CompositePlaceEventSource : IPlaceEventSource
    {
        private readonly IPlaceEventSource _sourceA;
        private readonly IPlaceEventSource _sourceB;

        public CompositePlaceEventSource(IPlaceEventSource sourceA, IPlaceEventSource sourceB)
        {
            _sourceA = sourceA;
            _sourceB = sourceB;
        }

        public IEnumerable<PlaceEvent> GetEvents()
        {
            IEnumerator<PlaceEvent> eventsA = _sourceA.GetEvents().GetEnumerator();
            IEnumerator<PlaceEvent> eventsB = _sourceB.GetEvents().GetEnumerator();

            if (eventsA.MoveNext() && eventsB.MoveNext())
            {
                while (true)
                {
                    if (eventsA.Current.Date <= eventsB.Current.Date)
                    {
                        yield return eventsA.Current;
                        if (!eventsA.MoveNext())
                            break;
                    }
                    else
                    {
                        yield return eventsB.Current;
                        if (!eventsB.MoveNext())
                            break;
                    }
                }
            }

            if (eventsA.Current != null)
            {
                while (eventsA.MoveNext())
                    yield return eventsA.Current;
            }
            if (eventsB.Current != null)
            {
                while (eventsB.MoveNext())
                    yield return eventsB.Current;
            }
        }
    }
}
