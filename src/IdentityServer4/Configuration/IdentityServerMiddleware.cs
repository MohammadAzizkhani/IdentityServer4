﻿using IdentityServer4.Core.Endpoints;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Core.Configuration
{
    public class IdentityServerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IdentityServerOptions _options;

        public IdentityServerMiddleware(RequestDelegate next, IdentityServerOptions options, ILoggerFactory loggerFactory)
        {
            _next = next;
            _options = options;
            _logger = loggerFactory.CreateLogger<IdentityServerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/token")))
            {
                var obj = context.ApplicationServices.GetService(typeof(TokenEndpoint));
                var endpoint = obj as IEndpoint;

                if (endpoint != null)
                {
                    await endpoint.ProcessAsync(context);
                    return;
                }
            }

            await _next(context);
        }
    }
}