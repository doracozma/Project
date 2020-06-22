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
    public class GetRoute
    {
        public class Query : IRequest <List<StationModel>>
        {
            public string id { get; set; }
        }

        public class Hendler : IRequestHandler<Query, List<StationModel>>
        {

            DataContext context;
            public Hendler(DataContext context)
            {
                this.context = context;
            }

            public Task<List<StationModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guid idRequest = Guid.Parse(request.id);
               RouteModel route = this.context.route.SingleOrDefault(i => i.id == idRequest);
                if (route != null)
                {
                    List<RouteStationModel> routeStations = this.context.routeStation.Where(i => i.routeId == idRequest).ToList();
                    List<Guid> stationIds = routeStations.Select(it => it.stationId).ToList();
                    List<StationModel> stations = this.context.station.Where(i => stationIds.Contains(i.id)).ToList();
                    return Task.FromResult(stations);

                }


                return Task.FromResult(new  List<StationModel>());
            }
        }
    }
}
