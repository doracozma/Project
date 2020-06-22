using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common;
using Persistence;
using Application;
using System.Linq;
using MediatR;
using System.Dynamic;
using Application.Authenticate;
using Serilog;
using Database;
using Microsoft.AspNetCore.Routing;
using Application.Station;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {

        public IMediator mediator;

        public StationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("get-route")]
        public async Task<Response> GetRoute(GetRoute.Query station)
        {
            Response result = new Response();
            List<StationModel> stations = await this.mediator.Send(station);
            if (station != null)
            {
                result.add("stations", stations);
            }
            return result;
        }


        [HttpPost]
        [Route("get-all-routes")]
        public async Task<Response> GetAllRoutes(GetAllRoutes.Query route)
        {
            Response result = new Response();
            List<RouteModel> routes = await this.mediator.Send(route);
            if (route != null)
            {
                result.add("routes", routes);
            }
            return result;
        }

        [HttpPost]
        [Route("get-route-obstacle")]
        public async Task<Response> GetRouteObstacl(GetRouteObstacle.Query obstacle)
        {
            Response result = new Response();
            List<RouteObstacleModel> routeObstacle = await this.mediator.Send(obstacle);
            if (routeObstacle != null)
            {
                result.add("routeObstacle", routeObstacle);
            }
            return result;
        }

        [TypeFilter(typeof(AnyUserFilter))]
        [HttpPost]
        [Route("add-route-obstacle")]
        public async Task<Response> AddRouteObstacle(AddRouteObstacle.Query routeObstacle)
        {
            Response result = new Response();
            Guid userId = (Guid)(HttpContext.Items["userId"]);
            RouteObstacleModel routes = await this.mediator.Send(routeObstacle);
            if (routeObstacle != null)
            {
                result.add("routes", routes);
            }
            return result;
        }

    }
}
