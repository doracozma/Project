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

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {

        public IMediator mediator;

        public AuthenticateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<Response> Login(Login.Query login)
        {
            Response result = new Response();
            UserModel user = await this.mediator.Send(login);
            if (user != null)
            {
                result.add("jwt", TokenUtil.createToken(user.id));
            }
            return result;
        }

        [HttpPost]
        [Route("register")]
        public async Task<Response> Register(Register.Query register)
        {
            Response result = new Response();
            UserModel user = await this.mediator.Send(register);
            if (user != null)
            {
                result.add("jwt", TokenUtil.createToken(user.id));
            }
            return result;
        }
    }
}
