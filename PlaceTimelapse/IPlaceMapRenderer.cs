using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceTimelapse
{
    public interface IPlaceMapRenderer
    {
        void RenderEvent(PlaceEvent placeEvent);
        Bitmap GetSnapshot();
    }
}
