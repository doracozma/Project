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
    public class GetAllRoutes
    {
        public class Query : IRequest<List<RouteModel>>
        {

        }

        public class Hendler : IRequestHandler<Query, List<RouteModel>>
        {

            DataContext context;
            public Hendler(DataContext context)
            {
                this.context = context;
            }

            public Task<List<RouteModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<RouteModel> routes = this.context.route.ToList();
                return Task.FromResult(routes);

            }
        }
    }
}
