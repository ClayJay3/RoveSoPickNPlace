using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LasSharp;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kroft.Api.Entities
{
    public class LiDARDataRepository : ILiDARDataRepository
    {
        // Declare member variables.
        private readonly LiDARDatabase _LiDARDatabase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db">Implicitly passed in.</param>
        public LiDARDataRepository(LiDARDatabase db)
        {
            _LiDARDatabase = db;
        }

        public Task<bool> ComputeMetrics(string areaName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<LiDARPoint?> ComputeMetricsForPoint(string areaName, double x, double y, double z, double radius, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateBTreeIndex(string areaName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRTREEIndex(string areaName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRTREETriggers(string areaName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProcessLasFile(string areaName, LasReader lasData, string manualUTMZone = "", CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<LiDARPoint>> QueryLiDARPoints(string areaName, PointFilter pointFilter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
