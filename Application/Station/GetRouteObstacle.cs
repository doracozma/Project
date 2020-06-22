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
    public class GetRouteObstacle
    {
        public class Query : IRequest<List<RouteObstacleModel>>
        {
            public string routeId { get; set; }

        }

        public class Hendler : IRequestHandler<Query, List<RouteObstacleModel>>
        {

            DataContext context;
            public Hendler(DataContext context)
            {
                this.context = context;
            }

            public Task<List<RouteObstacleModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guid routeIdRequest = Guid.Parse(request.routeId);
                RouteModel route = this.context.route.SingleOrDefault(i => i.id == routeIdRequest);
                if(route != null)
                {
                    List<RouteObstacleModel> routeObstacle = this.context.routeObstacle.Where(i => i.routeId == routeIdRequest).ToList();
                    return Task.FromResult(routeObstacle);
                }


                return Task.FromResult(new List<RouteObstacleModel>());



            }
        }
    }
}
