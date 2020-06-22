using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Authenticate
{
    public class Login
    {
        public class Query : IRequest<UserModel>
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class Hendler : IRequestHandler<Query, UserModel>
        {

            DataContext context;
            public Hendler(DataContext context)
            {
                this.context = context;
            }

            public Task<UserModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var emailRequest =request.email.Trim();
                UserModel user = this.context.user.SingleOrDefault(user => user.email == emailRequest && user.password == request.password);
                return Task.FromResult(user);
            }
        }
    }
}
