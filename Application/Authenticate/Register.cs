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
    public class Register
    {
        public class Query : IRequest<UserModel>
        {
            public string email { get; set; }
            public string password { get; set; }
            public string name { get; set; }
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
                var emailRequest = request.email.Trim();
                var user =this.context.user.SingleOrDefault(i => i.email == emailRequest );
                if(user == null)
                {
                    user = new UserModel();
                    user.creationDate = DateTime.Now;
                    user.id = Guid.NewGuid();
                    user.email = emailRequest;
                    user.name = request.name;
                    user.password = request.password;
                    this.context.user.Add(user);
                    this.context.SaveChanges();
                }
                return Task.FromResult(user);
            }
        }
    }
}
