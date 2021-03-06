﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace App.Metrics.Graphite.Sandbox.Controllers
{
    [Route("api/[controller]")]
    public class ExceptionThrowingController : Controller
    {
        private readonly IMetrics _metrics;

        public ExceptionThrowingController(IMetrics metrics)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        [HttpGet]
        public async Task<int> Get()
        {
            await Task.Run(() =>
            {
                throw new Exception();
            });

            return 0;
        }
    }
}
