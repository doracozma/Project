namespace API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using JWT;
    using JWT.Serializers;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Persistence;
    using Common;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class AnyUserFilter : ActionFilterAttribute
    {
        public DataContext dataContext { get; set; }
        public AnyUserFilter(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {

            try
            {
                actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
                var json = TokenUtil.decodeToken(token);
                Dictionary<string, object> payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Guid userId = Guid.Parse((String)payload.GetValueOrDefault("userId", null));

                UserModel user = this.dataContext.user.Find(userId);
                if (user == null)
                {
                    var response = actionContext.HttpContext.Response;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    actionContext.Result = new EmptyResult();
                }
                actionContext.HttpContext.Items.Add("userId", userId);
            }
            catch (Exception)
            {
                var response = actionContext.HttpContext.Response;
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                actionContext.Result = new EmptyResult();
            }
        }
    }
}