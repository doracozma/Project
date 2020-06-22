using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Station
{
    public class AddRouteObstacle
    {
        public class Query : IRequest<RouteObstacleModel>
        {
            public string routeId { get; set; }
            public Guid userId { get; set; }
            public Double latitude { get; set; }
            public Double longitude {get; set;}
            public int obstacleType { get; set; }
        }

        public class Hendler : IRequestHandler<Query, RouteObstacleModel>
        {

            DataContext context;
            public Hendler(DataContext context)
            {
                this.context = context;
            }

            public Task<RouteObstacleModel> Handle(Query request, CancellationToken cancellationToken)
            {
                Guid routeIdRequest = Guid.Parse(request.routeId);
                RouteModel route = this.context.route.SingleOrDefault(i => i.id == routeIdRequest);
               
                if (route != null)
                {
                    var routeObstacle = new RouteObstacleModel();
                    routeObstacle.id = Guid.NewGuid();
                    routeObstacle.routeId = routeIdRequest;
                    routeObstacle.latitude = request.latitude;
                    routeObstacle.longitude = request.longitude;
                    routeObstacle.userId = request.userId;
                    routeObstacle.obstacleType = request.obstacleType;
                    routeObstacle.seenDate = DateTime.Now;
                    this.context.routeObstacle.Add(routeObstacle);
                    this.context.SaveChanges();
                    return Task.FromResult(routeObstacle);
                }

                return Task.FromResult(default(RouteObstacleModel));

            }
        }
    }
}
