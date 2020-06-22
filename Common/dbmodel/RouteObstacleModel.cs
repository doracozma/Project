using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
     public class RouteObstacleModel
    {
        public Guid id { get; set; }
        public Guid routeId { get; set; }
        public int obstacleType { get; set; }
        public DateTime seenDate { get; set; }
        public Guid userId { get; set; }
        public Double latitude {get; set; }
        public Double longitude {get; set;}
    }
}
