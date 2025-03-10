using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Extensions.ServiceExtensions
{
    public static class HttpContentRegister
    {
        public static void AddHttpContent(this IServiceCollection services) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

    }//Class_end
}
