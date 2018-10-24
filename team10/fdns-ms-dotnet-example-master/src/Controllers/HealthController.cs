#pragma warning disable 1591 // disables the warnings about missing Xml code comments

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.HealthChecks;

namespace Foundation.Example.WebUI.Controllers
{
    /// <summary>
    /// Controller that provides APIs for health checking and health monitoring of this microservice
    /// </summary>
    [Route("api/1.0")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheck;

        public HealthController(IHealthCheckService healthCheck)
        {
            _healthCheck = healthCheck;
        }

        // GET api/1.0/health
        /// <summary>
        /// Gets a health check of this microservice
        /// </summary>
        /// <returns>CompositeHealthCheckResult</returns>
        [Produces("application/json")]
        [HttpGet("health")]
        public async Task<IActionResult> Index()
        {
            var timedTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var stopwatch = Stopwatch.StartNew();
            var checkResult = await _healthCheck.CheckHealthAsync(timedTokenSource.Token);
            return Ok(checkResult);
        }
    }
}

#pragma warning restore 1591