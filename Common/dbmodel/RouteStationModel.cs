using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
   public class RouteStationModel
    {
        public Guid id { get; set; }
        public Guid routeId{ get; set; }
        public Guid stationId { get; set; }
        public DateTime arriveDate { get; set; }

    }
}
